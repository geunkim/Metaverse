using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;
using System.Text;
using UnityEditor;

// Device 클래스: 디바이스 정보를 담는 클래스
public class Device
{
    public string ipAddress;  // 디바이스 IP 주소
    public int port;  // 디바이스 포트 번호
    public Socket socket;

    public Device(string ipAddress, int port)
    {
        this.ipAddress = ipAddress;
        this.port = port;
    }
}

public class ConnectionController : MonoBehaviour
{
    public string devicesFilePath;  // 디바이스 정보가 저장된 JSON 파일 경로
    private List<Device> devices = new List<Device>();  // 디바이스 정보를 저장할 리스트

    public GameObject uiPanel;  // UI 패널
    public GameObject uiPanel2;  // UI 패널2

    public Button deviceButtonPrefab; // 디바이스 버튼 프리팹 게임 오브젝트를 저장하는 변수
    //public GameObject devicePanel; // 디바이스 버튼을 생성할 패널 게임 오브젝트를 저장하는 변수

    public Text deviceNameText; // 선택된 디바이스의 이름을 표시하는 UI 텍스트를 저장하는 변수
    public Text deviceIPText; // 선택된 디바이스의 IP 주소를 표시하는 UI 텍스트를 저장하는 변수
    public Text deviceStatusText; // 선택된 디바이스의 연결 상태를 표시하는 UI 텍스트를 저장하는 변수

    private Device selectedDevice; // 현재 선택된 디바이스를 저장하는 변수

    public Button lightButton;
    public Button temperatureButton;
    public Button ExitButton;

    public bool isLightOn = false; // 조명이 켜져있는지 여부

    void Start()
    {
        LoadDevices();  // JSON 파일에서 디바이스 정보를 읽어와 리스트에 추가

        // 조명 버튼에 클릭 리스너 추가
        lightButton.onClick.AddListener(OnLightButtonClick);
        // 온도 버튼에 클릭 리스너 추가
        temperatureButton.onClick.AddListener(OnTemperatureButtonClick);
        // 종료 버튼에 클릭 리스너 추가
        ExitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Switch"))
        {
            // 모든 디바이스와 연결
            foreach (Device device in devices)
            {
                ConnectToDevice(device);
            }

            uiPanel.SetActive(true);  // UI 패널 활성화
            Cursor.visible = true;  // 마우스 커서를 보이게 함
            Cursor.lockState = CursorLockMode.None;  // 마우스 커서가 고정되지 않도록 함
        }
    }



    // JSON 파일에서 디바이스 정보를 읽어와 리스트에 추가
    private void LoadDevices()
    {
        if (File.Exists(devicesFilePath))
        {
            string json = File.ReadAllText(devicesFilePath);
            devices = JsonUtility.FromJson<List<Device>>(json);
            CreateDeviceButtons();
        }
        else
        {
            Debug.LogError("Devices file not found!");
        }
    }

    // 리스트에 추가된 디바이스 정보를 바탕으로 디바이스 버튼을 생성
    private void CreateDeviceButtons()
    {
        foreach (Device device in devices)
        {
            Button button = Instantiate(deviceButtonPrefab, uiPanel.transform);
            button.GetComponentInChildren<Text>().text = "Device " + (devices.IndexOf(device) + 1).ToString();
            button.onClick.AddListener(delegate { DeviceButtonClicked(device); });

            selectedDevice = device;
            deviceNameText.text = "Device " + (devices.IndexOf(device) + 1).ToString();
            deviceIPText.text = "IP Address: " + device.ipAddress;
            if (device.socket != null)
            {
                deviceStatusText.text = "Status: Connected";
            }
            else
            {
                deviceStatusText.text = "Status: Not Connected";
            }
        }
    }

    // 디바이스 버튼 클릭 시 Panel2 활성화
    private void DeviceButtonClicked(Device device)
    {
        uiPanel.SetActive(false);

        uiPanel2.SetActive(true); // 디바이스를 제어하는 버튼이 있는 UI 패널 활성화

    }

    private void OnLightButtonClick()
    {
        if (!isLightOn)
        {
            string message = "LightOn";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            //_clientSocket.SendTo(messageBytes, _remoteEndPoint);

            // 버튼 텍스트 변경
            isLightOn = true;
            lightButton.GetComponentInChildren<Text>().text = isLightOn ? "LightOff" : "LightOn";

            Debug.Log("Light " + (isLightOn ? "On" : "Off"));
        }
        else
        {
            string message = "LightOff";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            //_clientSocket.SendTo(messageBytes, _remoteEndPoint);

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
        //_clientSocket.SendTo(messageBytes, _remoteEndPoint);
        Debug.Log("Temperature On");
    }

    private void OnExitButtonClick()
    {
        uiPanel.SetActive(false);
        Cursor.visible = false; // 마우스 커서를 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 고정함
        //moveScript.enabled = true; // Move3D 스크립트를 활성화함

        // 라즈베리파이 기기와 연결 종료
        Debug.Log("Connection Close");
        //_clientSocket.Close();
    }

    // 디바이스와 연결하는 함수
    private void ConnectToDevice(Device device)
    {
        // 소켓 생성 및 연결
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse(device.ipAddress);
        IPEndPoint remoteEP = new IPEndPoint(ip, device.port);
        socket.Connect(remoteEP);
        device.socket = socket;

        // 연결이 성공하면 해당 디바이스와의 통신을 수행
        if (socket.Connected)
        {
            Debug.Log("Connected to device at " + device.ipAddress + ":" + device.port);
            // send to message for divice
            string message = "Hello from Unity";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            socket.Send(msg);
        }
        else
        {
            Debug.LogWarning("Failed to connect to device at " + device.ipAddress + ":" + device.port);
        }
    }

    // 스위치와의 접촉이 해제될 때 모든 디바이스와의 연결을 종료
    private void OnDestroy()
    {
        // 모든 디바이스와 연결 해제
        foreach (Device device in devices)
        {
            DisconnectFromDevice(device.ipAddress, device.port);
        }
    }

    // 해당 디바이스와의 연결 해제
    private void DisconnectFromDevice(string ipAddress, int port)
    {
        // 연결된 소켓 가져오기
        Socket socket = GetConnectedSocket(ipAddress, port);

        if (socket != null)
        {
            // 소켓 종료
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Debug.Log("Disconnected from device at " + ipAddress + ":" + port);
        }
        else
        {
            Debug.LogWarning("No connection found for device at " + ipAddress + ":" + port);
        }
    }

    // 주어진 IP 주소와 포트 번호에 해당하는 소켓 객체를 반환
    private Socket GetConnectedSocket(string ipAddress, int port)
    {
        // 소켓 생성
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 연결 시도
        try
        {
            socket.Connect(ipAddress, port);
            Debug.Log("Connected to device at " + ipAddress + ":" + port);
        }
        catch (SocketException e)
        {
            Debug.LogError("Failed to connect to device at " + ipAddress + ":" + port + ": " + e.Message);
            return null;
        }

        return socket;
    }

}
