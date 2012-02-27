﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections;

namespace Client
{
    public class Client
    {
        
        public static string IP;
        private static ASCIIEncoding asc = new ASCIIEncoding();
        private static TcpClient tcpclnt;
        private static MinesweeperGUI gui;

        public static void Main()
        {
            //Application.Run(new MinesweeperGUI());
            try
            {
                while (true)
                {

                    try
                    {
                        /*Application.Run(new InputIp());
                        tcpclnt = new TcpClient();
                        if (IP == null)
                            return;
                        Console.WriteLine(IP);
                        tcpclnt.Connect(IP, 8001);*/
                        tcpclnt = new TcpClient();
                        tcpclnt.Connect("localhost", 8001);
                        Console.WriteLine("Connection established");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error : " + e);
                        continue;
                    }
                    break;
                }
                gui = new MinesweeperGUI(tcpclnt);
                Application.Run(gui);

            }

            catch (Exception e)
            {
                MessageBox.Show("Error : " + e);
            }
        }

        public static string sendMessage(string msg)
        {
            string response = String.Empty;
            Stream stream = tcpclnt.GetStream();
            byte[] msgBytes = asc.GetBytes(msg);
            stream.Write(msgBytes, 0, msgBytes.Length);
            byte[] buffer = new byte[gui.width * gui.height * 9];
            int responseLength = stream.Read(buffer, 0, buffer.Length);
            for (int i = 0; i < responseLength; i++)
            {
                response += (char)buffer[i];
            }
            /*Console.WriteLine("Response from server:");
            Console.WriteLine(response);*/
            parseMessage(response);
            return response;


        }

        public static void parseMessage(string msg)
        {
            string[] tokens = msg.Split(' ');
            /*foreach (string s in tokens)
                s.Trim();*/
            //Console.WriteLine("eik tu nx " + tokens[0]);
            switch (tokens[0])
            {
                case "ok":
                    return;
                case "explode":
                    gui.explode(tokens);
                    break;
                case "reveal":
                    gui.revealTiles(tokens);
                    break;
                case "dismantle":
                case "flag":
                case "none":
                    {
                        gui.modifyAddon(msg);
                        break;
                    }

            }
            return;
        }

        
    }
}