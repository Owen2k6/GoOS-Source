#ifndef SYSINFO_HPP
#define SYSINFO_HPP

#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <sys/utsname.h>
#include <array>
#include <cstdio>
#include <regex>
#include <cmath>

void displayKernelInfo() {
    // Display kernel version
    struct utsname buffer;
    if (uname(&buffer) != 0) { // Uname is the syscall the linux kernel uses for its info
        std::cerr << "Unable to get kernel version" << std::endl; // If we get here then it failed to get the kernel version for whatever reason and we must then throw an error
    }
    std::cout << "\033[1mKernel\033[0m " << buffer.release << std::endl; // the kernel version is spesifically stored in .release.
}

void displayCPUInfo() {
    std::ifstream cpuinfo("/proc/cpuinfo");
    std::string line;

    if (cpuinfo.is_open()) {
        while (std::getline(cpuinfo, line)) {
            if (line.find("model name") != std::string::npos) {
                std::cout << "\033[1mProcessor\033[0m " << line.substr(line.find(":") + 2) << std::endl;
                break; // Only get the first occurrence
            }
        }
        cpuinfo.close();
    } else {
        std::cerr << "Unable to open /proc/cpuinfo" << std::endl;
    }
}

// Function to get GPU information using lspci command
void displayGPUInfoa() {
    std::array<char, 128> buffer;
    std::string result;
    
    FILE* pipe = popen("lspci | grep VGA", "r");
    if (!pipe) {
        return;
    }
    
    while (fgets(buffer.data(), buffer.size(), pipe) != nullptr) {
        result += buffer.data();
    }
    
    pclose(pipe);
    
    if (!result.empty()) {
        std::cout << "\033[1mGraphics:\033[0m " << result;
    }
}

void displayGPUInfo() {
    std::array<char, 128> buffer;
    std::string result;
    FILE* pipe = popen("lspci | grep VGA", "r");
    if (!pipe) {
        return;
    }
    while (fgets(buffer.data(), buffer.size(), pipe) != nullptr) {
        result += buffer.data();
    }
    pclose(pipe);
    if (!result.empty()) {
        // Find the position after "VGA compatible controller: "
        size_t startPos = result.find("VGA compatible controller: ");
        if (startPos != std::string::npos) {
            // Remove everything before and including "VGA compatible controller: "
            result = result.substr(startPos + 27);
            std::cout << "\033[1mGraphics\033[0m " << result;
        }
    }
}

// Function to get RAM information from /proc/meminfo
void displayRAMInfo() {
    std::ifstream meminfo("/proc/meminfo");
    std::string line;
    
    if (meminfo.is_open()) {
        while (std::getline(meminfo, line)) {
            if (line.find("MemTotal") != std::string::npos) {
                // Extract just the number using regex
                std::regex numbers("\\d+");
                std::smatch matches;
                
                if (std::regex_search(line, matches, numbers)) {
                    // Convert kB to GB (divide by 1024*1024)
                    double gb = std::stod(matches[0]) / (1024.0 * 1024.0);
                    // Round to 1 decimal place
                    gb = std::round(gb * 10.0) / 10.0;
                    
                    std::cout << "\033[1mMemory\033[0m " << gb << " GB" << std::endl;
                }
                break;
            }
        }
        meminfo.close();
    }
}

#endif SYSINFO_HPP