# Hyperledger Aries

이 저장소는 Hyperledger Aries 구조, 코드 분석 및 테스트 코드 작성 결과를 공유하는 곳이다.

### [aries-python-test](./aries-python-test)

ACA-PY 코드의 로컬 테스트를 위한 저장소
- ACA-PY는 Cloud 기반의 동작을 위해 설계 되었으며 구현되어 있다. 
- ACA-PY는 사용자의 개인 정보가 Cloud에 올라가 있어 상황에 따라 개인정보의 소실 및 노출로 이어질 수 있다.
- 이러한 문제를 방지하기 위해 ACA-PY의 Cloud 기반 시스템을 클라이언트 단위로 내리는 작업이 필요하다.

'aries-python-test'는 ACA-PY의 코드를 Cloud 단에서 분리해 클라이언트 기반의 동작을 위한 테스트를 실시한다.

### [ACA-PY 실행 테스트](./StartACA-PY.md.md)

ACA-PY 실행에 대한 내용을 담는다.

### [ACA-PY 코드 분석](./ACA-PY_CodeAnalysis.md)

ACA-PY의 코드 분석 내용을 정리한 문서

### [Hyperledger Aries 분석](./HyperledgerAries.md)

Hyperledger Aries의 표준을 분석한 문서

### [Avata DID](./AvataDID)

메타버스에서 사용하기 위한 DID의 사용 및 표준, 설계를 작성한 문서

### [CHAPI_test](./CHAPI_test)

웹 상에서 VC, VP를 사용한 검증을 진행할 수 있데 해주는 패키지인 CHAPI를 분석한 문서

### [indy-python](./indy-python)

Hyperledger Indy의 indy-sdk를 python 기반으로 구현한 테스트 코드 저장소

- Hyperledger Indy 기반의 VC, VP 발급 테스트 시퀀스를 테스트를 통해 확인할 수 있다.
- indy-sdk 기반의 클라이언트 기준 기능들이 구현되어 있으며 
- indy-sdk를 구현한 libindy의 python 기준의 코드가 작성되어 있다. 
