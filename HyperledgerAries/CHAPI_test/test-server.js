/*
async function f() {
  const polyfill = window.credentialHandlerPolyfill;
// must be run from an async function if top-level await is unavailable
  await polyfill.loadOnce();
}

f()
console.log('Ready to work with credentials!');
*/

var http = require('http');
var url = require('url');
var querystring = require('querystring'); 
var fs = require('fs');

const hostname = '127.0.0.1';
const port = 3000;

var server = http.createServer(function(req,res){
  console.log('--- log start ---');

  // 4. 브라우저에서 요청한 주소를 parsing 하여 객체화 후 출력
  var parsedUrl = url.parse(req.url);
  console.log(parsedUrl);

  // 5. 객체화된 url 중에 Query String 부분만 따로 객체화 후 출력
  var parsedQuery = querystring.parse(parsedUrl.query,'&','=');
  console.log(parsedQuery);  

  // 6. 콘솔화면에 로그 종료 부분을 출력
  console.log('--- log end ---');

  res.writeHead(200,{'Content-Type':'text/html'});
  res.end('Hello World!!');
   
  });

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});