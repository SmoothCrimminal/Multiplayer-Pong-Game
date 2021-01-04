using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerTcp
{
    class MainClass
    {
        static void Main(string[] args)
        {
            //Utworzenie połączenia z gniazdem BSD z C
            IPAddress ip = IPAddress.Parse("192.168.1.31");
            int port = 1234;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("client connected!!");

            //Dostęp do przesyłania dancch
            NetworkStream ns = client.GetStream();

            //Utworzenie nowego wątku odpowiedzialnego za otrzymywanie wiadomości
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));
            thread.Start(client);

            //Pobieranie wiadomosci z konsoli
            string s;
            while (!string.IsNullOrEmpty((s = Console.ReadLine())))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(s);
                ns.Write(buffer, 0, buffer.Length);
            }

            //Zakmnięcie połączenia z gniadem BSD, jeżeli użytkownik nacisnie ENTER
            client.Client.Shutdown(SocketShutdown.Send);
            thread.Join();
            ns.Close();
            client.Close();
            Console.WriteLine("disconnect from server!!");
            Console.ReadKey();
        }

        //Funkcja odpowiadająca za odbiór wiadomości
        static void ReceiveData(TcpClient client)
        {
            //Dostęp do przechwytywania dancyh
            NetworkStream ns = client.GetStream();


            byte[] receivedBytes = new byte[1024000];
            int byte_count;
            string receivedString = String.Empty;
            string MessageToSend = String.Empty;

            //Przechwytywanie wiadomości
            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                MessageToSend = String.Empty;
                Console.Write("From clients:");
                receivedString = System.Text.Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                //Console.WriteLine(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));

                //Wyświetlanie otrzymanej wiadomości
                Console.WriteLine(receivedString);

                //Funckja zajmująca się obsługą dostarczonego Stringa
                ManageJson(receivedString, ns);

            }
            Console.WriteLine("Koniec Wiadomości");
        }

        static bool ManageJson(string ToJson, NetworkStream ns)
        {
            string JsonToSend = String.Empty;
            JObject stuff;
            try
            {
                stuff = JObject.Parse(ToJson);
            } catch (Exception)
            {
                stuff = JObject.Parse("{ 'Error': 'True'}");
                Console.WriteLine("Otrzymano plik nie będący poprawnym plikien Json");
                return false;
            }

            //Tutaj będziemy modyfikować tego Jsona, pobierać użytkowników, do których mamy wysłać itp.
            stuff["Status"] = "Waiting";
            JsonToSend = stuff.ToString(Formatting.None);

            //Wysyłanie do każdego klienta z danego pokoju
            byte[] buffer = Encoding.ASCII.GetBytes(stuff["Clients"]["1"] + JsonToSend);
            ns.Write(buffer, 0, buffer.Length);

            //Opóźniamy program, aby nie łączył wiadomości
            System.Threading.Thread.Sleep(10);
            byte[] buffer2 = Encoding.ASCII.GetBytes(stuff["Clients"]["2"] + JsonToSend);
            ns.Write(buffer2, 0, buffer2.Length);

            return true;

        }

        //static void ManageSend(NetworkStream ns, string message)
        //{
        //    byte[] buffer = Encoding.ASCII.GetBytes(message);
        //    ns.Write(buffer, 0, buffer.Length);
        //}
        }
    }
