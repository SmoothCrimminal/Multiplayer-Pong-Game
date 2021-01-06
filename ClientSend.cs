using UnityEngine;
using Newtonsoft.Json; 

public class ClientSend : MonoBehaviour{

    private static void SendTCPData(string json){

        Client.instance.tcp.SendData(json)
        // wysyłanie
    }

    public static void PaddleMovement(bool[] _inputs){

        foreach (bool _input in _inputs)
        {
            // tu będziemy wyłapywać ruchy gracza, konwertować je na jsona i wysyłać
            // do wszystkich klientów, ale jeszcze nie wiem jak to napisać xD
        }
    }
}