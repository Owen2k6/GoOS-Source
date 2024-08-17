using MOOS;
using System.Runtime;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MOOS.FS;
using MOOS.Misc;

unsafe class Program
{
    static void Main() { }

    public static ACFFont lg12;
    public static ACFFont lg18;
    public static ACFFont lg24;
    public static ACFFont lg36;

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
        byte[] lg12Raw = File.ReadAllBytes("LucidaGrande12.acf");
        byte[] lg18Raw = File.ReadAllBytes("LucidaGrande18.acf");
        byte[] lg24Raw = File.ReadAllBytes("LucidaGrande24.acf");
        byte[] lg36Raw = File.ReadAllBytes("LucidaGrande36.acf");
       
        lg12 = new ACFFont(lg12Raw);
        lg18 = new ACFFont(lg18Raw);
        lg24 = new ACFFont(lg24Raw);
        lg36 = new ACFFont(lg36Raw);

        Console.Clear();
        Console.WriteLine("GoOS 1.6 NAOT");

        lg12.DrawString(50, 53 + 12, "Testicles", Color.White, Framebuffer.Graphics);
        lg18.DrawString(50, 53 + 18 + 5, "Testicles", Color.White, Framebuffer.Graphics);
        lg24.DrawString(50, 53 + 24 + 10, "Testicles", Color.White, Framebuffer.Graphics);
        lg36.DrawString(50, 53 + 36 + 15, "Testicles", Color.White, Framebuffer.Graphics);
        for (; ; ) 
        {
            Thread.Sleep(10);

            Framebuffer.Graphics.FillRectangle(50, 50, 10, 10, 0xFFFFFFFF);
        }
    }
}