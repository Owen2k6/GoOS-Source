using Internal.Runtime.CompilerServices;
using MOOS;
using MOOS.Driver;
using MOOS.Misc;
using System.Runtime;
using System.Runtime.InteropServices;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

public static class IDT
{
    [DllImport("*")]
    private static extern unsafe void set_idt_entries(void* idt);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct IDTEntry
    {
        public ushort BaseLow;
        public ushort Selector;
        public byte Reserved0;
        public byte Type_Attributes;
        public ushort BaseMid;
        public uint BaseHigh;
        public uint Reserved1;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IDTDescriptor
    {
        public ushort Limit;
        public ulong Base;
    }

    private static IDTEntry[] idt;
    public static IDTDescriptor idtr;


    public static bool Initialised { get; private set; }


    public static unsafe bool Initialise()
    {
        idt = new IDTEntry[256];

        set_idt_entries(Unsafe.AsPointer(ref idt[0]));

        fixed (IDTEntry* _idt = idt)
        {
            idtr.Limit = (ushort)((sizeof(IDTEntry) * 256) - 1);
            idtr.Base = (ulong)_idt;
        }

        Native.Load_IDT(ref idtr);

        Initialised = true;
        return true;
    }

    public static void Enable()
    {
        Native.Sti();
    }

    public static void Disable()
    {
        Native.Cli();
    }

    public struct RegistersStack 
    {
        public ulong rax;
        public ulong rcx;
        public ulong rdx;
        public ulong rbx;
        public ulong rsi;
        public ulong rdi;
        public ulong r8;
        public ulong r9;
        public ulong r10;
        public ulong r11;
        public ulong r12;
        public ulong r13;
        public ulong r14;
        public ulong r15;
    }

    //https://os.phil-opp.com/returning-from-exceptions/
    public struct InterruptReturnStack
    {
        public ulong rip;
        public ulong cs;
        public ulong rflags;
        public ulong rsp;
        public ulong ss;
    }

    public struct IDTStackGeneric
    {
        public RegistersStack rs;
        public ulong errorCode;
        public InterruptReturnStack irs;
    }

    [RuntimeExport("intr_handler")]
    public static unsafe void intr_handler(int irq, IDTStackGeneric* stack)
    {
        if(irq < 0x20)
        {
            Panic.Error($" A cpu with the ID of {SMP.ThisCPU} has called a panic requiring the system to lock up.", true);
            InterruptReturnStack* irs;
            switch (irq)
            {
                case 8:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 17:
                case 21:
                case 29:
                case 30:
                    irs = (InterruptReturnStack*)(((byte*)stack) + sizeof(RegistersStack));
                    break;

                default:
                    irs = (InterruptReturnStack*)(((byte*)stack) + sizeof(RegistersStack) + sizeof(ulong));
                    break;
            }
            Console.ForegroundColour = System.ConsoleColour.Blue;
            Console.WriteLine($" RIP: 0x{stack->irs.rip.ToString("x2")}");
            Console.WriteLine($" Code Segment: 0x{stack->irs.cs.ToString("x2")}");
            Console.WriteLine($" RFlags: 0x{stack->irs.rflags.ToString("x2")}");
            Console.WriteLine($" RSP: 0x{stack->irs.rsp.ToString("x2")}");
            Console.WriteLine($" Stack Segment: 0x{stack->irs.ss.ToString("x2")}");
            Console.ForegroundColour = System.ConsoleColour.Yellow;
            switch (irq)
            {
                case 0: Console.WriteLine(" The CPU tried to divide a number by zero, which is not allowed. \n This usually happens due to a bug in the programme where it tried to perform an invalid mathematical operation."); break;
                case 1: Console.WriteLine(" The CPU encountered a debugging operation and paused execution. \n This usually happens when a programme is being debugged step by step. \n If you're not actively debugging, this might indicate a serious issue in the programme’s execution flow."); break;
                case 2: Console.WriteLine(" A critical hardware signal was sent to the CPU that cannot be ignored. \n This interrupt usually indicates an emergency situation, such as hardware failure or a watchdog timer triggering due to an unresponsive system."); break;
                case 3: Console.WriteLine(" The CPU hit a breakpoint, which is a deliberate stopping point used by developers to debug the programme. \n If you're not debugging, encountering a breakpoint could indicate an issue with the programme's execution."); break;
                case 4: Console.WriteLine(" The CPU encountered a mathematical overflow, where a calculation produced a result too large to be stored in the available space. \n This often happens when working with very large numbers and can lead to incorrect results or crashes."); break;
                case 5: Console.WriteLine(" The CPU detected an attempt to access data outside the allowed memory range. \n This means a programme tried to read or write data beyond its limits, which could cause instability or data corruption."); break;
                case 6: Console.WriteLine(" The CPU tried to execute an operation that it does not understand. \n This error occurs when a programme sends an instruction to the CPU that isn’t valid, possibly due to a corrupted programme or bug in the software."); break;
                case 7: Console.WriteLine(" The CPU attempted to use a math coprocessor (like for floating-point calculations), but it was either not present or not available. \n This could indicate an issue with the system's configuration or the software trying to use features that the CPU doesn’t support."); break;
                case 8: Console.WriteLine(" A critical error occurred while the CPU was trying to handle another error. \n This usually means the system encountered a very serious issue and was unable to recover, leading to a complete crash."); break;
                case 9: Console.WriteLine(" The CPU encountered an issue with the math coprocessor, where an operation went beyond the expected data range. \n This can happen due to a bug in how data is handled during complex calculations."); break;
                case 10: Console.WriteLine(" The CPU tried to switch to another task but found that the task’s state information was invalid or corrupted. \n This could be due to a problem in how tasks are managed by the operating system."); break;
                case 11: Console.WriteLine(" The CPU tried to access a segment of memory that wasn’t available. \n This usually indicates a serious problem in memory management, where the programme or operating system is trying to access memory that doesn’t exist or is not accessible."); break;
                case 12: Console.WriteLine(" The CPU encountered an error while handling the stack, which is a special memory area used to store temporary data like function calls and local variables. \n This could be due to a stack overflow, where the stack runs out of space, or a stack underflow, where it tries to access non-existent data"); break;
                case 13: Console.WriteLine(" The CPU encountered a general protection fault, which means a programme tried to perform an operation it wasn't allowed to, \n such as accessing restricted memory or executing an invalid instruction. This is a sign of a serious problem in the programme or system."); break;
                case 14:
                    ulong CR2 = Native.ReadCR2();
                    if ((CR2 >> 5) < 0x1000)
                    {
                        Console.WriteLine(" The CPU tried to use a pointer (a reference to a memory location) that was set to 'null,' meaning it pointed to nothing. \n This usually happens due to a bug in the programme, where it tried to access memory that doesn't exist.");
                    }
                    else
                    {
                        Console.WriteLine(" The CPU tried to access a section of memory that isn't currently available. \n This could happen if the program tried to access memory that hasn't been allocated or if there’s an issue with how memory is being managed. \n Sometimes, the system can recover from this, but it can also cause a crash.");
                    }
                    break;
                case 16: Console.WriteLine(" An error occurred with the math coprocessor during a calculation. \n This might indicate a hardware issue with the coprocessor or a software bug where the CPU and coprocessor are not communicating correctly."); break;
                default: Console.WriteLine(" The CPU encountered an error it didn't recognise. \n This means something went wrong, but the system couldn't identify the exact cause. \n It could be due to a hardware issue or an unexpected software problem."); break;
            }
            Framebuffer.Update();
            for (; ; );
        }

        //DEAD
        if(irq == 0xFD) 
        {
            Native.Cli();
            Native.Hlt();
            for (; ; ) Native.Hlt();
        }

        //For main processor
        if (SMP.ThisCPU == 0)
        {
            //System calls
            if (irq == 0x80)
            {
                var pCell = (MethodFixupCell*)stack->rs.rcx;
                string name = string.FromASCII(pCell->Module->ModuleName, strings.strlen((byte*)pCell->Module->ModuleName));
                stack->rs.rax = (ulong)API.HandleSystemCall(name);
                name.Dispose();
            }
            switch (irq)
            {
                case 0x20:
                    //misc.asm Schedule_Next
                    if (stack->rs.rdx != 0x61666E6166696E)
                        Timer.OnInterrupt();
                    break;
            }
            Interrupts.HandleInterrupt(irq);
        }

        if (irq == 0x20)
        {
            ThreadPool.Schedule(stack);
        }

        Interrupts.EndOfInterrupt((byte)irq);
    }
}