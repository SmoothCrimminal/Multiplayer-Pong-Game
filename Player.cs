using System;
using System.Numerics;
using Newtonsoft.Json; 

class Player{

    public int id;
    public Vector2 playerPosition;
    public Vector2 ballPosition;

    private bool[] inputs;

    public Player(int _id, int _position){

        id = _id;
        position = _position;
    }

    public void SetInput(bool[] _inputs){

        inputs = _inputs
    }
        // Zmiana kierunku ruchu

    public void Update(){

        Vector2 _inputDirection = Vector2.Zero;

        if (inputs[0]){

            _inputDirection.Y += 7; 
        }

        if (inputs[1]){
            
            _inputDirection.Y -= 7;
        }

        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection){

        Vector2 _move = Vector2.Transform(new Vector2(0, 1));

        move = _move * _inputDirection.Y;

        playerPosition += move;

        Info jsonInfo = new Info();

        jsonInfo.paddlePosition = Convert.ToString(playerPosition); // to jeszcze pomyślimy
        string json = JsonConvert.SerializeObject(jsonInfo); // jak by mogło działać


        ServerSend.PlayerPosition(json);
    }

}