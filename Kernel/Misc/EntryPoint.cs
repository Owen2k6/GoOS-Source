using Internal.Runtime.CompilerHelpers;
using MOOS;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Graph;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MOOS.Misc
{
    internal static unsafe class EntryPoint
    {
        [RuntimeExport("Entry")]
        public static void Entry(MultibootInfo* Info, IntPtr Modules, IntPtr Trampoline)
        {
            Allocator.Initialise((IntPtr)0x20000000);

            StartupCodeHelpers.InitialiseModules(Modules);

            PageTable.Initialise();

            ASC16.Initialise();

            VBEInfo* info = (VBEInfo*)Info->VBEInfo;
            if (info->PhysBase != 0)
            {
                Framebuffer.Initialise(info->ScreenWidth, info->ScreenHeight, (uint*)info->PhysBase);
                Framebuffer.Graphics.Clear(0x0);
            }
            else 
            {
                for (; ; ) Native.Hlt();
            }

            Console.Setup();
            IDT.Disable();
            GDT.Initialise();
            IDT.Initialise();
            Interrupts.Initialise();
            IDT.Enable();

            SSE.enable_sse();
            //AVX.init_avx();

            ACPI.Initialise();
#if UseAPIC
            PIC.Disable();
            LocalAPIC.Initialise();
            IOAPIC.Initialise();
#else
        PIC.Enable();
#endif
            Timer.Initialise();

            Keyboard.Initialise();

            Serial.Initialise();

            PS2Controller.Initialise();
            VMwareTools.Initialise();

            SMBIOS.Initialise();

            PCI.Initialise();

            IDE.Initialise();
            SATA.Initialise();

            ThreadPool.Initialise();

            Console.WriteLine($"[SMP] Trampoline: 0x{((ulong)Trampoline).ToString("x2")}");
            Native.Movsb((byte*)SMP.Trampoline, (byte*)Trampoline, 512);

            SMP.Initialise((uint)SMP.Trampoline);

            //Only fixed size vhds are supported!
            Console.Write("[Initrd] Initrd: 0x");
            Console.WriteLine((Info->Mods[0]).ToString("x2"));
            Console.WriteLine("[Initrd] Initializing Ramdisk");
            new Ramdisk((IntPtr)(Info->Mods[0]));
            //new FATFS();
            new TarFS();

            KMain();
        }

        [DllImport("*")]
        public static extern void KMain();
    }
}