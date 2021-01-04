using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace klient
{
    class MainClass2
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("192.168.1.31");
            int port = 1234;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("client connected!!");
            NetworkStream ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));

            thread.Start(client);

            string s;
            while (!string.IsNullOrEmpty((s = Console.ReadLine())))
            {
                s = "{'Room':'001','Ball':{ 'x':'20','y':'20'},'Pallets':{'1':'100','2':'120'},'Score':{'1':'0','2':'0'},'Status':'Empty', 'Clients':{'1':'1','2':'2'}}";
                byte[] buffer = Encoding.ASCII.GetBytes(s);
                ns.Write(buffer, 0, buffer.Length);
            }

            client.Client.Shutdown(SocketShutdown.Send);
            thread.Join();
            ns.Close();
            client.Close();
            Console.WriteLine("disconnect from server!!");
            Console.ReadKey();
        }

        static void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                Console.Write("From server:");
                string message = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                message = message.Remove(0, 1);
                Console.WriteLine(message);
            }
        }

        //static void Main2(string[] args)
        //{
        //    string message = "Siema";
        //    string messages = "Elo";
        //    TcpClient client = new TcpClient("192.168.1.31", 1234);
        //    while (true)
        //    {
        //        try
        //        {
        //            // Create a TcpClient.
        //            // Note, for this client to work you need to have a TcpServer
        //            // connected to the same address as specified by the server, port
        //            // combination.
        //            //Int32 port = 13000;

        //            Console.WriteLine("Podaj wiadomosc");
        //            messages = Console.ReadLine();
        //            message = "{'Room':'001','Ball':{ 'x':'20','y':'20'},'Pallets':{'1':'100','2':'120'},'Score':{'1':'0','2':'0'},'Status':'Empty', 'Client':'1'}";
        //            // Translate the passed message into ASCII and store it as a Byte array.
        //            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

        //            // Get a client stream for reading and writing.
        //            //  Stream stream = client.GetStream();

        //            NetworkStream stream = client.GetStream();

        //            // Send the message to the connected TcpServer.
        //            stream.Write(data, 0, data.Length);

        //            Console.WriteLine("Sent: {0}", message);

        //            // Receive the TcpServer.response.

        //            // Buffer to store the response bytes.
        //            data = new Byte[256];

        //            // String to store the response ASCII representation.
        //            String responseData = String.Empty;

        //            // Read the first batch of the TcpServer response bytes.
        //            Int32 bytes = stream.Read(data, 0, data.Length);
        //            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        //            Console.WriteLine("Received: {0}", responseData);

        //            // Close everything.
        //            //stream.Close();
        //            //client.Close();
        //        }
        //        catch (ArgumentNullException e)
        //        {
        //            Console.WriteLine("ArgumentNullException: {0}", e);
        //        }
        //        catch (SocketException e)
        //        {
        //            Console.WriteLine("SocketException: {0}", e);
        //        }

        //        Console.WriteLine("\n Press Enter to continue...");
        //        //Console.Read();
        //    }
        //}
    }
}