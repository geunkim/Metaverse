import express from 'express';
import * as polyfill from 'credential-handler-polyfill';
const app = express();
const port = 3000;

async function test() {

  await polyfill.loadOnce();
console.log('Ready to work with credentials!');
}

/*
credentialHandlerPolyfill
    .loadOnce(MEDIATOR)
    .then(console.log('Polyfill loaded.'))
    .catch(e => console.error('Error loading polyfill:', e));
*/

app.get('/', (req, res) => {
  res.sendFile(__dirname + '/main.html');
  test();
});

app.listen(port, () => {
  console.log(`Server listening at http://localhost:${port}`);
});
