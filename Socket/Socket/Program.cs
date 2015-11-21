using System;
using System.Text;
using System.Net.Sockets;

class SocketServer

{
    static int port;

    public static void Main()

    {
        Console.Title = "Server";
        NetworkStream networkStream;
            port = 80;
        TcpListener tcpListener = TcpListener.Create(port);

        tcpListener.Start();

       // tcpListener.Start();

        Console.WriteLine("The Server has started on port {0}\n",port);
        Socket serverSocket = tcpListener.AcceptSocket();

        try
        {
            byte[] ReadBuffer = new byte[1024];
            if (serverSocket.Connected)
            {
                //Console.WriteLine("Client connected\nWaiting message from the client...\n");

                networkStream = new NetworkStream(serverSocket);
                int numberOfBytesRead = 0;
                StringBuilder CompleteMessage = new StringBuilder();
                string password = "1234";

                Console.WriteLine("\nListening...");
                while (true)
                {
                    
                    if (networkStream.CanRead)
                    {
                        try
                        {
                            do
                            {
                                numberOfBytesRead = networkStream.Read(ReadBuffer, 0, ReadBuffer.Length);
                                CompleteMessage.AppendFormat("{0}", Encoding.Unicode.GetString(ReadBuffer, 0, numberOfBytesRead));
                            }
                            while (networkStream.DataAvailable);
                        }
                        catch(Exception exception)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Beep(100,500);
                            Console.WriteLine(exception.Message);
                            Console.ResetColor();

                        }
                        if (CompleteMessage.ToString()==password)
                        {
                            //if (networkStream.CanWrite)
                            {
                                byte[] message = Encoding.Unicode.GetBytes("true");
                                networkStream.Write(message, 0, message.Length);

                            }
                            Console.WriteLine("\nClient connected ");
                            CompleteMessage.Clear();
                            break;
                        }
                        else
                        {
                            CompleteMessage.Clear();
                            byte[] message = Encoding.Unicode.GetBytes("0");
                            networkStream.Write(message, 0, message.Length);
                        }
                    }
                    
                }
                while (true)

                {
                    if (networkStream.CanRead)
                    {
                        do
                        {
                            numberOfBytesRead = networkStream.Read(ReadBuffer, 0, ReadBuffer.Length);
                            CompleteMessage.AppendFormat("{0}", Encoding.Unicode.GetString(ReadBuffer, 0, numberOfBytesRead));
                        }
                        while (networkStream.DataAvailable);


                       // Console.Write("\nMessage from the Client: ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(CompleteMessage + "\n");

                        CompleteMessage.Clear();
                        Console.ResetColor();
                    }
                    else
                    {

                    }
                    Console.Write("Enter your message: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (networkStream.CanWrite)
                    {
                        byte[] message = Encoding.Unicode.GetBytes(Console.ReadLine());
                        networkStream.Write(message, 0, message.Length);
                    }
                    else
                    {
                        Console.WriteLine("You cannot write");
                    }
                    Console.ResetColor();
                }

            }

            else if (!serverSocket.Connected)
            {
                serverSocket.Close();
            }
            Console.Read();

        }

        catch (SocketException exception)
        {

            Console.WriteLine(exception);
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
   
}