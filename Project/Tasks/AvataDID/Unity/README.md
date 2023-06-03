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

- indy 모듈 파일을 유니티 프로젝트 내 Library/Pythoninstall/lib 로 이동
- 유니티 실행 시 모듈은 찾아지나 다른 에러가 발생
![image](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/5e678941-a540-4385-a6e0-e7df259b4550)
- 인코딩 문제 -> 한국어 처리로 인해 발생하는가? -> 한글로 작성된 주석을 삭제
![image](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/b6e00c55-b71f-4fad-904a-94c984b1cf7a)
- 정상적인 실행 완료, 지갑 생성, DID 생성 등을 확인



- 설계 작업부터 시작
- indy-SDK 라이브러리를 통해 DID 생성 및 관리 기능을 하는 코드
```C#
using System;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.WalletApi;

public class DIDManager
{
    private const string PoolName = "my_pool";
    private const string WalletName = "my_wallet";
    private const string WalletKey = "wallet_key";

    public async void GenerateDID()
    {
        try
        {
            // 1. 풀 연결
            var poolConfig = "{\"genesis_txn\": \"path/to/genesis.txn\"}"; // 실제 genesis.txn 파일의 경로로 수정해야 함
            await Pool.CreatePoolLedgerConfigAsync(PoolName, poolConfig);

            // 2. 지갑 생성
            await Wallet.CreateWalletAsync(PoolName, WalletName, "default", null, WalletKey);

            // 3. 지갑 열기
            var wallet = await Wallet.OpenWalletAsync(WalletName, null, WalletKey);

            // 4. DID 생성
            var didJson = "{\"seed\": \"000000000000000000000000Steward1\"}"; // 원하는 시드값으로 수정 가능
            var createResult = await Did.CreateAndStoreMyDidAsync(wallet, didJson);

            var did = createResult.Did;
            var verkey = createResult.VerKey;

            Console.WriteLine("Generated DID: " + did);
            Console.WriteLine("Generated Verkey: " + verkey);

            // 5. DID 정보 조회
            var getDidResult = await Did.GetDidAsync(wallet, did);
            Console.WriteLine("Retrieved DID: " + getDidResult.Did);
            Console.WriteLine("Retrieved Verkey: " + getDidResult.VerKey);

            // 6. 지갑 닫기
            await wallet.CloseAsync();

            // 7. 지갑 삭제
            await Wallet.DeleteWalletAsync(WalletName, null);

            // 8. 풀 해제
            await Pool.DeletePoolLedgerConfigAsync(PoolName);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }
}
```
- pool : 블록체인 네트워크에서 참가자들이 공동으로 사용하는 신뢰 가능한 데이터베이스.
- 지갑 : 개인 키, 인증서, 암호화된 데이터 등을 안전하게 저장하고 관리하는 데이터 저장소. 현재는 DID를 생성하고 저장하기 위한 용도.
- 시드값 : 암호학적으로 생성된 난수, 개인키를 생성하는 데에 사용. 동일한 시드값은 동일한 개인키를 생성하며 개인 키 관리 및 복구에 사용됨.
- genesis 파일 : 블록체인 네트워크의 초기 상태와 구성을 정의하는 파일. 이 파일은 블록체인 네트워크의 모든 참가자가 동일한 원장 상태로 시작할 수 있도록 네트워크를 초기화함.
  - 원장 상태 : 블록체인 네트워크의 현재 상태를 나타내는 데이터의 집합

#

### 설계 도형

![did drawio](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/cdee987a-f3de-496a-8962-4d9b573ea0be)

- 사용자가 아바타를 생성 시 DIDManager로부터 DID를 부여받음.
- DID는 아바타가 아닌 사용자에게 존재.
- 이 DID를 통해 신원증명
- 아바타마다 고유의 DID를 할당할 수 있을지














