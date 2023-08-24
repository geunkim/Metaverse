var http = require('http');
var options = {
    hostname: '220.68.5.140',
    port: '8080',
    path: '/pool_transactions_genesis'
  };
function handleResponse(response) {
  var serverData = '';
  response.on('data', function (chunk) {
    serverData += chunk;
  });
  response.on('end', function () {
    console.log(serverData);
  });
}
http.request(options, function(response){
  handleResponse(response);
}).end();