# Directories
EXCLUDE_DIR = bin

# Find all subdirectories excluding the specified directory
SUBDIRS := $(filter-out $(EXCLUDE_DIR)/, $(wildcard */))

# Default target to run all found Makefiles
all: $(SUBDIRS)

# Rule to invoke Makefiles in each subdirectory
$(SUBDIRS):
	@$(MAKE) -C $@

# Clean target
clean:
	rm -rf ./bin/*

# Phony targets
.PHONY: all clean $(SUBDIRS)