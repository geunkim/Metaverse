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
using System.Threading.Tasks;

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
        public string ipAddress;  // ����̽� IP �ּ�
        public int port;  // ����̽� ��Ʈ ��ȣ
        public Socket socket;

        public Device(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }
    }



    public class IoTConnection : MonoBehaviour
    {
        public string devicesFilePath;      // ����̽� ������ ����� JSON ���� ���
        private List<Device> devices = new List<Device>();  // ����̽� ������ ������ ����Ʈ

        public GameObject uiPanel;          // UI �г�
        public GameObject uiPanel2;         // UI �г�2

        public Button deviceButtonPrefab;   // ����̽� ��ư ������ ���� ������Ʈ�� �����ϴ� ����

        public Text deviceNameText;         // ���õ� ����̽��� �̸��� ǥ���ϴ� UI �ؽ�Ʈ�� �����ϴ� ����
        public Text deviceIPText;           // ���õ� ����̽��� IP �ּҸ� ǥ���ϴ� UI �ؽ�Ʈ�� �����ϴ� ����
        public Text deviceStatusText;       // ���õ� ����̽��� ���� ���¸� ǥ���ϴ� UI �ؽ�Ʈ�� �����ϴ� ����

        private Device selectedDevice;      // ���� ���õ� ����̽��� �����ϴ� ����

        public Button lightButton;
        public Button exitButton;
        public Button temButton;
        public Button disButton;

        public string recvmessage;

        public bool isLightOn = false; // ������ �����ִ��� ����

        void Start()
        {
            LoadDevices();  // JSON ���Ͽ��� ����̽� ������ �о�� ����Ʈ�� �߰�

            // ���� ��ư�� Ŭ�� ������ �߰�
            lightButton.onClick.AddListener(OnLightButtonClick);
            // exit ��ư�� Ŭ�� ������ �߰�
            exitButton.onClick.AddListener(OnExitButtonClick);
            // �µ� ��ư�� Ŭ�� ������ �߰�
            temButton.onClick.AddListener(OnTemperatureButtonClick);
            // �Ÿ� ��ư�� Ŭ�� ������ �߰�
            disButton.onClick.AddListener(OnDistanceButtonClick);

            uiPanel.SetActive(false);
            uiPanel2.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("player"))
            {
                uiPanel.SetActive(true);  // UI �г� Ȱ��ȭ

                CreateDeviceButtons();
            }
        }

        // uiPanel ��ư ���� �޼���
        private void CreateDeviceButtons()
        {
            foreach (Device device in devices)
            {
                Button button = Instantiate(deviceButtonPrefab, uiPanel.transform);
                button.GetComponentInChildren<Text>().text = "Device " + (devices.IndexOf(device) + 1).ToString();
                button.onClick.AddListener(() => DeviceButtonClicked(device)); 
            }
        }

        // ����̽� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
        private void DeviceButtonClicked(Device device)
        {
            uiPanel.SetActive(false);
            uiPanel2.SetActive(true);

            // ���õ� ����̽� ����
            selectedDevice = device;

            // ���õ� ����̽��� ����
            ConnectToDevice(selectedDevice);
        }

        // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
        private void OnLightButtonClick()
        {
            if (!isLightOn)
            {
                string message = "LightOn";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                selectedDevice.socket.Send(messageBytes);

                // ��ư �ؽ�Ʈ ����
                isLightOn = true;
                lightButton.GetComponentInChildren<Text>().text = isLightOn ? "LightOff" : "LightOn";

                Debug.Log("Light " + (isLightOn ? "On" : "Off"));
            }
            else
            {
                string message = "LightOff";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                selectedDevice.socket.Send(messageBytes);

                // ��ư �ؽ�Ʈ ����
                isLightOn = false;
                lightButton.GetComponentInChildren<Text>().text = isLightOn ? "LightOff" : "LightOn";

                Debug.Log("Light " + (isLightOn ? "On" : "Off"));
            }
        }

        // �µ� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
        private void OnTemperatureButtonClick()
        {
            if (selectedDevice != null)
            {
                string message = "Temperature";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                StartCoroutine(UpdateButtonText(temButton, recvmessage, message, 3.0f));
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

        // �Ÿ� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
        private  async void OnDistanceButtonClick()
        {
            if (selectedDevice != null)
            {
                string message = "Distance";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                
                try
                {
                    selectedDevice.socket.Send(messageBytes);
                    Debug.Log("Distance On");

                    await Task.Delay(100); // ������ ���� �� �ణ�� ����

                    StartCoroutine(UpdateButtonText(disButton, recvmessage, message, 3.0f));
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

        // exit ��ư Ŭ�� �� ȣ��Ǵ� �޼���
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
                // ����̽��κ��� �����͸� ���� ����
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

        // ����̽��κ��� �����͸� �����ϴ� �޼���
        private async void ReceiveData(Device device)
        {
            try
            {
                while (device.socket != null && device.socket.Connected)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await device.socket.ReceiveAsync(buffer, SocketFlags.None);

                    if (bytesRead > 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Debug.Log("Received message from device: " + message);
                        recvmessage = message;

                        
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to receive data from device: " + e.Message);
            }
        }


        // ��ư �ؽ�Ʈ�� �ӽ÷� �����ϴ� �ڷ�ƾ
        private IEnumerator UpdateButtonText(Button clickbutton, string newText, string originalText, float duration)
        {
            // ��ư �ؽ�Ʈ ����
            Button button = clickbutton; // �ش� ��ư�� �°� ����
            Text buttonText = button.GetComponentInChildren<Text>();
            string originalButtonText = buttonText.text;
            buttonText.text = newText;

            yield return new WaitForSeconds(duration);

            buttonText.text = originalText;

        }

    }
}
