# indy-python

indy-sdk를 이용한 python 테스트용 코드

개발환경 : Window 10, python3, pip3, libindy 1.16.0

내부 코드는 다음과 같은 기능을 테스트한다.
- Pool 연결
- 지갑 생성
- DID 생성 및 저장 (개인키 공개키 생성)
- VC 생성 및 저장
- VP 생성 및 증명

## libindy 설치

해당 코드는 indy-sdk 기능을 사용하며 이를 읽기 위해선 libindy 공유 라이브러리를 읽을 수 있어야 한다.

- 윈도우 libindy : https://repo.sovrin.org/windows/libindy/stable/1.16.0/libindy_1.16.0.zip

위 libindy를 다운받은 뒤, 해당 경로(설치 디렉토리)로 작성된 코드를 수정해야한다.

```python
# 아래 코드의 경로 수정
# os.add_dll_directory의 경우 Window 한정
os.add_dll_directory("D:\libindy_1.16.0\lib") 
```

## 실행 환경 구축

윈도우 환경에서 실행하였으며 python 3.11.3 버전 사용

    > python --version
    Python 3.11.3

pip를 사용해 패키지 다운

    > pip --version
    pip 23.1

패키지의 경우 indy-python 폴더로 이동 후 pip로 패키지 다운

    > cd indy-python
    > pip install -r requirements.txt --user

## 실행

python을 사용해 코드 실행

    > python negotiate_proof.py

## 코드 내용

코드는 [indy-sdk python의 예제 코드]()를 가져왔다.
