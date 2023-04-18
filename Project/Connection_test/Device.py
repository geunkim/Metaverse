import socket
import RPi.GPIO as GPIO
import threading

# 전구가 연결된 GPIO 핀 번호 설정
GPIO_PIN = 18

# GPIO 핀 설정
GPIO.setmode(GPIO.BCM)
GPIO.setup(GPIO_PIN, GPIO.OUT)

HOST = '192.168.0.1' # 라즈베리파이의 IP 주소
PORT = 8888 # 포트번호

def handle_client(client_socket, addr):
    while True:
        data = client_socket.recv(1024).decode()
        if not data:
            break

        # 받은 데이터가 'on'인 경우 전구 켜기
        if data == 'on':
            print('Turn on the light')
            GPIO.output(GPIO_PIN, True)

        # 받은 데이터가 'off'인 경우 전구 끄기
        elif data == 'off':
            print('Turn off the light')
            GPIO.output(GPIO_PIN, False)

        # 잘못된 데이터가 온 경우
        else:
            print('Invalid data')

    # 클라이언트 소켓 닫기
    client_socket.close()
    print('Disconnected by', addr)

def start_server():
    # 소켓 생성 및 연결 대기
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server_socket.bind((HOST, PORT))
    server_socket.listen()

    print('Server started')

    while True:
        # 클라이언트 접속 대기
        client_socket, addr = server_socket.accept()
        print('Connected by', addr)

        # 새로운 스레드 생성하여 클라이언트 처리
        client_thread = threading.Thread(target=handle_client, args=(client_socket, addr))
        client_thread.start()

    # 소켓 닫기
    server_socket.close()

if __name__ == '__main__':
    start_server()
