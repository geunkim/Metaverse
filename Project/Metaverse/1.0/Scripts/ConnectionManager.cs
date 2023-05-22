using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace myspace
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

    public class ConnectionManager : MonoBehaviour
    {
        public string devicesFilePath;      // 디바이스 정보가 저장된 JSON 파일 경로
        static public List<Device> devices = new List<Device>();  // 디바이스 정보를 저장할 리스트

        private Device selectedDevice;      // 현재 선택된 디바이스를 저장하는 변수

        void Start()
        {
            LoadDevices();  // JSON 파일에서 디바이스 정보를 읽어와 리스트에 추가
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
    }
}
