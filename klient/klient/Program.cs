using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace klient
{
    class MainClass
    {

        static void Main2(string[] args)
        {
            //Testowy plik, który będziemy przesyłać
            /*Generalnie proponuje takie pola

            Room - będzie informowało gdzie odbywa się gra

            Ball - pozycja piłki

            Pallets - pozycje paletek (potrzebna jest tylko jedna współrzędna)

            Score - wiadomo, żeby trzymać gdzieś wynik spotkania

            Status - tutaj proponuje trzymać status gry i przyjmowałby on następujące fazy:

                Empty - pokój całkowicie pusty i można dorzucić tam osobę rozpoczynającą grę

                Waiting - ktoś dołączył do pokoju i czeka na kolejną osobę
                (takie pokoje powinny być w pierwszej kolejności wykorzystane w trakcie szukana gry)

                Playing - pokój zajęty, nie można dołączyć, bo trwa gra

                Finnish - gra została zakończona, graczom wyświetla się wynik meczu i mogą opuścić pokój
                (Wtedy można tutaj dodać opcje zagraj ponownie i wyszukuje wolny pokój lub opuść pokój)

                Abort - rozważyłbym jeszcze możliwość, gdy ktoś opuści spotkanie,
                można wtedy dać chwilę czasu na ponowne dołączneie, a jeżeli gracz nie dołączy pnownie,
                to gra przejdzie w fazę Finnish

            Players (?) - nie wiem czy musimy to przechowywać w Jsonie,
            być może serwer będzie wiedział od razu kto gra na podstawie numeru pokoju,
            więc kwestia do przemyślenia
             
             */
            JObject stuff = JObject.Parse("{'Room':'001','Ball':{'x':'20','y':'20'},'Pallets':{'1':'100','2':'120'},'Score':{'1':'0','2':'0'},'Status':'Empty'}");
            string message = stuff.ToString(Formatting.None);

            //Utworzenia połączenia
            UdpClient udpClient = new UdpClient(1111);

            //Łączenie z serwerem
            udpClient.Connect("192.168.1.12", 1234);

            while (true) {

                //Na razie wysyla na serwer i z powrotem tutaj, więc możesz podać cokolwiek
                Console.WriteLine("Podaj adres IP, na który chcesz wysłać wiadomość:");

                //Bierzemy tylko adres z podanego wejscia
                string[] input = Console.ReadLine().Split(' ');
                string address = input[0];
                address = address.Trim();

                //Formatujemy JSON na stringa, żeby móc go wysłać dalej
                //Na razie stosuję stringa typu "[adres, na który wysyłamy] [tresc Jsona]"
                //Moim zdaniem tak najłatwiej ogarnąć dalsze przesyłanie w ukochanym C

                message = stuff.ToString(Formatting.None);
                message = address + " " + message;
                message = message.ToString();

                //Podajemy gdzie i co wysyłamy
                Console.WriteLine("Gdzie i co wysylamy: " + message);

                //Przerabiamy wiadomość na bajty i wysyłamy do hosta, z którym jesteśmy połączeni
                //W tym przypadku jest to aplikacja w C, gdzie gniazda BSD zajmują się komunikacją

                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                udpClient.Send(sendBytes, sendBytes.Length);

                //Ten obiekt umożliwia odczytywanie danych, z jakiegokolwiek hosta
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                //Czekamy, aż zostanie wysłana do nas wiadomość zwrotna
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                //Przerabiamy bajty na stringa
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine("Co dostajemy: " + returnData);

                //Przerabiamy stringa na obiekt Jsona
                //Stąd możemy elegancko odczytywać tak jak ze słownika Pythona
                JObject reurnString = JObject.Parse(returnData.ToString());

                //Tutaj testujemy, że wszytsko działa elegancko z Jsonem
                Console.WriteLine("Wypiszmy co dostaliśmy: ");
                Console.WriteLine(reurnString.ToString());
                Console.WriteLine("A teraz z odwolaniami do pól: ");

                Console.WriteLine("Room: " + reurnString["Room"]);

                Console.WriteLine("Ball: " + reurnString["Ball"]);

                Console.WriteLine("Pallets: " + reurnString["Pallets"]);

                Console.WriteLine("Score: " + reurnString["Score"]);

                //Tutaj możemy jeszcze podejrzeć za pomocą obiektu RemoteIpEndPoint,
                //kto wysłał daną wiadomość i z jakiego adresu oraz portu
                Console.WriteLine("This is the message you received: " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                        RemoteIpEndPoint.Address.ToString() +
                                        " on their port number " +
                                        RemoteIpEndPoint.Port.ToString());
            }

        }
    }
}
