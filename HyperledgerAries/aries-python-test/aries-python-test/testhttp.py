from http.server import HTTPServer, BaseHTTPRequestHandler

HOST = "0.0.0.0"
PORT = 9900

class HTTPServer(BaseHTTPRequestHandler):

    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-type", "text/html")
        self.end_headers()

        self.wfile.write(bytes("<html><body><h1>Test HTTP Server</h1></body></html>"))

test_server = HTTPServer((HOST, PORT), HTTPServer)

print("running")
print(test_server)
print(HOST)
test_server.serve_forever()
test_server.server_close()
print("stopped")