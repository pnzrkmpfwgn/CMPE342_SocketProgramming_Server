using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp4
{
    public class SynchronousSocketListener
    {
        static void Main(string[] args)
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAdress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAdress, 11000);
           
            Socket Listener  = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Listener.Bind(localEndPoint);
            Listener.Listen(10);

            String data;

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                Socket handler = Listener.Accept();

                data = null;
                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if(data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                Console.WriteLine("Text Recieved : {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes("Your mesasge is received.");

                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}
