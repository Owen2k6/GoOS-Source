﻿using System;
using System.IO;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;
using TcpClient = Cosmos.System.Network.IPv4.TCP.TcpClient;
using System.Text;
using System.Collections.Generic;
using PrismAPI.Graphics;
using static GoOS.Resources;

namespace GoOS.GUI.Apps.GoStore
{
    public class MainFrame : Window
    {
        Button[] RepoFilesButtons;

        List<(string, string, string, string, string, string)> repoFiles =
            new List<(string, string, string, string, string, string)>();

        readonly string[] repos =
        {
            "apps.goos.owen2k6.com",
            "repo.mobren.net"
        };

        public MainFrame()
        {
            try
            {
                // Get the file list from every repo
                string[][] infoFiles =
                {
                    GetInfoFile(repos[0]).Split('\n'),
                    GetInfoFile(repos[1]).Split('\n'),
                };

                int o = 0;
                foreach (string[] file in infoFiles)
                {
                    foreach (string program in file)
                    {
                        string[] metadata = program.Split('|');
                        repoFiles.Add((metadata[0], metadata[1], metadata[2], metadata[3], metadata[4], repos[o]));
                    }

                    o++;
                }

                // Generate the fonts.
                Generate(ResourceType.Fonts);

                // Create the window.
                Contents = new Canvas(400, 300);
                Title = "GoStore";
                Visible = true;
                Closable = true;
                SetDock(WindowDock.Center);

                // Initialize the controls.
                RepoFilesButtons = new Button[repoFiles.Count];
                bool GS = HasLaunched();
                if (GS || Kernel.BuildType == "NIFPR")
                {
                    for (int i = 0; i < repoFiles.Count; i++)
                    {
                        RepoFilesButtons[i] = new Button(this, Convert.ToUInt16(10 + i / 10 * 185),
                            Convert.ToUInt16(52 + (i * 20 - i / 10 * 200)),
                            Convert.ToUInt16(repoFiles[i].Item1.Length * 8), 20, repoFiles[i].Item1)
                        {
                            Name = repoFiles[i].Item1,
                            UseSystemStyle = false,
                            BackgroundColour = new Color(0, 0, 0, 0),
                            SelectionColour = Color.DeepGray,
                            HasSelectionColour = true,
                            ClickedAlt = repoFiles_Click,
                            RenderWithAlpha = true
                        };
                    }

                    // Paint the window.
                    Contents.DrawImage(0, 0, Resources.GoStore, false);
                    RenderSystemStyleBorder();
                    foreach (Button i in RepoFilesButtons) i.Render();
                }
                else
                {
                    Contents.DrawImage(0, 0, Resources.GoStore, false);
                    RenderSystemStyleBorder();
                    Contents.DrawString(0, 0, "The GoStore is coming soon...\nThis will allow users to install applications directly to their device\nWe are working on preparing the GoStore and getting apps made for\nyou to enjoy! See you on launch day!",Font_1x, Color.White, true);
                }
            }
            catch (Exception ex)
            {
                Dialogue.Show("Error", ex.ToString());
            }
        }

        private string GetInfoFile(string repo)
        {
            try
            {
                var dnsClient = new DnsClient();
                var tcpClient = new TcpClient();

                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk(repo);
                Address address = dnsClient.Receive();
                dnsClient.Close();

                tcpClient.Connect(address, 80);

                string httpget = "GET /" + "info.glist" + " HTTP/1.1\r\n" +
                                 "User-Agent: GoOS\r\n" +
                                 "Accept: */*\r\n" +
                                 "Accept-Encoding: identity\r\n" +
                                 "Host: " + repo + "\r\n" +
                                 "Connection: Keep-Alive\r\n\r\n";

                tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                var ep = new EndPoint(Address.Zero, 0);
                var data = tcpClient.Receive(ref ep);
                tcpClient.Close();

                string httpresponse = Encoding.ASCII.GetString(data);

                string[] responseParts =
                    httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

                if (responseParts.Length == 2)
                {
                    return responseParts[1];
                }
            }
            catch
            {
            }

            return string.Empty;
        }

        private Boolean HasLaunched()
        {
            try
            {
                var dnsClient = new DnsClient();
                var tcpClient = new TcpClient();

                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk("apps.goos.owen2k6.com");
                Address address = dnsClient.Receive();
                dnsClient.Close();

                tcpClient.Connect(address, 80);

                string httpget = "GET /GoOS/gslaunch.goos HTTP/1.1\r\n" +
                                 "User-Agent: GoOS\r\n" +
                                 "Accept: */*\r\n" +
                                 "Accept-Encoding: identity\r\n" +
                                 "Host: api.owen2k6.com\r\n" +
                                 "Connection: Keep-Alive\r\n\r\n";

                tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                var ep = new EndPoint(Address.Zero, 0);
                var data = tcpClient.Receive(ref ep);
                tcpClient.Close();

                string httpresponse = Encoding.ASCII.GetString(data);

                string[] responseParts =
                    httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

                if (responseParts.Length == 2)
                {
                    if (responseParts[1] == "true")
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        private int GetIndexByTitle(string title)
        {
            for (int i = 0; i < repoFiles.Count; i++)
            {
                if (repoFiles[i].Item1 == title) return i;
            }

            return 0;
        }

        private void repoFiles_Click(string i)
        {
            WindowManager.AddWindow(new DescriptionFrame(
                repoFiles[GetIndexByTitle(i)].Item1, repoFiles[GetIndexByTitle(i)].Item4,
                repoFiles[GetIndexByTitle(i)].Item5, repoFiles[GetIndexByTitle(i)].Item3,
                repoFiles[GetIndexByTitle(i)].Item2, repoFiles[GetIndexByTitle(i)].Item6));
        }
    }
}