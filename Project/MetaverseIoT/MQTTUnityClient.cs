using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTUnityClient
{
    public string brokerIpAddress; // ���Ŀ IP �ּ�
    public string topic; // ������ ����

    private MqttClient client; // MQTT Ŭ���̾�Ʈ ��ü
    
    public void ConnectToBroker()
    {
        // Ŭ���̾�Ʈ ���� �� ����
        client = new MqttClient(brokerIpAddress);
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        if (client.IsConnected)
        {
            Console.WriteLine("Connected to broker.");
            client.Subscribe(new string[] { topic }, new byte[] { 2 }); // QoS level 2

            RsaKeyPairGenerator rsaKeyPairGn = new RsaKeyPairGenerator();
            rsaKeyPairGn.Init(new KeyGenerationParameters(new SecureRandom(), 2048));

            AsymmetricCipherKeyPair keyPair1 = rsaKeyPairGn.GenerateKeyPair();
            AsymmetricCipherKeyPair keyPair2 = rsaKeyPairGn.GenerateKeyPair();

            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/None/OAEPWithSHA1AndMGF1Padding");
            cipher.Init(true, keyPair2.Public);

            byte[] inputBytes = Encoding.UTF8.GetBytes("Light"); // ���� �޽���
            byte[] cipherText = cipher.DoFinal(inputBytes);

            Console.WriteLine(Convert.ToBase64String(cipherText));


            cipher.Init(false, keyPair2.Private);
            byte[] plainTextBytes = cipher.DoFinal(cipherText);

            Console.WriteLine(Encoding.UTF8.GetString(plainTextBytes));

            var msg = Encoding.UTF8.GetBytes(plainTextBytes);

            client.Publish(topic, msg, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false); // ����

        }
    }
}
