using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var ipAddress = IPAddress.Parse("127.0.0.1");

            var socket = new Socket(ipAddress.AddressFamily, 
                SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(ipAddress, 1);


            socket.Bind(endPoint);
            socket.Listen(1);

            while(true)
            {   
                Console.WriteLine("Socket started. Waiting connections...");
                var clientSocket = socket.Accept();
                Console.WriteLine("Client is connected!");

                byte[] buffer = new byte[1024];

                string data = null;
                while(true)
                {
                      var numberOfBytesReceived = clientSocket.Receive(buffer);
                      Console.WriteLine("{0} bytes received", numberOfBytesReceived);

                      data += Encoding.ASCII.GetString(buffer, 0, numberOfBytesReceived);

                      Console.WriteLine(data);
                      if(data.Contains("<EOF>"))
                      {
                          break;
                      }                  
                }

                Console.WriteLine("Message received --> {0}", data);
                
                var response = Encoding.ASCII.GetBytes("I got your message, thank you client!");
                
                clientSocket.Send(response);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }


        }
    }
}
