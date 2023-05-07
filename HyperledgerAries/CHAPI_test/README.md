# CHAPI 분석

# CHAPI 란?

Credential Handler API(이하 CHAPI)란 W3C에서 표준화하고 있는 기술로 자격 증명 기술의 표준이다. 웹 상에서 자격 증명 사용을 위한 표준을 작성하고 있으며, 이를 통해 웹사이트에서 DID 기반 인증 및 디지털 자격증을 관리할 수 있다. 

CHAPI의 최종 목표는 다음과 같다. 

1. 사용자가 자격 증명을 더 쉽고 안전하게 사용하도록 만들기
2. 사용자에게 지갑 공급자를 선택할 수 있는 기능 제공
3. 웹 앱 개발자에게 표준 지갑 API 제공

CHAPI 공식 사이트 : [https://chapi.io/](https://chapi.io/)

W3C Credential Handler API 1.0 : [https://w3c-ccg.github.io/credential-handler-api/](https://w3c-ccg.github.io/credential-handler-api/)

Credential Handler Team (Github) : [https://github.com/credential-handler](https://github.com/credential-handler)

# CHAPI Polyfill 란?

Credential Handler API (CHAPI) polyfill은 CHAPI 표준을 구현한 프로젝트로 JavaScript 기반이다.  credential-handler-polyfill 라이브러리를 추가하여 사용할 수 있으며 이를 통해 웹 개발자는 쉽게 자격 증명 처리 기능을 추가할 수 있다. 

CHAPI의 동작은 WebCredential을 기반으로 동작하며 이는 Credential Management에 속해있다. 

credential-handler-polyfill (Github) : https://github.com/credential-handler/credential-handler-polyfill

W3C credential-management-1 : [https://www.w3.org/TR/credential-management-1/#introduction](https://www.w3.org/TR/credential-management-1/#introduction)

## CHAPI Polyfill 사용

### Node.js를 사용한 웹 서버 가동

먼저 Node.js를 설치한다. Node.js는 다음 링크에서 설치 가능하다. → [https://nodejs.org/ko](https://nodejs.org/ko)

Node.js 설치 이후 실행을 위한 폴더 및 파일을 만든다. 아래의 코드는 ‘Hello World’를 출력하는 웹 서버 코드이다. 

- server.js

```jsx
var http = require('http');

var server = http.createServer(function(request,response){ 

    response.writeHead(200,{'Content-Type':'text/html'});
    response.end('Hello World!!');

});

server.listen(8080, function(){ 
    console.log('Server is running...');
});
```

위 파일 생성 이후 파일 위치로 이동해 실행시키면 웹 서버가 실행되며 ‘Hello World!!’를 확인할 수 있다. → [http://127.0.0.1:3000/](http://127.0.0.1:3000/)

```bash
node server.js
```

- [http://127.0.0.1:3000/](http://127.0.0.1:3000/) 링크로 이동 시 가동 중인 웹 서버 접속이 가능

![20230427_CHAPI테스트_1.png](Image/20230427_CHAPI_1.png)

### Node.js를 사용한 패키지 관리

CHAPI polyfill는 Node.js의 패키지 매니저로 다운 받을 수 있으며 이 때 npm을 사용한다. 

<aside>
💡 npm은 ‘Node Packge Manager’로 Node.js에서 사용하는 다양한 패키지들을 관리하고 다운받을 수 있게 만들어준다. npm은 Node.js 설치 시 자동으로 같이 설치하며 ‘npm --v’을 통해 동작과 버전을 확인할 수 있다.

</aside>

Node.js로 코드 작업을 진행할 폴더로 이동해 프로젝트 환경을 만들어준다.

```bash
npm init
```

위 명령어 입력 시 추가적으로 입력 사항이 나오며 필요에 따라 입력해주면 된다. 이후 package.json 파일이 생성되며 해당 파일을 통해 프로젝트에서 사용하는 패키지를 관리할 수 있다. 

- packge.json

```json
{
  "name": "credential-handler-test",
  "version": "1.0.0",
  "description": "",
  "main": "server.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "start": "node server.js"
  },
  "author": "",
  "license": "ISC",
  "dependencies": {
    "credential-handler-polyfill": "^3.2.0"
  }
}
```

- name : 프로젝트 이름
- main : 프로젝트의 메인이 되는 실행되는 파일
- scripts : 프로젝트 실행의 자동화를 위해 작성하는 부분으로 npm 이후 작성에 따라 뒤의 명령어를 실행시켜준다.
- dependencies : 프로젝트의 패키지를 관리하는 부분으로 프로젝트에서 사용되는 패키지들을 기록하여 관리할 수 있다.

위 항목 중 dependencies 부분이 프로젝트에서 사용하는 패키지를 관리하는 부분으로 필요한 패키지를 작성한다. CHAPI polyfill의 경우 dependencies에 위와 같이 추가하여 패키지를 설치한다.

```bash
npm install // packge.json을 확인해 "dependencies"의 패키지들을 설치
npm start // scripts 작성에 따라 "node server.js" 명령어 실행
```