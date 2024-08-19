using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    // This data structure is a contract with the compiler. It holds the address of a static
    // constructor and a flag that specifies whether the constructor already executed.
    [StructLayout(LayoutKind.Sequential)]
    public struct StaticClassConstructionContext
    {
        // Pointer to the code for the static class constructor method. This is initialised by the
        // binder/runtime.
        public IntPtr cctorMethodAddress;

        // Initialization state of the class. This is initialised to 0. Every time managed code checks the
        // cctor state the runtime will call the classlibrary's CheckStaticClassConstruction with this context
        // structure unless initialised == 1. This check is specific to allow the classlibrary to store more
        // than a binary state for each cctor if it so desires.
        public int initialised;
    }
}