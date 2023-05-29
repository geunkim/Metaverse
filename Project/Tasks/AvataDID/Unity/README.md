### Unity에 DID(분산신원증명)를 적용 및 사용하기

1. DID 관련 라이브러리 설치 
  - Indy-SDK 사용, libindy 1.6.0 버전 사용 -> [링크](https://repo.sovrin.org/windows/libindy/stable/)
2. 다음의 글을 참고하여 Unity에 Indy-SDK 라이브러리 적용 -> [참고자료](https://github.com/geunkim/Metaverse/tree/main/HyperledgerAries/AvataDID)
3. 파이썬 코드에서 'UnityEngine'을 사용하여 진행을 Unity 로그에 출력, 이를 위해선 파이썬 코드는 유니티 프로젝트 내부에 위치해야 함
4. 실행 후 'K'를 눌러 PythonRunner.RunFile() 메서드를 호출해 DID 발급 및 관리 기능이 구현된 외부 파이썬 파일을 불러들임.

  - 해당 과정에서 'indy' 모듈을 찾을 수 없다는 에러가 발생
  - 파이썬 파일의 경로를 변경 -> 해결 X
  - cmd 창의 'pip install python3-indy' 를 입력해 indy 모듈 설치 -> 해결 X
  - 시스템 환경 변수 경로에 libindy의 dll 파일이 있는 경로를 추가 -> 해결 X
  - 파이썬 코드에 dll 파일이 있는 경로를 직접 추가 (sys.path.append('D:\indysdk\lib') 작성) -> 해결 X
  - 유니티가 아닌 VS Code에서 해당 파이썬 코드를 실행 시 'indy' 모듈을 문제없이 불러들임 -> 개발 환경 차이
    - VS Code에서 'indy' 모듈을 불러들인 곳의 경로를 추적하여 시스템 환경 변수 추가 -> 해결 X



![image](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/b73f8517-4750-44d5-bfb0-ee24e5b38335)
