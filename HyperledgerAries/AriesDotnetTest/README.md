# AriesDotnetTest

이 저장소는 Hyperledger Aries .NET을 사용해 C# 환경에서 Hyperledger Aries가 제대로 동작하는지 테스트한다. 테스트의 경우 두 에이전트 간의 연결 및 메시지 교환을 중심으로 테스트한다.

- Connection Protocol

## 테스트 환경 

.NET 버전과 사용 패키지 정보의 경우 .csproj 파일을 통해 확인할 수 있다.

- 운영체제 : Window 10 
- 사용 언어 : C# (.NET 7)
- 사용 패키지 : Hyperledger Aries .NET "1.6.4", Hyperledger Indy "1.11.1" (indy-sdk dotnet wrapper)
- 개발 툴 : Visual Sudio Code 

## 테스트 환경 조정

먼저 [해당 링크](https://dotnet.microsoft.com/ko-kr/download)를 통해 .NET 7 버전을 다운 받는다. .NET 설치가 완료되면 다음 명령어를 통해 확인할 수 있다.

    > dotnet --version

프로젝트 실행 시 다음을 통해 시작할 수 있다.

    > dotnet run

## 코드

- 코드 내용의 경우 Program.cs 파일에 작성되어 있다.

### 실행 문제

- Connection Protocol의 초대장을 만드는 도중 프로그램이 멈춰 끝나지 않는 오류 발생
  - 83번째 줄의 'CreateInvitationAsync' 함수 호출에서 프로그램이 멈춤
  - Connection Protocol 사용을 위해 만든 'DefaultConnectionService' 객체 생성에 문제가 있다고 판단
    - 76번째 줄의 'DefaultConnectionService' 객체 생성을 위한 파라미터 값들의 정의가 올바른지, 추가로 필요한 값이 있는지 확인 필요
  - 42번째 줄의 AgentContext의 경우 문제 없음 