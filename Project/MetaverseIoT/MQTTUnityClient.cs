using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTUnityClient
{
    public string brokerIpAddress; // 브로커 IP 주소
    public string topic; // 구독할 토픽

    private MqttClient client; // MQTT 클라이언트 객체
    
    public void ConnectToBroker()
    {
        // 클라이언트 생성 및 연결
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

            byte[] inputBytes = Encoding.UTF8.GetBytes("Light"); // 원본 메시지
            byte[] cipherText = cipher.DoFinal(inputBytes);

            Console.WriteLine(Convert.ToBase64String(cipherText));


            cipher.Init(false, keyPair2.Private);
            byte[] plainTextBytes = cipher.DoFinal(cipherText);

            Console.WriteLine(Encoding.UTF8.GetString(plainTextBytes));

            var msg = Encoding.UTF8.GetBytes(plainTextBytes);

            client.Publish(topic, msg, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false); // 발행

        }
    }
}
