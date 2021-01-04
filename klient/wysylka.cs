using System;
namespace Application
{
    public class wysylka
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("10.45.13.220", 1234);

            Stream stream = client.GetStream();

            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("Testing...");

            client.Close();
        }
    }
}
