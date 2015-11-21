using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Transmitter;
using System.Threading;
using System.Diagnostics;

class SocketClient
{
    static TcpClient tcpClient;
    static NetworkStream networkStream;
    static StreamReader Reader;
    static StreamWriter Writer;
    static int port = 80; 
    static void Main(string[] args)
    {
        try
        {
            Console.Title = "Client";
            Console.WriteLine("Enter IP of server: ");
            // tcpClient = new TcpClient("localhost", port);
            tcpClient = new TcpClient(Console.ReadLine(), port);
            networkStream = tcpClient.GetStream();

            Reader = new StreamReader(networkStream);
            Writer = new StreamWriter(networkStream);


            byte[] ReadBuffer = new byte[1024];

             Console.WriteLine("Server connected\n");

            int numberOfBytesRead = 0;
            StringBuilder CompleteMessage = new StringBuilder();

            Console.Write("Please enter your name: ");
            string Name = Console.ReadLine();
        
                while(true)
                {
                    Console.Write("Please enter password to connect with server: ");

                    if (networkStream.CanWrite)
                    {

                        byte[] message = Encoding.Unicode.GetBytes(Console.ReadLine());
                        networkStream.Write(message, 0, message.Length);
                        Console.WriteLine();
                    }
                    if (networkStream.CanRead)
                    {
                        do
                        {
                            numberOfBytesRead = networkStream.Read(ReadBuffer, 0, ReadBuffer.Length);
                            CompleteMessage.AppendFormat("{0}", Encoding.Unicode.GetString(ReadBuffer, 0, numberOfBytesRead));
                        }
                        while (networkStream.DataAvailable);
                        if (CompleteMessage.ToString() =="true")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Beep();
                            Console.WriteLine("\nAccess opened ");
                            Console.ResetColor();
                            CompleteMessage.Clear();

                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Beep(100, 500);
                            Console.WriteLine("\nWrong password. Try again...");
                            Console.ResetColor();
                            CompleteMessage.Clear();
                        }
                    }
            }

            string command;
                while (true)
                {
                    Console.Write("Enter your message: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                command = Console.ReadLine();
                switch(command)
                {
                    case "END":
                        tcpClient.Close();
                        Process.GetCurrentProcess().Kill();
                        break;
                    default:
                        if (networkStream.CanWrite)
                        {

                            byte[] message = Encoding.Unicode.GetBytes(Name + ": " + command);
                            networkStream.Write(message, 0, message.Length);
                            Console.ResetColor();
                        }
                        if (networkStream.CanRead)
                        {
                            Console.WriteLine("\nWaiting answer from the server...\n");
                            do
                            {
                                numberOfBytesRead = networkStream.Read(ReadBuffer, 0, ReadBuffer.Length);
                                CompleteMessage.AppendFormat("{0}", Encoding.Unicode.GetString(ReadBuffer, 0, numberOfBytesRead));
                            }
                            while (networkStream.DataAvailable);

                            networkStream.Flush();
                            Console.Write("Message from the Server: ");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(CompleteMessage + "\n");
                            CompleteMessage.Clear();
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("Error with connecting");
                        }
                        break;
                }

            }

        }
                    

        catch (SocketException exception)

        {
            Console.WriteLine("Error: "+exception.Message);
        }
        Console.ReadLine();

    }
}