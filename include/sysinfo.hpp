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
#include <unistd.h>    // For getcwd()
#include <limits.h>    // For PATH_MAX

// This doesnt really belong here but fuck it
std::string getCurrentWorkingDirectory() {
    char buffer[PATH_MAX];
    if (getcwd(buffer, sizeof(buffer)) != nullptr) {
        return std::string(buffer);
    } else {
        perror("getcwd error");
        return "";  // Return an empty string on failure
    }
}

utsname getKernelInfo() {
    // Display kernel version
    struct utsname buffer;
    if (uname(&buffer) != 0) { // Uname is the syscall the linux kernel uses for its info
        std::cerr << "Unable to get kernel version" << std::endl; // If we get here then it failed to get the kernel version for whatever reason and we must then throw an error
    }
    return buffer; // we will return the entire buffer so we can get anything we want from it, not just the kernel version
}

std::string getCPUInfo() {
    std::ifstream cpuinfo("/proc/cpuinfo");
    std::string line;

    if (cpuinfo.is_open()) {
        while (std::getline(cpuinfo, line)) {
            if (line.find("model name") != std::string::npos) {
                return line.substr(line.find(":") + 2);
                break; // Only get the first occurrence
            }
        }
        cpuinfo.close();
    } else {
        std::cerr << "Unable to open /proc/cpuinfo" << std::endl;
    }
}

// Function to get GPU information using lspci command
std::string getGPUInfo() {
    std::array<char, 128> buffer;
    std::string result;
    FILE* pipe = popen("lspci | grep VGA", "r");
    if (!pipe) {
        return "";
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
            return result;
        }
    }
}

// Function to get RAM information from /proc/meminfo
double getRAMInfo() {
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
                    
                    return gb;
                }
                break;
            }
        }
        meminfo.close();
    }
}

void displayKernelInfo() {
    struct utsname buffer = getKernelInfo();
    std::cout << "\033[1mKernel\033[0m " << buffer.release << std::endl; // the kernel version is spesifically stored in .release.
}

void displayCPUInfo() {
    std::cout << "\033[1mProcessor\033[0m " << getCPUInfo() << std::endl;
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
    std::cout << "\033[1mMemory\033[0m " << getRAMInfo() << " GB" << std::endl;
}

#endif SYSINFO_HPP