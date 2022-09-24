﻿using Cosmos.HAL;
using Cosmos.System.Graphics;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using System.IO;
using System.Linq.Expressions;
using Cosmos.Core.Memory;
using System.Drawing;
using IL2CPU.API.Attribs;
using System.Text;
using CosmosTTF;
using TechOS.System;
using TechOS.GUI;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using Cosmos.Core;
using Cosmos.Core.Memory;
using CosmosFtpServer;
using Cosmos.System.Network.IPv4.UDP;
using System.Diagnostics;
using GoOS;
using Cosmos.HAL.BlockDevice.Registers;
using System.Threading;

//Goplex Studios - GoOS
//Copyright (C) 2022  Owen2k6
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace GoOS
{



    public class Kernel : Sys.Kernel
    {
        //GoOS Core
        public void print(string str)
        {
            Console.WriteLine(str);
        }
        public void log(System.ConsoleColor colour, string str)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(str);
        }
        public void write(string str)
        {
            Console.Write(str);
        }
        public void textcolour(System.ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
        }
        public void highlightcolour(System.ConsoleColor colour)
        {
            Console.BackgroundColor = colour;
        }
        public void sleep(int time)
        {
            Thread.Sleep(time);
        }
        //Core end




        //[ManifestResourceStream(ResourceName = "Wallpaper.bmp")]
        //public static byte[] Wallpaper;
        //public static Bitmap wallpaper = new Bitmap(Wallpaper);
        public static Canvas canvas;


        private Boolean adminconsoledisk = false;


        //Login Data
        private Boolean loginsystemenabled = false;
        private String username = null;
        private String password = null;



        private static Sys.FileSystem.CosmosVFS FS;
        public static string file;

        private static string request = string.Empty;
        private static TcpClient tcpc = new TcpClient(80);
        private static Address dns = new Address(8, 8, 8, 8);
        private static EndPoint endPoint = new EndPoint(dns, 80);
        public static bool ParseHeader()
        {
            return false;
        }
        bool isenabled = true;
        public static VGAScreen VScreen = new VGAScreen();
        /* SNAKE VARS */
        public int[] matrix;
        public List<int[]> commands;
        public List<int[]> snake;
        public List<int> food;
        public int randomNumber;
        public int snakeCount;
        Random rnd = new Random();

        protected override void BeforeRun()
        {

            try
            {
                NetworkDevice nic = NetworkDevice.GetDeviceByName("eth0"); //get network device by name
                IPConfig.Enable(nic, new Address(192, 168, 1, 32), new Address(255, 255, 255, 0), new Address(192, 168, 1, 254)); //enable IPv4 configuration
                using (var xClient = new DHCPClient())
                {
                    /** Send a DHCP Discover packet **/
                    //This will automatically set the IP config after DHCP response
                    xClient.SendDiscoverPacket();
                }
                using (var xClient = new DnsClient())
                {
                    xClient.Connect(new Address(192, 168, 1, 254)); //DNS Server address

                    /** Send DNS ask for a single domain name **/
                    xClient.SendAsk("github.com");

                    /** Receive DNS Response **/
                    Address destination = xClient.Receive(); //can set a timeout value


                }

            }
            catch
            {
                log(ConsoleColor.Green, "Error starting Goplex Web Interface.");
                log(ConsoleColor.Green, "The system will proceed to boot without networking.");
                log(ConsoleColor.Green, "Press ENTER to continue (and yes it has to be ENTER)");
                Console.ReadLine();
            }

            Console.Clear();
            log(ConsoleColor.Red, "                    GGGGGGGGGGGG                   ");
            log(ConsoleColor.DarkRed, "               GGGGGGGGGGGGGGGGGGGGGG              ");
            log(ConsoleColor.Magenta, "  GGGGGGGGGG GGGGGGGGGGGGGGGGGGGGGGGGGG            ");
            log(ConsoleColor.DarkMagenta, "  GGGGGGGG   GGGGGGGGG        GGGGGG               ");
            log(ConsoleColor.Red, "  GGGGGGG    GGGGG                                 ");
            log(ConsoleColor.DarkRed, "  GGGGGG     GGG                                   ");
            //Do NOT change owen.
            textcolour(ConsoleColor.Magenta);
            write("  GGGGG      GG                                    ");
            textcolour(ConsoleColor.White);
            write("Goplex Studios GoOS.");
            log(ConsoleColor.Green, "");
            textcolour(ConsoleColor.Red);
            write("  GGGGG      G            GGGGGGGGGGGGGGGGGGGG     ");
            textcolour(ConsoleColor.White);
            write("Copyright 2022 (c) Owen2k6.");
            log(ConsoleColor.Green, "");
            textcolour(ConsoleColor.DarkRed);
            write("  GGGGG      GG           GGGGGGGGGGGGGGGGGGG      ");
            textcolour(ConsoleColor.White);
            write("Version 1.4");
            log(ConsoleColor.Green, "");
            textcolour(ConsoleColor.Magenta);
            write("  GGGGG      GG           GGGGGGGGGGGGGGGGGGG      ");
            textcolour(ConsoleColor.White);
            write("Development Channel");
            log(ConsoleColor.Green, "");
            //Ok now continue
            log(ConsoleColor.DarkMagenta, "  GGGGGG     GGGG         GGGGGGGGGGGGGGGGGG       ");
            log(ConsoleColor.Red, "  GGGGGGG    GGGGGG              GGGGGGGGGG        ");
            log(ConsoleColor.DarkRed, "  GGGGGGGGG  GGGGGGGGGGGGGGGGGGGGGGGGGGGG          ");
            log(ConsoleColor.Magenta, "  GGGGGGGGGGG                                      ");
            log(ConsoleColor.DarkMagenta, "  GGGGGGGGGGGGGGG                  GGGG            ");
            log(ConsoleColor.Red, "  GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG            ");
            log(ConsoleColor.DarkRed, "  GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG            ");
            try
            {
                FS = new Sys.FileSystem.CosmosVFS(); Sys.FileSystem.VFS.VFSManager.RegisterVFS(FS); FS.Initialize(true);
                var total_space = FS.GetTotalSize(@"0:\");
                adminconsoledisk = true;
            }
            catch (Exception e)
            {
                log(ConsoleColor.Red, "GoOS Admin could not detect a disk. system will not support any apps that require a HDD to write/read from.");
                log(ConsoleColor.Red, "GoOS Needs a HDD installed to use some of the cool features");
                log(ConsoleColor.Red, "The GitHub releases page usually includes a disk built for GoOS");
                log(ConsoleColor.Red, "Disks aren't required but they're highly reccomended.");
                adminconsoledisk = false;
            }
            textcolour(ConsoleColor.Green);
            if (loginsystemenabled)
            {
                textcolour(ConsoleColor.Magenta);
                //Login System 0.1 Primitive edition
                log(ConsoleColor.Magenta, "Hello, " + username + "!");
                log(ConsoleColor.Magenta, "In order to proceed into GoOS, you must login with your password.");
                textcolour(ConsoleColor.Yellow);
                String input = Console.ReadLine();
                if (input == password)
                {
                    textcolour(ConsoleColor.Cyan);
                    Console.Clear();
                    log(ConsoleColor.Cyan, "Welcome back to GoOS.");
                }
                else
                {
                    log(ConsoleColor.Red, "Incorrect password.");
                    log(ConsoleColor.Red, "Press any key to retry");
                    Console.ReadKey();
                    Cosmos.System.Power.Reboot();
                }
            }
        }

        protected override void Run()
        {
            textcolour(ConsoleColor.Green);
            write("0:\\");
            textcolour(ConsoleColor.Gray);
            String input = Console.ReadLine().ToLower();
            //And so it begins...
            //Commands Section
            if (input == "cinfo")
            {
                log(ConsoleColor.Magenta, "Goplex Operating System");
                log(ConsoleColor.Blue, "GoOS is owned by Goplex Studios.");
                log(ConsoleColor.Red, "SYSTEM INFOMATION:");
                log(ConsoleColor.Red, "GoOS Version 1.4");
                log(ConsoleColor.Red, "Owen2k6 Api version: 0.15");
                log(ConsoleColor.Red, "Branch: Development");
                log(ConsoleColor.White, "Copyright 2022 (c) Owen2k6");
            }
            else if (input == "help")
            {
                log(ConsoleColor.Magenta, "Goplex Operating System");
                log(ConsoleColor.Blue, "HELP - Shows system commands");
                log(ConsoleColor.Blue, "CINFO - Shows system infomation");
                log(ConsoleColor.Blue, "SUPPORT - Shows how to get support");
                log(ConsoleColor.Blue, "GAMES - Shows the list of GoOS Games");
                log(ConsoleColor.Blue, "CORE - Displays GoOS Core infomation");
                log(ConsoleColor.Blue, "CALC - Shows a list of possible calculation commands");
                log(ConsoleColor.Blue, "CREDITS - Shows the GoOS Developers");
                log(ConsoleColor.Blue, "DISKCHECK - Check Disk Information");
                log(ConsoleColor.Blue, "LS - List all files on the disk");
                log(ConsoleColor.Blue, "NOTEPAD - MIV Notepad (Looks like VIM)");
                log(ConsoleColor.Blue, "DEL - Delete a file");
                log(ConsoleColor.Blue, "LD - ReLabel a disk (Rename disk)");
                log(ConsoleColor.Blue, "FTP - Will not work - File Transfer Protocol");
                log(ConsoleColor.Blue, "IPCONF - List all networking information");
                log(ConsoleColor.Blue, "GUI - See a cool lil test!");
            }
            else if (input == "credits")
            {
                log(ConsoleColor.Cyan, "Goplex Studios - GoOS");
                log(ConsoleColor.Cyan, "Discord Link: https://discord.owen2k6.com/");
                log(ConsoleColor.Red, "Contributors:");
                log(ConsoleColor.Red, "Owen2k6 - Main Developer and creator");
                log(ConsoleColor.Red, "Zulo - Helped create the command system");
                log(ConsoleColor.Red, "moderator_man - Helped with my .gitignore issue and knows code fr");
                log(ConsoleColor.Red, "");
            }
            else if (input == "support")
            {
                log(ConsoleColor.Cyan, "Goplex Studios Support");
                log(ConsoleColor.Red, "== For OS Support");
                log(ConsoleColor.Red, "To get support, you must be in the Goplex Studios Discord Server.");
                log(ConsoleColor.Red, "Discord Link: https://discord.owen2k6.com/");
                log(ConsoleColor.Red, "Open support tickets in #get-staff-help");
                log(ConsoleColor.Red, "== To report a bug");
                log(ConsoleColor.Red, "Go to the issues tab on the Owen2k6/GoOS Github page");
                log(ConsoleColor.Red, "and submit an issue with the bug tag.");
            }
            else if (input == "core")
            {
                log(ConsoleColor.Magenta, "GoOS Core Ver 0.3");
                log(ConsoleColor.Magenta, "The Main backend to GoOS.");
                log(ConsoleColor.Magenta, "==========================");
                log(ConsoleColor.Magenta, "==Developed using Cosmos==");
                log(ConsoleColor.Magenta, "==========================");
                log(ConsoleColor.Red, "GoOS Core Is still in early development.");
                log(ConsoleColor.Red, "there are a lot of issues known and we are working on it! ");
            }

            //Games Section

            //Calculator Area

            else if (input == "calc")
            {
                log(ConsoleColor.Magenta, "GoCalc Commands");
                log(ConsoleColor.Blue, "ADD - Add 2 numbers");
                log(ConsoleColor.Blue, "SUBTRACT - Subtract 2 numbers");
                log(ConsoleColor.Blue, "DIVIDE - Divide 2 numbers");
                log(ConsoleColor.Blue, "MULTIPLY - Multiply 2 numbers");
                log(ConsoleColor.Blue, "SQUARE - Square a number");
                log(ConsoleColor.Blue, "CUBE - Cube a number");
                log(ConsoleColor.Blue, "POWER10 - Make a number to the power of 10");
            }
            else if (input == "add")
            {
                log(ConsoleColor.Green, "GoCalc - Addition");
                log(ConsoleColor.Green, "Whole numbers only !!");
                log(ConsoleColor.Green, "Enter the first number: ");
                int no1 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Enter the second number: ");
                int no2 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Adding up to");
                int ans = no1 + no2;
            }
            else if (input == "subtract")
            {
                log(ConsoleColor.Green, "GoCalc - Subtraction");
                log(ConsoleColor.Green, "Whole numbers only !!");
                log(ConsoleColor.Green, "Enter the first number: ");
                int no1 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Enter the second number: ");
                int no2 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Adding up to");
                int ans = no1 - no2;
            }
            else if (input == "divide")
            {
                log(ConsoleColor.Green, "GoCalc - Division");
                log(ConsoleColor.Green, "Whole numbers only !!");
                log(ConsoleColor.Green, "Enter the first number: ");
                int no1 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Enter the second number: ");
                int no2 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Adding up to");
                int ans = no1 / no2;
            }
            else if (input == "multiply")
            {
                log(ConsoleColor.Green, "GoCalc - Multiplication");
                log(ConsoleColor.Green, "Whole numbers only !!");
                log(ConsoleColor.Green, "Enter the first number: ");
                int no1 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Enter the second number: ");
                int no2 = Convert.ToInt32(Console.ReadLine());
                log(ConsoleColor.Green, "Adding up to");
                int ans = no1 * no2;
            }
            else if (input == "square")
            {
                log(ConsoleColor.Green, "GoCalc - Squaring");
                log(ConsoleColor.Green, "Whole numbers only !!");
                log(ConsoleColor.Green, "Enter number to square: ");
                int no1 = Convert.ToInt32(Console.ReadLine());
                int ans = no1 * no1;
            }
            else if (input == "cube")
            {
                log(ConsoleColor.Green, "GoCalc - Cubing");
                log(ConsoleColor.Green, "Whole numbers only !!");
                log(ConsoleColor.Green, "Enter number to cube: ");
                int no1 = Convert.ToInt32(Console.ReadLine());
                int ans = no1 * no1 * no1;
            }
            else if (input == "power10")
            {
                log(ConsoleColor.Green, "GoCalc - To the power of 10");
                log(ConsoleColor.Green, "Whole numbers only !!");
                log(ConsoleColor.Green, "Enter number to p10: ");
                int no1 = Convert.ToInt32(Console.ReadLine());
                int ans = no1 * no1 * no1 * no1 * no1 * no1 * no1 * no1 * no1 * no1;
            }

            // GoOS Admin

            //Disk Only stuff
            else if (input == "diskcheck")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    try
                    {
                        log(ConsoleColor.Red, "GoOS Admin: Showing Disk Information for 0:\\");
                        var available_space = FS.GetAvailableFreeSpace(@"0:\");
                        var total_space = FS.GetTotalSize(@"0:\");
                        var label = FS.GetFileSystemLabel(@"0:\");
                        var fs_type = FS.GetFileSystemType(@"0:\");
                        log(ConsoleColor.Red, "Available Free Space: " + available_space + "(" + (available_space / 1e+9) + "GiB)");
                        log(ConsoleColor.Red, "Total Space on disk: " + total_space + "(" + (total_space / 1e+9) + "GiB)");
                        log(ConsoleColor.Red, "Disk Label: " + label);
                        log(ConsoleColor.Red, "File System Type: " + fs_type);
                    }
                    catch (Exception e)
                    {
                        log(ConsoleColor.Red, "GoOS Admin: Error Loading disk! You might have disconnected the drive!");
                        log(ConsoleColor.Red, "GoOS Admin: For system security, we have disabled all Drive functions.");
                        adminconsoledisk = false;
                    }
                }
            }
            else if (input == "ls")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    try
                    {
                        var directory_list = Directory.GetFiles(@"0:\");
                        foreach (var file in directory_list)
                        {
                            log(ConsoleColor.Red, file);
                        }
                    }
                    catch (Exception e)
                    {
                        log(ConsoleColor.Red, "GoOS Admin: Error Loading disk! You might have disconnected the drive!");
                        log(ConsoleColor.Red, "GoOS Admin: For system security, we have disabled all Drive functions.");
                        adminconsoledisk = false;
                    }
                }
            }
            else if (input == "notepad")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    textcolour(ConsoleColor.White);
                    MIV.StartMIV();
                }
            }
            else if (input == "del")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: Enter file name");
                    textcolour(ConsoleColor.Yellow);
                    write("FilePath: 0:\\");
                    String inputaman = Console.ReadLine();
                    try
                    {
                        File.Delete(@"0:\" + inputaman);
                        log(ConsoleColor.Blue, "GoOS Admin: File Deleted!");
                    }
                    catch (Exception e)
                    {
                        log(ConsoleColor.Red, "Please send the following to GoOS Developers");
                        log(ConsoleColor.Red, e.ToString());
                    }
                }
            }
            else if (input == "run")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: Enter file name");
                    textcolour(ConsoleColor.Yellow);
                    write("FilePath: 0:\\");
                    String inputaman = Console.ReadLine();
                    try
                    {
                        log(ConsoleColor.Blue, "GoOS Admin: Attempting to run " + inputaman);
                        if (!inputaman.EndsWith(".goexe"))
                        {
                            log(ConsoleColor.Red, "GoOS Admin: Incompatible format.");
                            log(ConsoleColor.Red, "GoOS Admin: File must be .goexe");
                        }
                        if (inputaman.EndsWith(".goexe"))
                        {
                            var content = File.ReadAllLines(@"0:\" + inputaman);
                            string theysaid = null;
                            int count = 1;
                            String a;
                            String b;
                            String c;
                            String d;
                            String e;
                            String f;
                            String g;
                            String h;
                            String i;
                            String j;
                            foreach (string line in content)
                            {
                                count = count + 1;
                                //log(ConsoleColor.Magenta, "LINE FOUND: CONTENT: " + line);
                                if (line.StartsWith("#"))
                                {
                                    
                                }
                                if (line.StartsWith(""))
                                {
                                    
                                }
                                if (line.StartsWith("print="))
                                {
                                    string thingtosay = line.Replace("print=", "");
                                    thingtosay = thingtosay.Replace("{getInput}", theysaid);
                                    log(ConsoleColor.Magenta, thingtosay);
                                }
                                if (line.StartsWith("sleep="))
                                {
                                    String howlong = line.Split("=")[1];
                                    int potato = Convert.ToInt32(howlong);
                                    sleep(potato);
                                }
                                if (line.StartsWith("input="))
                                {
                                    if (line == "input=")
                                    {
                                        textcolour(ConsoleColor.Blue);
                                        theysaid = Console.ReadLine();
                                    }
                                    else
                                    {
                                        String addon = line.Replace("input=", "");
                                        write(addon);
                                        textcolour(ConsoleColor.Blue);
                                        theysaid = Console.ReadLine();
                                    }

                                }
                                if (line.StartsWith("variable="))
                                {
                                    if (line.Split("=")[1] == "1")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            a = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "2")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            b = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "3")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            c = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "4")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            d = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "5")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            e = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "6")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            f = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "7")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            g = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "8")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            h = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "9")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            i = gethandled;
                                        }
                                    }
                                    if (line.Split("=")[1] == "10")
                                    {
                                        if(line.Split("=")[3] == null || line.Split("=")[3] == "")
                                        {
                                            log(ConsoleColor.Red, "ERROR ON LINE " + count);
                                            log(ConsoleColor.Red, "Variable creation must have a value and can not be blank.");
                                            break;
                                        }
                                        else
                                        {
                                            String gethandled = line.Split("=")[3].Replace("{getInput}", theysaid);
                                            j = gethandled;
                                        }
                                    }

                                }

                            }

                        }
                    }
                    catch (Exception e)
                    {
                        log(ConsoleColor.Red, "Please send the following to GoOS Developers");
                        log(ConsoleColor.Red, e.ToString());
                    }
                }
            }
            else if (input == "ld")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    var label = FS.GetFileSystemLabel(@"0:\");
                    log(ConsoleColor.Red, "GoOS Admin: Relabel disk");
                    log(ConsoleColor.Red, "GoOS Admin: Press ENTER to leave the label as \"" + label + "\"");
                    textcolour(ConsoleColor.Yellow);
                    write("New Label for 0:\\: ");
                    String inputamana = Console.ReadLine();
                    if (inputamana == string.Empty)
                    {
                        inputamana = label;
                    }
                    try
                    {
                        FS.SetFileSystemLabel(@"0:\", inputamana);
                        log(ConsoleColor.Blue, "GoOS Admin: Drive Label modified from " + label + " to " + inputamana);
                    }
                    catch (Exception e)
                    {
                        log(ConsoleColor.Red, "Please send the following to GoOS Developers");
                        log(ConsoleColor.Red, e.ToString());
                    }
                }
            }
            else if (input == "ftp")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    using (var xServer = new FtpServer(FS, "0:\\"))
                    {
                        /** Listen for new FTP client connections **/
                        log(ConsoleColor.Blue, "GoOS Admin: Listening on " + NetworkConfiguration.CurrentAddress.ToString() + ":21");
                        log(ConsoleColor.Blue, "Use PLAIN configurations with no login information.");
                        log(ConsoleColor.Blue, "FTP MODE ENABLED. REBOOT TO DISABLE");
                        xServer.Listen();
                    }
                }
            }
            else if (input == "ipconf")
            {
                log(ConsoleColor.Red, "GoOS Admin: Showing Internet Information");
                log(ConsoleColor.Red, NetworkConfiguration.CurrentAddress.ToString());
            }

            //smth cool bro

            else if (input == "gui")
            {
                if (!adminconsoledisk)
                {
                    log(ConsoleColor.Red, "GoOS Admin: There is currently no disk loaded to the system.");
                }
                if (adminconsoledisk)
                {
                    // backup canvases
                    //
                    // canvas = new VBECanvas(new Mode(1024, 768, ColorDepth.ColorDepth32));
                    // canvas = new SVGAIICanvas(new Mode(1024, 768, ColorDepth.ColorDepth32));
                    canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1024, 768, ColorDepth.ColorDepth32));
                    Sys.MouseManager.ScreenWidth = 1012;
                    Sys.MouseManager.ScreenHeight = 768;
                    while (true)
                    {
                        Heap.Collect();
                        canvas.Clear(Color.Green);
                        //guicanvas.DrawImage(wallpaper, 0, 0);
                        TextBox.textbox(canvas, 4, 4, 0, 0, "GoOS GUI");
                        TextBox.textbox(canvas, 12, 12, 0, 0, "24092022");
                        TextBox.textbox(canvas, 40, 40, 60, 24, "24092022");
                        Cursor.DrawCursor(canvas, Sys.MouseManager.X, Sys.MouseManager.Y);

                        canvas.Display();
                    }
                }


            }



            else
            {
                textcolour(ConsoleColor.Red);
                write("sorry, but ");
                textcolour(ConsoleColor.Yellow);
                write("`" + input + "` ");
                textcolour(ConsoleColor.Red);
                write("is not a command");
                textcolour(ConsoleColor.Magenta);
                log(ConsoleColor.Green, "");
                log(ConsoleColor.Magenta, "Type HELP for a list of commands");
            }
            textcolour(ConsoleColor.Green);
        }
    }


}


