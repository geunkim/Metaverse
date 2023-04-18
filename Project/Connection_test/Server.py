import socket
import threading

# 서버의 IP 주소와 포트 번호
ip_address = '127.0.0.1'
port = 12345

# 연결된 클라이언트들을 저장할 리스트
clients = []

# 소켓 생성
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# 서버 바인딩
server_socket.bind((ip_address, port))

# 서버 대기
server_socket.listen()

# 클라이언트와 통신할 함수
def handle_client(client_socket, address):
    # 연결된 클라이언트 추가
    clients.append(client_socket)

    # 클라이언트와 통신
    while True:
        data = client_socket.recv(1024)

        # 클라이언트로부터 데이터를 받으면, 다른 클라이언트들에게 데이터를 전송
        clients_copy = clients.copy()
        for client in clients_copy:
            if client != client_socket:
                try:
                    client.send(data)
                except ConnectionResetError:
                    clients.remove(client)
                    client.close()

        # 클라이언트와 연결이 끊어지면, 연결된 클라이언트 리스트에서 제거
        if not data:
            clients.remove(client_socket)
            client_socket.close()
            break


# 클라이언트 연결을 받아들이는 함수
def accept_clients():
    while True:
        client_socket, address = server_socket.accept()
        print("Connected by", address)

        # 클라이언트와 통신하는 쓰레드 생성
        client_thread = threading.Thread(target=handle_client, args=(client_socket, address))
        client_thread.start()

# 클라이언트 연결 받기 시작
accept_clients()


