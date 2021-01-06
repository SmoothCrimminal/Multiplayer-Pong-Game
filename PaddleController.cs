using UnityEngine;

public class PaddleController : MonoBehaviour{
    private void FixedUpdate(){

        SendInputToServer();
    }

    private void SendInputToServer(){

        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.UpArrow);
            Input.GetKey(KeyCode.DownArrow);
        };

        ClientSend.PlayerMovement(_inputs);
    }
}