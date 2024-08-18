using MOOS;
using System.Runtime;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MOOS.FS;
using MOOS.Misc;
using MOOS.Driver;
using MOOS.Graph;
using static MOOS.NETv4;
using GoOS.GUI;
using GoOS;
using Control = System.Windows.Forms.Control;
using GoOS.GUI.Apps;

unsafe class Program
{
    static void Main() { }

    public static WindowManager wm;
    public static ProcessManager pm;

    public static ACFFont lg12;
    public static ACFFont lg18;
    public static ACFFont lg24;
    public static ACFFont lg36;

    public static Image scafellPike;

    public static Bitmap mouse;

    /*
     * Minimum system requirement:
     * 1024MiB of RAM
     * Memory Map:
     * 256 MiB - 512MiB   -> System
     * 512 MiB - ∞     -> Free to use
     */
    //Check out Kernel/Misc/EntryPoint.cs
    [RuntimeExport("KMain")]
    static void KMain() 
    {
        Console.Clear();

        Hub.Initialize();
        HID.Initialize();
        EHCI.Initialize();

        if (HID.Mouse != null)
        {
            Console.Write("[Warning] Press please press Mouse any key to validate USB Mouse ");
            bool res = Console.Wait(&USBMouseTest, 2000);
            Console.WriteLine();
            if (!res)
            {
                lock (null)
                {
                    USB.NumDevice--;
                    HID.Mouse = null;
                }
            }
        }

        if (HID.Keyboard != null)
        {
            Console.Write("[Warning] Press please press any key to validate USB keyboard ");
            bool res = Console.Wait(&USBKeyboardTest, 2000);
            Console.WriteLine();
            if (!res)
            {
                lock (null)
                {
                    USB.NumDevice--;
                    HID.Keyboard = null;
                }
            }
        }

        USB.StartPolling();

        //Use qemu for USB debug
        //VMware won't connect virtual USB HIDs
        if (HID.Mouse == null)
        {
            //Console.WriteLine("USB Mouse not present");
        }
        if (HID.Keyboard == null)
        {
            //Console.WriteLine("USB Keyboard not present");
        }

        static bool USBMouseTest()
        {
            HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out var Buttons);
            return Buttons != MouseButtons.None;
        }

        static bool USBKeyboardTest()
        {
            HID.GetKeyboardThings(HID.Keyboard, out var ScanCode, out var Key);
            return ScanCode != 0;
        }

        byte[] lg12Raw = File.ReadAllBytes("Resources/Fonts/LucidaGrande12.acf");
        byte[] lg18Raw = File.ReadAllBytes("Resources/Fonts/LucidaGrande18.acf");
        byte[] lg24Raw = File.ReadAllBytes("Resources/Fonts/LucidaGrande24.acf");
        byte[] lg36Raw = File.ReadAllBytes("Resources/Fonts/LucidaGrande36.acf");

        byte[] scafellPikeRaw = File.ReadAllBytes("Resources/Images/ScafellPike.png");

        byte[] mouseRaw = File.ReadAllBytes("Resources/Images/mouse.bmp"); 
       
        lg12 = new ACFFont(lg12Raw);
        lg18 = new ACFFont(lg18Raw);
        lg24 = new ACFFont(lg24Raw);
        lg36 = new ACFFont(lg36Raw);

        Image scafellPikePre = new PNG(scafellPikeRaw);
        scafellPike = scafellPikePre.ResizeImage(Framebuffer.Width, Framebuffer.Height);
        scafellPikePre.Dispose();

        mouse = new Bitmap(mouseRaw);

        Framebuffer.TripleBuffered = true;

        wm = new WindowManager();
        pm = new ProcessManager();

        Test testProcess = new Test();
        pm.addProcess(testProcess);

        for (; ; )
        {
            Framebuffer.Graphics.DrawImage(0, 0, scafellPike, false);

            pm.Execute();
            wm.Render(Framebuffer.Graphics);

            Framebuffer.Graphics.DrawImage(Control.MousePosition.X, Control.MousePosition.Y, mouse);

            Framebuffer.Update();
        }
    }
}