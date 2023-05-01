using System;
using System.Text;
using System.Threading;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTManager : MonoBehaviour
{
    private MqttClient mqttClient;

    void Start()
    {
        string brokerAddress = "a36q1930sr47ba-ats.iot.us-west-2.amazonaws.com";
        string clientId = Guid.NewGuid().ToString();
        mqttClient = new MqttClient(brokerAddress);
        mqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;

        // connect to MQTT broker
        var result = mqttClient.Connect(clientId);
        if (result != 0)
        {
            Debug.LogError($"Failed to connect to MQTT broker: {result}");
            return;
        }

        mqttClient.Subscribe(new string[] { "myTopic" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        // Publish a message every second
        ThreadPool.QueueUserWorkItem(state =>
        {
            while (true)
            {
                string message = "Hello, MQTT!";
                mqttClient.Publish("myTopic", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);

                Thread.Sleep(1000);
            }
        });
    }



    void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string message = Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received message: " + message);
    }

    void OnDestroy()
    {
        if (mqttClient != null)
        {
            mqttClient.Disconnect();
        }
    }
}
