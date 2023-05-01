Python 3.11.2 (tags/v3.11.2:878ead1, Feb  7 2023, 16:38:35) [MSC v.1934 64 bit (AMD64)] on win32
Type "help", "copyright", "credits" or "license()" for more information.
import paho.mqtt.client as mqtt
import ssl
import time

# AWS IoT 서비스에서 제공하는 인증서와 개인키 경로
cert_path = "certificate.pem.crt"
key_path = "private.pem.key"

# AWS IoT 서비스에서 제공하는 엔드포인트 주소와 포트번호
host = "a36q1930sr47ba-ats.iot.us-west-2.amazonaws.com"
port = 8883

# 인증서와 개인키를 이용하여 AWS IoT 서비스에 연결하는 함수
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to AWS IoT broker.")
...     else:
...         print("Connection failed with error code " + str(rc))
... 
... # AWS IoT 서비스에서 메시지를 받았을 때 처리하는 함수
... def on_message(client, userdata, msg):
...     print(msg.topic + " " + str(msg.payload))
... 
... client = mqtt.Client()
... client.tls_set(cert_path, key_path, certfile=None, keyfile=None, cert_reqs=ssl.CERT_REQUIRED, tls_version=ssl.PROTOCOL_TLS, ciphers=None)
... client.on_connect = on_connect
... client.on_message = on_message
... 
... try:
...     client.connect(host, port, keepalive=60)
... except ConnectionRefusedError:
...     print("Connection refused: Check if the endpoint is correct.")
... except ssl.SSLError:
...     print("SSL Error: Check if the certificate and private key paths are correct.")
... except Exception as e:
...     print("Error: " + str(e))
... 
... client.subscribe("myTopic", qos=0)
... 
... # AWS IoT 서비스로 메시지를 보내는 예시 코드
... try:
...     while True:
...         message = "Hello, AWS IoT!"
...         client.publish("myTopic", message)
...         time.sleep(1)
... except KeyboardInterrupt:
...     print("Disconnected.")
... except Exception as e:
...     print("Error: " + str(e))
... 
... client.disconnect()
>>> [DEBUG ON]
>>> [DEBUG OFF]
