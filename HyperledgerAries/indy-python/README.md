# indy-python

indy-sdk를 이용한 python 테스트용 코드

## 실행 내용

내부 코드는 다음과 같은 기능을 테스트한다.
- Pool 연결
- 지갑 생성
- DID 생성 및 저장 (개인키 공개키 생성)
- VC 생성 및 저장
- VP 생성 및 증명

## 실행 방법

개발환경 : Window 10, python3, pip3, libindy 1.16.0

### libindy 설치

해당 코드는 indy-sdk 기능을 사용하며 이를 읽기 위해선 libindy 공유 라이브러리를 읽을 수 있어야 한다.

- 윈도우 libindy : https://repo.sovrin.org/windows/libindy/stable/1.16.0/libindy_1.16.0.zip

위 libindy를 다운받아 압축을 푼 뒤, 해당 경로(설치 디렉토리)를 코드 내의 'os.add_dll_directory'의 입력 값으로 지정해야한다. 

```python
# os.add_dll_directory는 공유 라이브러리를 읽어오는 기능 (libindy.dll)
# 코드 내의 os.add_dll_directory 뒤에 libindy를 다운 받아 압푹을 푼 디렉토리 경로를 뒤에 입력
# os.add_dll_directory의 경우 Window 한정
os.add_dll_directory("D:\libindy_1.16.0\lib") 
```

### 풀 연결

libindy를 사용한 풀 연결을 위해선 풀이 가동되어 있어야 하며 이를 접속하기 위해선 'pool_transactions_genesis' 파일이 필요하다.
'pool_transactions_genesis' 파일은 2가지 방법을 통해 가져온다.

1. 로컬에 저장된 'pool_transactions_genesis' 파일을 읽는다.
2. 서버에 요청해 'pool_transactions_genesis' 파일을 받아 읽는다.

#### 기존의 풀 연결

현재 이미 가동 중인 풀이 있으며 접속하기 위한 코드는 'negotiate_proof.py'의 'proof_negotiation' 함수 또는 'pool_connect_test' 함수를 통해 확인할 수 있다.

#### 테스트 용 풀 생성

새로운 테스트 풀을 만들어 사용하기 위해선 Docker를 사용해 풀을 만들어야 하며 아래 두가지 방법을 제안한다.

1. [von-network를 사용한 풀 환경 구축](https://github.com/bcgov/greenlight/blob/master/docker/VONQuickStartGuide.md)
2. [indy-sdk의 로컬 풀 환경 구축](https://github.com/hyperledger/indy-sdk#how-to-start-local-nodes-pool-with-docker)

1번 사용시 'genesis_file_url', 2번 사용시 'pool_transactions_genesis' 경로에 맞춰 'genesis_file_path' 값을 수정한다.

### 코드 실행 환경 구축

윈도우 환경에서 실행하였으며 python 3.11.3 버전 사용

    > python --version
    Python 3.11.3

pip를 사용해 패키지 다운

    > pip --version
    pip 23.1

패키지의 경우 indy-python 폴더로 이동 후 pip로 패키지 다운

    > cd indy-python
    > pip install -r requirements.txt --user

### 실행

python을 사용해 코드 실행

    > python negotiate_proof.py

### 코드 내용

코드는 [indy-sdk python의 예제 코드](https://github.com/hyperledger/indy-sdk/blob/main/docs/how-tos/negotiate-proof/python/negotiate_proof.py)를 가져왔다. 자세한 내용은 [indy-sdk의 'How Tos'](https://github.com/hyperledger/indy-sdk/tree/main/docs/how-tos)에서 확인 가능하다.
