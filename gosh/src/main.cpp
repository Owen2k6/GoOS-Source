#include <iostream>
#include <string>
#include <vector>
#include <unistd.h>   // For fork(), execvp()
#include <sys/wait.h> // For waitpid()
#include <cerrno>     // For error handling with errno and perror
#include <cstring>    // For strerror()
#include "../../lib/sysinfo.hpp"

std::string prompt = "$ ";

void genPrompt() {
    std::string user = getlogin();
    std::string hostname = getKernelInfo().nodename;
    std::string cwd = getCurrentWorkingDirectory();

    prompt = "\033[38;5;123m" + user + "\033[38;5;226m@\033[38;5;123m" + hostname + " \033[38;5;46m" + cwd + "/\033[0m";
}

void putPrompt() {
    std::cout << prompt;
}

std::vector<std::string> parseInput(const std::string &input) {
    std::vector<std::string> tokens;
    size_t pos = 0, found;
    while ((found = input.find_first_of(' ', pos)) != std::string::npos) {
        tokens.push_back(input.substr(pos, found - pos));
        pos = found + 1;
    }
    tokens.push_back(input.substr(pos));
    return tokens;
}

// Function to handle built-in commands
bool handleBuiltinCommand(const std::vector<std::string> &args) {
    if (args.empty()) return false;

    if (args[0] == "cd") {
        if (args.size() < 2) {
            std::cerr << "cd: missing argument\n";
        } else if (chdir(args[1].c_str()) != 0) {
            std::cerr << "cd: " << strerror(errno) << "\n";
        }
        genPrompt();
        return true;
    }
    // Add other built-ins here in the future if needed

    return false; // Not a built-in command
}

void executeCommand(const std::vector<std::string> &args) {
    // Check for built-in commands first
    if (handleBuiltinCommand(args)) {
        return; // Built-in command handled, return without forking
    }

    // Convert args to a char* array for execvp()
    std::vector<char*> argv;
    for (const auto &arg : args) {
        argv.push_back(const_cast<char*>(arg.c_str()));
    }
    argv.push_back(nullptr);

    // Fork the process for non-built-in commands
    pid_t pid = fork();
    if (pid == 0) {
        // Child process
        execvp(argv[0], argv.data());
        perror("gosh"); // If execvp fails
        exit(1);
    } else if (pid > 0) {
        // Parent process
        waitpid(pid, nullptr, 0); // Wait for child process to finish
    } else {
        // Fork failed
        perror("gosh");
    }
}

int main() {
    genPrompt();
    std::string input;
    while (true) {
        putPrompt();
        std::getline(std::cin, input);
        if (input.empty()) continue;
        if (input == "exit") break;

        auto args = parseInput(input);
        executeCommand(args);
    }
    return 0;
}