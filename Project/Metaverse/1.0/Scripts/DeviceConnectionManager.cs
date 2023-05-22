using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;

namespace myspace
{
    
    // 디바이스와의 연결 관리 클래스
    public class DeviceConnectionManager : MonoBehaviour
    {
        public Device selectedDevice;      // 현재 선택된 디바이스를 저장하는 변수

        public string receivedData;

        public Text deviceNameText;         // 선택된 디바이스의 이름을 표시하는 UI 텍스트를 저장하는 변수
        public Text deviceIPText;           // 선택된 디바이스의 IP 주소를 표시하는 UI 텍스트를 저장하는 변수
        public Text deviceStatusText;       // 선택된 디바이스의 연결 상태를 표시하는 UI 텍스트를 저장하는 변수
        public DeviceConnectionManager(Device device)
        {
            selectedDevice = device;
        }

        public void ConnectToDevice(Device selectedDevice)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(selectedDevice.ipAddress);
                IPEndPoint endPoint = new IPEndPoint(ipAddress, selectedDevice.port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                // 연결 성공
                Debug.Log("Connected to device: " + selectedDevice.ipAddress);
                // 디바이스로부터 데이터를 수신 시작
                ReceiveData(socket);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to connect to device: " + e.Message);
            }
        }

        private void ReceiveData(Socket socket)
        {
            Thread receiveThread = new Thread(() =>
            {
                try
                {
                    while (socket.Connected)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = socket.Receive(buffer);
                        receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Debug.Log("Received data: " + receivedData);
                        // 수신한 데이터를 처리하는 로직 추가
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error while receiving data: " + e.Message);
                }
            });

            receiveThread.Start();
        }

        public void DisconnectFromDevice(Device device)
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
    }
}
