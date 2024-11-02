#include <iostream>
#include "../../lib/sysinfo.hpp"

int main() {
  //std::cout << "GoOS version 1.6 Scafell Pike\n";
  std::cout << "\033[1mGoOS\033[0m Scaffel Pike\nVersion 1.6\n" << std::endl;

  // these functions are all located in sysinfo.hpp, normally it would be in a .cpp but i got lazy and wrote it all into the header file.
  // thought we could include some more info, i tried to order it somewhat like the macOS "About my Mac" menu is.
  displayKernelInfo();
  displayCPUInfo();
  displayRAMInfo();
  displayGPUInfo();

  return 0;
}