using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using myspace;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections;

namespace myspace
{
    // UI 관리 클래스
    public class UIManager : MonoBehaviour
    {

        private List<Device> devices;          // 디바이스 정보를 저장할 리스트

        public GameObject uiPanel;          // UI 패널
        public GameObject uiPanel2;         // UI 패널2
        public Button deviceButtonPrefab;   // 디바이스 버튼 프리팹 게임 오브젝트를 저장하는 변수

        public Button lightButton;
        public Button exitButton;
        public Button temButton;
        public Button disButton;

        public bool isLightOn = false; // 조명이 켜져있는지 여부

        DeviceConnectionManager deviceConnectionManager;

        private Device selectedDevice;      // 현재 선택된 디바이스를 저장하는 변수

        void Start()
        {
            deviceConnectionManager = FindObjectOfType<DeviceConnectionManager>();
            devices = ConnectionManager.devices;
            selectedDevice = deviceConnectionManager.selectedDevice;
           
            CreateDeviceButtons();

            // 조명 버튼에 클릭 리스너 추가
            lightButton.onClick.AddListener(OnLightButtonClick);
            // exit 버튼에 클릭 리스너 추가
            exitButton.onClick.AddListener(OnExitButtonClick);
            // 온도 버튼에 클릭 리스너 추가
            temButton.onClick.AddListener(OnTemperatureButtonClick);
            // 거리 버튼에 클릭 리스너 추가
            disButton.onClick.AddListener(OnDistanceButtonClick);

            uiPanel.SetActive(false);
            uiPanel2.SetActive(false);
        }

        private void CreateDeviceButtons()
        {
            foreach (Device device in devices)
            {
                Button button = Instantiate(deviceButtonPrefab, uiPanel.transform);
                button.GetComponentInChildren<Text>().text = "Device " + (devices.IndexOf(device) + 1).ToString();
                button.onClick.AddListener(() => DeviceButtonClicked(device));
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
            deviceConnectionManager.ConnectToDevice(selectedDevice);
            
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
                    message = deviceConnectionManager.receivedData;
                    StartCoroutine(UpdateButtonText(temButton, message, "Temperature", 3.0f));
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

        // 거리 버튼 클릭 시 호출되는 메서드
        private void OnDistanceButtonClick()
        {
            if (selectedDevice != null)
            {
                string message = "Distance";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                try
                {
                    selectedDevice.socket.Send(messageBytes);
                    Debug.Log("Distance On");
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
            deviceConnectionManager.DisconnectFromDevice(selectedDevice);

            uiPanel2.SetActive(false);
            uiPanel.SetActive(true);
        }

        // 버튼 텍스트를 임시로 변경하는 코루틴
        private IEnumerator UpdateButtonText(Button clickbutton, string newText, string originalText, float duration)
        {
            // 버튼 텍스트 변경
            Button button = clickbutton; // 해당 버튼에 맞게 수정
            Text buttonText = button.GetComponentInChildren<Text>();
            string originalButtonText = buttonText.text;
            buttonText.text = newText;

            yield return new WaitForSeconds(duration);

            buttonText.text = originalText;

        }
    }
}
