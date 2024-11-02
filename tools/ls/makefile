# Directories
APP_NAME = ls
SRC_DIR = ./
OBJ_DIR = ../obj
BIN_DIR = ../bin

# Files
SRC = $(wildcard $(SRC_DIR)/*.cpp)
OBJ = $(SRC:$(SRC_DIR)/%.cpp=$(OBJ_DIR)/%.o)
TARGET = $(BIN_DIR)/$(APP_NAME)

# Detect OS and set compiler
ifeq ($(shell uname), Darwin)
    ifeq ($(shell uname -m), arm64)
        CC = aarch64-linux-gnu-g++
    else
        CC = x86_64-linux-gnu-g++
    endif
else
    CC = g++
endif

# Compilation flags
CXXFLAGS = -Wall -Wextra -std=c++23
LDFLAGS =

# Default target
all: $(TARGET)
	rm -rf $(OBJ_DIR)

# Create output directories if they don't exist
$(OBJ_DIR) $(BIN_DIR):
	mkdir -p $@

# Compile and link
$(TARGET): $(OBJ) | $(BIN_DIR)
	$(CC) $(OBJ) -o $(TARGET) $(LDFLAGS)
	rm -f $(OBJ)  # Clean up object files after linking

# Compile source files to object files in obj directory
$(OBJ_DIR)/%.o: $(SRC_DIR)/%.cpp | $(OBJ_DIR)
	$(CC) $(CXXFLAGS) -c $< -o $@

# Run the application
run:
	@./$(TARGET)

# Phony targets
.PHONY: all run