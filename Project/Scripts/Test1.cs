using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;
using System.Text;
using System;
using System.Threading;




namespace space
{
    [System.Serializable]
    public class DeviceList
    {
        public Device[] devices;
    }

    [System.Serializable]
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



    public class Test1 : MonoBehaviour
    {
        public string devicesFilePath;      // 디바이스 정보가 저장된 JSON 파일 경로
        private List<Device> devices = new List<Device>();  // 디바이스 정보를 저장할 리스트

        public GameObject uiPanel;          // UI 패널
        public GameObject uiPanel2;         // UI 패널2

        public Button deviceButtonPrefab;   // 디바이스 버튼 프리팹 게임 오브젝트를 저장하는 변수

        public Text deviceNameText;         // 선택된 디바이스의 이름을 표시하는 UI 텍스트를 저장하는 변수
        public Text deviceIPText;           // 선택된 디바이스의 IP 주소를 표시하는 UI 텍스트를 저장하는 변수
        public Text deviceStatusText;       // 선택된 디바이스의 연결 상태를 표시하는 UI 텍스트를 저장하는 변수

        private Device selectedDevice;      // 현재 선택된 디바이스를 저장하는 변수

        public Button lightButton;
        public Button exitButton;
        public Button temButton;

        public bool isLightOn = false; // 조명이 켜져있는지 여부

        void Start()
        {
            LoadDevices();  // JSON 파일에서 디바이스 정보를 읽어와 리스트에 추가

            // 조명 버튼에 클릭 리스너 추가
            lightButton.onClick.AddListener(OnLightButtonClick);
            // exit 버튼에 클릭 리스너 추가
            exitButton.onClick.AddListener(OnExitButtonClick);
            // 온도 버튼에 클릭 리스너 추가
            temButton.onClick.AddListener(OnTemperatureButtonClick);

            uiPanel.SetActive(false);
            uiPanel2.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("player"))
            {
                uiPanel.SetActive(true);  // UI 패널 활성화

                CreateDeviceButtons();
            }
        }

        // uiPanel 버튼 생성 메서드
        private void CreateDeviceButtons()
        {
            foreach (Device device in devices)
            {
                Button button = Instantiate(deviceButtonPrefab, uiPanel.transform);
                button.GetComponentInChildren<Text>().text = "Device " + (devices.IndexOf(device) + 1).ToString();
                button.onClick.AddListener(() => DeviceButtonClicked(device)); // 수정된 부분
            }
        }

        // 디바이스 버튼 클릭 시 호출되는 메서드
        private void DeviceButtonClicked(Device device)
        {
            uiPanel.SetActive(false);
            uiPanel2.SetActive(true);

            // 선택된 디바이스 설정
            selectedDevice = device;

            // 선택된 디바이스에 연결
            ConnectToDevice(selectedDevice);
        }

        // 전구 버튼 클릭 시 호출되는 메서드
        private void OnLightButtonClick()
        {
            if (!isLightOn)
            {
                string message = "LightOn";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                selectedDevice.socket.Send(messageBytes);

                // 버튼 텍스트 변경
                isLightOn = true;
                lightButton.GetComponentInChildren<Text>().text = isLightOn ? "LightOff" : "LightOn";

                Debug.Log("Light " + (isLightOn ? "On" : "Off"));
            }
            else
            {
                string message = "LightOff";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                selectedDevice.socket.Send(messageBytes);

                // 버튼 텍스트 변경
                isLightOn = false;
                lightButton.GetComponentInChildren<Text>().text = isLightOn ? "LightOff" : "LightOn";

                Debug.Log("Light " + (isLightOn ? "On" : "Off"));
            }
        }

        // 온도 버튼 클릭 시 호출되는 메서드
        private void OnTemperatureButtonClick()
        {
            if (selectedDevice != null)
            {
                string message = "Temperature";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                try
                {
                    selectedDevice.socket.Send(messageBytes);
                    Debug.Log("Temperature On");
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to send data to the device: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("No device selected!");
            }
        }

        // exit 버튼 클릭 시 호출되는 메서드
        private void OnExitButtonClick()
        {
            DisconnectFromDevice(selectedDevice);

            uiPanel2.SetActive(false);
            uiPanel.SetActive(true);
        }

        private void LoadDevices()
        {
            if (File.Exists(devicesFilePath))
            {
                string json = File.ReadAllText(devicesFilePath);
                DeviceList deviceList = JsonUtility.FromJson<DeviceList>(json);
                devices = new List<Device>(deviceList.devices);
                Debug.Log("Devices loaded: " + devices.Count);
            }
            else
            {
                Debug.LogError("Devices file not found!");
            }
        }

        private void ConnectToDevice(Device device)
        {
            try
            {

                IPAddress ipAddress = IPAddress.Parse(device.ipAddress);
                IPEndPoint endPoint = new IPEndPoint(ipAddress, device.port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                device.socket = socket;
                //deviceStatusText.text = "Status: Connected";
                Debug.Log("Connected to device: " + (devices.IndexOf(device) + 1).ToString());
                // 디바이스로부터 데이터를 수신 시작
                ReceiveData(device);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to connect to device: " + e.Message);
            }
        }

        private void DisconnectFromDevice(Device device)
        {
            try
            {
                if (device.socket != null && device.socket.Connected)
                {
                    device.socket.Shutdown(SocketShutdown.Both);
                    device.socket.Close();
                }
                device.socket = null;
                deviceStatusText.text = "Status: Not Connected";
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to disconnect from device: " + e.Message);
            }
        }

        // 디바이스로부터 데이터를 수신하는 메서드
        private void ReceiveData(Device device)
        {
            // 수신 작업을 담당하는 스레드 생성
            Thread receiveThread = new Thread(() =>
            {
                try
                {
                    while (device.socket != null && device.socket.Connected)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = device.socket.Receive(buffer);

                        if (bytesRead > 0)
                        {
                            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Debug.Log("Received message from device: " + message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to receive data from device: " + e.Message);
                }
            });

            // 스레드 시작
            receiveThread.Start();
        }

    }
}
