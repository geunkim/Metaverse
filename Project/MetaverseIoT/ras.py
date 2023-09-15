import paho.mqtt.client as mqtt

def on_connect(client, userdata, flags, rc):
    print(f"Connected with result code {rc}")
    
def on_message(client, userdata, msg):
    print(f"Topic: {msg.topic}\nMessage: {str(msg.payload)}")

client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

client.connect("192.168.0.19", 1883, 60)

client.loop_start()

# 특정 토픽 구독
client.subscribe("unityclient)