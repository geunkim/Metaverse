using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class PythonServer : MonoBehaviour
{
    private const string SERVER_IP = "127.0.0.1";
    private const int PORT = 8888;

    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            _clientSocket.Connect(SERVER_IP, PORT);
            Debug.Log("Connected to server");
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void SendData(string data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);
        _clientSocket.Send(byteData);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "switch")
        {
            SendData("device1|on");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        SendData("device1|off");
    }

    void OnApplicationQuit()
    {
        _clientSocket.Close();
    }
}
