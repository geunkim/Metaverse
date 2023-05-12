using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
//using UnityEngine.UIElements;

public class RasEvent : MonoBehaviour
{
    private const int PORT = 8555;
    private const string DEVICE_IP = "192.168.0.206"; // 라즈베리파이 기기의 IP 주소

    private Socket _clientSocket;
    private IPEndPoint _remoteEndPoint;
    private Move3D moveScript; // Move3D 스크립트

    public GameObject uiPanel; // UI 패널
    public Button lightButton; // 조명 버튼
    public Button temperatureButton; // 온도 버튼
    public Button ExitButton;        // 종료 버튼
    public bool isLightOn = false; // 조명이 켜져있는지 여부

    void Start()
    {
        moveScript = GetComponent<Move3D>(); // Move3D 스크립트를 가져옴

        uiPanel.SetActive(false); // UI 패널 비활성화

        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _remoteEndPoint = new IPEndPoint(IPAddress.Parse(DEVICE_IP), PORT);

        // 조명 버튼에 클릭 리스너 추가
        lightButton.onClick.AddListener(OnLightButtonClick);
        // 온도 버튼에 클릭 리스너 추가
        temperatureButton.onClick.AddListener(OnTemperatureButtonClick);
        // 종료 버튼에 클릭 리스너 추가
        ExitButton.onClick.AddListener(OnExitButtonClick);

        // ReceiveData 메서드를 쓰레드로 실행
        Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Switch"))
        {
            ConnectToServer();

            moveScript.enabled = false;
            uiPanel.SetActive(true);
            Cursor.visible = true; // 마우스 커서를 보이게 함
            Cursor.lockState = CursorLockMode.None; // 마우스 커서가 고정되지 않도록 함
        }   
    }


    private void OnLightButtonClick()
    {
        if(!isLightOn)
        {
            string message = "LightOn";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            _clientSocket.SendTo(messageBytes, _remoteEndPoint);

            // 버튼 텍스트 변경
            isLightOn = true;
            lightButton.GetComponentInChildren<Text>().text = isLightOn ? "LightOff" : "LightOn";

            Debug.Log("Light " + (isLightOn ? "On" : "Off"));
        }
        else
        {
            string message = "LightOff";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            _clientSocket.SendTo(messageBytes, _remoteEndPoint);

            // 버튼 텍스트 변경
            isLightOn = false;
            lightButton.GetComponentInChildren<Text>().text = isLightOn ? "LightOff" : "LightOn";

            Debug.Log("Light " + (isLightOn ? "On" : "Off"));
        }
    }

    private void OnTemperatureButtonClick()
    {
        string message = "Temperature";
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        _clientSocket.SendTo(messageBytes, _remoteEndPoint);
        Debug.Log("Temperature On");
    }

    private void OnExitButtonClick()
    {
        uiPanel.SetActive(false);
        Cursor.visible = false; // 마우스 커서를 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 고정함
        moveScript.enabled = true; // Move3D 스크립트를 활성화함

        // 라즈베리파이 기기와 연결 종료
        Debug.Log("Connection Close");
        _clientSocket.Close();
    }


    private void ConnectToServer()
    {
        string message = "Connect";
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);

        _clientSocket.SendTo(messageBytes, _remoteEndPoint);

        Debug.Log("Connected to Raspberry Pi device");
    }

    private void ReceiveData()
    {
        // 무한 루프로 데이터 수신을 대기
        while (true)
        {
            byte[] receiveBytes = new byte[1024];
            int n = _clientSocket.Receive(receiveBytes);
            string receiveMessage = Encoding.ASCII.GetString(receiveBytes, 0, n);
            Debug.Log("Received: " + receiveMessage);
        }
        
    }
}
