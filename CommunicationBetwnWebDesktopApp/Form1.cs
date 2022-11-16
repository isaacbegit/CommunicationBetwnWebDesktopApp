using Fleck;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommunicationBetwnWebDesktopApp
{
    public partial class Form1 : Form
    {
        List<IWebSocketConnection> allSockets;
        public Form1()
        {
            InitializeComponent();
        }

        private void logData(string str)
        {

            Invoke(new Action(() =>
            {
                textBox1.Text += str + Environment.NewLine;
            }));
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                FleckLog.Level = LogLevel.Debug;
                allSockets = new List<IWebSocketConnection>();
                var server = new WebSocketServer("ws://0.0.0.0:8181");
                server.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        logData("Open!");
                        allSockets.Add(socket);
                    };
                    socket.OnClose = () =>
                    {
                        logData("Close!");
                        allSockets.Remove(socket);
                    };
                    socket.OnMessage = message =>
                    {
                        logData(message);
                        allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                    };
                });

            } catch (Exception ex)
            {
                logData(ex.Message);
            }
            //var input = Console.ReadLine();
            //while (input != "exit")
            //{
            //    foreach (var socket in allSockets.ToList())
            //    {
            //        socket.Send(input);
            //    }
            //    input = Console.ReadLine();
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var input = textBox2.Text;

            foreach (var socket in allSockets.ToList())
            {
                socket.Send(input);
            }
           
        }
    }
    }
    

