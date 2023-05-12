# Avata DID

Metaverse에서 사용하기 위한 Avata에 DID 적용

![20230407_metaverse_Avater_2.png](/HyperledgerAries/Avata DID/Image/20230407_metaverse_Avater_2.png)

# Avata DID 상황에 따른 설계

## 처음 실행 시 (회원 가입)

1. 외부 지갑과의 연결 또는 내부 지갑 생성
2. 지갑 내 새로운 DID 생성 및 메타버스 내에서 사용할 아바타 생성
3. 아바타와 DID를 매핑하여 서버에 저장

→ 중간에 인증 과정이 필요한가? (기존 메타버스에서 범죄를 저지른 사람을 유추)

→ 로컬에 아바타 저장, 다른 곳에서도 사용 가능

→ 아바타의 범위? → 아바타 관련 표준이 있는가?

<aside>
💡 아바타 표준의 경우 현재 VRM, Ready Player Me, VRC가 있으며 대부분 Unity를 지원한다.  Unity의 경우 엔진 내에서 사용하기 위한 모델의 표준이 작성되어 있다. 또한 Metaverse에서 사용하기 위한 아바타 표준의 경우 Metaverse-standards forum에서 협의 진행 중이다.

</aside>

<aside>
💡 VRM의 경우 파일 포맷이며 VRM 확장자로 만들어진 모델은 VRM 기능을 지원하는 모든 프로그램에 자유롭게 사용할 수 있다.

</aside>

- 관련 링크)
    
    VRM : [https://vrm.dev/en/](https://vrm.dev/en/) 
    
    Ready Player Me : [https://docs.readyplayer.me/ready-player-me/](https://docs.readyplayer.me/ready-player-me/)
    
    VRC : [https://docs.vrchat.com/docs/rig-requirements](https://docs.vrchat.com/docs/rig-requirements)
    
    Unity 메뉴얼 : [https://docs.unity3d.com/Manual/CreatingDCCAssets.html](https://docs.unity3d.com/Manual/CreatingDCCAssets.html)
    
    Metaverse-standards forum : [https://metaverse-standards.org/](https://metaverse-standards.org/)
    
- 그외 아바타 표준 관련 링크)
    
    애니메이션 관련 표준 glTF : https://github.com/KhronosGroup/glTF
    
    Web3D : [https://www.web3d.org/](https://www.web3d.org/)
    

→ 인증 시 아바타를 사용한 인증은 가능한가? → 블록체인에 DID 저장 시 아바타 정보를 같이 저장하며 이후 해당 아바타를 사용해 로그인 → 아바타 하나에 종속적이며 아바타 분실 또는 복제 시 이에 대한 대처 방법이 필요 → 블록체인에 저장할 아바타 정보?

→ 유니티에서 연결할 지갑은? → 외부 지갑을 연결하기 위한 방법? 

- 유니티 지갑 관련 링크)
    
    moralis - Unity App to a Web3 Wallet : [https://moralis.io/how-to-connect-a-unity-app-to-a-web3-wallet/](https://moralis.io/how-to-connect-a-unity-app-to-a-web3-wallet/)
    
    Unity-Solana Wallet : https://github.com/allartprotocol/unity-solana-wallet
    

## 처음 실행 이후 사용 시 (로그인)

1. 내부 저장소를 읽어 지갑 확인 또는 DID와 공개 키로 로그인

## 메타버스 내부의 인증 상황

- 메타버스 내부에서 VC 발급 시
- 메타버스 내부에서 VP 사용 시
- 외부에서 발급한 VC를 메타버스 내부에서 사용 시
- 

## 아바타의 VC 발급 과정

1. 이전 활동을 통해 발급자와의 인증 완료
2. 아바타의 DID와 VC 발급자의 DID를 사용한 연결
3. DIDComm을 사용한 통신을 통해 VC 전달

## 아바타의 VP 제시 과정

1. 아바타의 서비스 사용 요청
2. 검증자의 VP 제출 요청 (이때 검증에 필요한 정보 전달)
3. VP 제출 요청 기반으로 VP 생성 후 전달
4. 검증자는 받은 VP를 검증
5. 검증 결과에 따라 서비스 제공 또는 거부

## 예시 상황1) 메타버스 집 출입

1. 사용자가 메타버스 내의 집을 구매
    1. 사용자가 직접 집을 만들 경우?
2. 집 제공자는 사용자에게 집의 주인이라는 증명서 제공
    1. NFT를 사용한 증명서 또는 DID를 사용한 VC
    2. 집의 잠금 장치가 사용자의 DID와 공개 키를 저장
3. 사용자는 증명서를 지갑에 저장
4. 집 출입을 위해 사용자가 집에 접근
5. 사용자가 본인의 DID를 제공하여 집 주인임을 인증
    1. 집의 잠금 장치가 사용자의 DID를 통해 신원 인증 진행
    2. 집의 잠금 장치에 사용자의 DID가 등록되어 가벼운 DID Auth를 통해 인증
6. 인증이 완료되면 사용자가 집에 출입

- 추가 출처
    
    메타버스와 오픈소스 : [https://www.oss.kr/oss_guide/show/c203c6fb-c9cc-4e67-bb65-ea177244ecdd](https://www.oss.kr/oss_guide/show/c203c6fb-c9cc-4e67-bb65-ea177244ecdd)
    
    오픈 메타버스 논문 : [https://outlierventures.io/research/the-open-metaverse-os/](https://outlierventures.io/research/the-open-metaverse-os/)
    

# Unity에 DID 적용

## Unity에서 Python 사용

Unity에선 Python을 위한 기능을 제공하고 있으며 이는 패키지 매니저를 통해 다운받을 수 있다. 

<aside>
💡 자세한 정보는 링크에 있다. → [https://docs.unity3d.com/Packages/com.unity.scripting.python@7.0/manual/index.html](https://docs.unity3d.com/Packages/com.unity.scripting.python@7.0/manual/index.html)

</aside>

먼저 Unity를 실행해 프로젝트를 생성한 뒤 창을 띄운다. 다음 ‘Window → Package Manager’를 클릭해 Package Manager 창을 띄운다. 다음 ‘+’를 클릭해 ‘Add pakage by name’을 클릭하여 ‘com.unity.scripting.python’을 입력해 추가한다. 

- Unity에서 제공하는 Python 사용을 위한 패키지는 아래와 같다.

![Untitled](/HyperledgerAries/Avata DID/Image/Untitled.png)

패키지를 다운 받으면 Unity 창으로 돌아가 ‘Window → General → Python Consol’을 클릭한다. ‘Python Consol’을 클릭하면 ‘Python Script Editor’ 창이 뜨며 Python을 실행할 수 있다.

- 아래의 코드는 Python 버전을 출력하는 코드이며 아래에 코드를 작성한 뒤, Execute 버튼을 누르면 실행된다.

![20230504_유니티파이썬_1.PNG](/HyperledgerAries/Avata DID/Image/20230504_1.png)

다음은 C# 스크립트에서 Python을 실행하는 방법이다. 먼저 테스트를 위한 스크립트를 작성한다. 

- Assets/_Script/PythonTest.cs

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;

public class PythonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            PrintHelloWorldFromPython();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            IndyFromPython();
        }
    }

    static void PrintHelloWorldFromPython()
    {
        PythonRunner.RunString(@"
                import UnityEngine;
                UnityEngine.Debug.Log('hello world')
                ");
    }

    static void IndyFromPython()
    {
        UnityEngine.Debug.Log($"{Application.dataPath}");
        PythonRunner.RunFile($"{Application.dataPath}/_PythonScript/pythonTest.py");
    }
}
```

- 위 코드는 Python이 제대로 동작하는지 확인하기 위해 작성한 코드로 테스트 용이다.
- Update : 사용자가 W키 또는 K키를 누르면 Python 코드가 실행되도록 작성했다.
- PrintHelloWorldFromPython : C# 코드 내에서 Python 코드를 작성하는 방법으로 PythonRunner.RunString에 실행할 Python 코드를 작성해 실행시킨다.
- IndyFromPython : C# 코드 밖에서 작성한 Python 코드를 실행하는 방법으로 PythonRunner.RunFile에 실행할 Python 코드 파일 경로를 작성해 실행시킨다.

<aside>
💡 Application.dataPath는 Unity 프로젝트 내부의 Asset 파일 경로이다.

</aside>

- Assets/_PythonScript/pythonTest.py

```python
import UnityEngine

print("python.print")
UnityEngine.Debug.Log('Unity.Log')
```

- Unity Engine 패키지는 유니티에서 제공해주는 패키지로 이를 통해 Unity 내의 게임 오브젝트 정보나 기능을 Python을 통해 제어할 수 있다.
- 위 코드의 경우 둘 다 문자열을 출력하지만 Unity에서 출력을 원할 경우 UnityEngine의 Debug를 사용해야 출력을 할 수 있다.
- 출처
    
    unity에서 python 사용 :  [https://docs.unity3d.com/Packages/com.unity.scripting.python@2.0/manual/PythonScriptEditor.html](https://docs.unity3d.com/Packages/com.unity.scripting.python@2.0/manual/PythonScriptEditor.html)
    
    [출처] Unity3D 유니티에서 파이썬파일 실행시키기(Run .py in Unity)|작성자 kanrhaehfdl1 : [https://blog.naver.com/PostView.nhn?blogId=kanrhaehfdl1&logNo=221675044575&parentCategoryNo=&categoryNo=10&viewDate=&isShowPopularPosts=true&from=search](https://blog.naver.com/PostView.nhn?blogId=kanrhaehfdl1&logNo=221675044575&parentCategoryNo=&categoryNo=10&viewDate=&isShowPopularPosts=true&from=search)
    
    unity python script 6.0 : [https://docs.unity3d.com/Packages/com.unity.scripting.python@6.0/manual/installation.html](https://docs.unity3d.com/Packages/com.unity.scripting.python@6.0/manual/installation.html)
    

## Unity에서 indy-sdk 사용

위와 같은 방법으로 Unity에서 Python을 사용할 수 있다. 아래는 indy-sdk가 Unity에서 정상적으로 작동하는지 테스트하기 위한 코드이며 지갑 생성 및 열기, 삭제를 진행한다.

- Assets\_PythonScript\pythonTest.py

```python
import sys, os
import asyncio
import json
import pprint
from pathlib import Path

from indy import pool, ledger, wallet, did, anoncreds
from indy.error import ErrorCode, IndyError

import UnityEngine

os.add_dll_directory("D:\libindy_1.16.0\lib")

pool_name = 'pool'
issuer_wallet_config = json.dumps({"id": "issuer_wallet"})
issuer_wallet_credentials = json.dumps({"key": "issuer_wallet_key"})
genesis_file_path = Path.home().joinpath("pool_transactions_genesis")

async def proof_negotiation():
    try:
        await pool.set_protocol_version(2)
        UnityEngine.Debug.Log("proof_negotiation")
        UnityEngine.Debug.Log(genesis_file_path)
        # 1.
        UnityEngine.Debug.Log('\n1. opening a new local pool ledger configuration that will be used '
                  'later when connecting to ledger.\n')
        pool_config = json.dumps({'genesis_txn': str(genesis_file_path)})
        try:
            await pool.create_pool_ledger_config(config_name=pool_name, config=pool_config)
        except IndyError as ex:
            if ex.error_code == ErrorCode.PoolLedgerConfigAlreadyExistsError:
                pass

        UnityEngine.Debug.Log(pool_config)

        # 2.
        UnityEngine.Debug.Log('\n2. Open pool ledger and get the handle from libindy\n')
        pool_handle = await pool.open_pool_ledger(config_name=pool_name, config=None)

        # 3.
        UnityEngine.Debug.Log('\n3. Creating Issuer wallet and opening it to get the handle.\n')
        try:
            await wallet.create_wallet(issuer_wallet_config, issuer_wallet_credentials)
        except IndyError as ex:
            if ex.error_code == ErrorCode.WalletAlreadyExistsError:
                pass

        issuer_wallet_handle = await wallet.open_wallet(issuer_wallet_config, issuer_wallet_credentials)

        # 4.
        UnityEngine.Debug.Log('\n4. Generating and storing steward DID and verkey\n')
        steward_seed = '000000000000000000000000Steward1'
        did_json = json.dumps({'seed': steward_seed})
        steward_did, steward_verkey = await did.create_and_store_my_did(issuer_wallet_handle, did_json)

        # 5.
        UnityEngine.Debug.Log('\n5. Generating and storing trust anchor DID and verkey\n')
        trust_anchor_did, trust_anchor_verkey = await did.create_and_store_my_did(issuer_wallet_handle, "{}")

        # 6.
        UnityEngine.Debug.Log('\n6. Building NYM request to add Trust Anchor to the ledger\n')
        nym_transaction_request = await ledger.build_nym_request(submitter_did=steward_did,
                                                                 target_did=trust_anchor_did,
                                                                 ver_key=trust_anchor_verkey,
                                                                 alias=None,
                                                                 role='TRUST_ANCHOR')
        UnityEngine.Debug.Log('NYM transaction request: ')
        UnityEngine.Debug.Log(json.loads(nym_transaction_request))

        # 7.
        UnityEngine.Debug.Log('\n7. Sending NYM request to the ledger\n')
        nym_transaction_response = await ledger.sign_and_submit_request(pool_handle=pool_handle,
                                                                        wallet_handle=issuer_wallet_handle,
                                                                        submitter_did=steward_did,
                                                                        request_json=nym_transaction_request)
        UnityEngine.Debug.Log('NYM transaction response: ')
        UnityEngine.Debug.Log(json.loads(nym_transaction_response))

        # 8.
        UnityEngine.Debug.Log('\n8. Issuer create Credential Schema\n')
        schema = {
            'name': 'gvt',
            'version': '1.0',
            'attributes': '["age", "sex", "height", "name"]'
        }
        issuer_schema_id, issuer_schema_json = await anoncreds.issuer_create_schema(steward_did, 
                                                                                schema['name'],
                                                                                schema['version'],
                                                                                schema['attributes'])
        UnityEngine.Debug.Log('Schema: ')
        UnityEngine.Debug.Log(issuer_schema_json)

        # 9.
        UnityEngine.Debug.Log('\n9. Build the SCHEMA request to add new schema to the ledger\n')
        schema_request = await ledger.build_schema_request(steward_did, issuer_schema_json)
        UnityEngine.Debug.Log('Schema request: ')
        UnityEngine.Debug.Log(json.loads(schema_request))

        # 10.
        UnityEngine.Debug.Log('\n10. Sending the SCHEMA request to the ledger\n')
        schema_response = \
            await ledger.sign_and_submit_request(pool_handle,
                                                 issuer_wallet_handle,
                                                 steward_did,
                                                 schema_request)
        UnityEngine.Debug.Log('Schema response:')
        UnityEngine.Debug.Log(json.loads(schema_response))

        # 11.
        UnityEngine.Debug.Log('\n11. Creating and storing Credential Definition using anoncreds as Trust Anchor, for the given Schema\n')
        cred_def_tag = 'TAG1'
        cred_def_type = 'CL'
        cred_def_config = json.dumps({"support_revocation": False})

        (cred_def_id, cred_def_json) = \
            await anoncreds.issuer_create_and_store_credential_def(issuer_wallet_handle,
                                                                   trust_anchor_did,
                                                                   issuer_schema_json,
                                                                   cred_def_tag,
                                                                   cred_def_type,
                                                                   cred_def_config)
        
        UnityEngine.Debug.Log('Credential definition: ')
        UnityEngine.Debug.Log(json.loads(cred_def_json))

        # 12.
        UnityEngine.Debug.Log('\n12. Creating Prover wallet and opening it to get the handle.\n')
        prover_did = 'VsKV7grR1BUE29mG2Fm2kX'
        prover_wallet_config = json.dumps({"id": "prover_wallet"})
        prover_wallet_credentials = json.dumps({"key": "prover_wallet_key"})

        try:
            await wallet.create_wallet(prover_wallet_config, prover_wallet_credentials)
        except IndyError as ex:
            if ex.error_code == ErrorCode.WalletAlreadyExistsError:
                pass

        prover_wallet_handle = await wallet.open_wallet(prover_wallet_config, prover_wallet_credentials)

        # 13.
        UnityEngine.Debug.Log('\n13. Prover is creating Link Secret\n')
        prover_link_secret_name = 'link_secret'
        link_secret_id = await anoncreds.prover_create_master_secret(prover_wallet_handle,
                                                                     prover_link_secret_name)

        # 14.
        UnityEngine.Debug.Log('\n14. Issuer (Trust Anchor) is creating a Credential Offer for Prover\n')
        cred_offer_json = await anoncreds.issuer_create_credential_offer(issuer_wallet_handle,
                                                                         cred_def_id)
        UnityEngine.Debug.Log('Credential Offer: ')
        UnityEngine.Debug.Log(json.loads(cred_offer_json))

        # 15.
        UnityEngine.Debug.Log('\n15. Prover creates Credential Request for the given credential offer\n')
        (cred_req_json, cred_req_metadata_json) = \
            await anoncreds.prover_create_credential_req(prover_wallet_handle,
                                                         prover_did,
                                                         cred_offer_json,
                                                         cred_def_json,
                                                         prover_link_secret_name)
        UnityEngine.Debug.Log('Credential Request: ')
        UnityEngine.Debug.Log(json.loads(cred_req_json))

        # 16.
        UnityEngine.Debug.Log('\n16. Issuer (Trust Anchor) creates Credential for Credential Request\n')
        cred_values_json = json.dumps({
            "sex": {"raw": "male", "encoded": "5944657099558967239210949258394887428692050081607692519917050011144233"},
            "name": {"raw": "Alex", "encoded": "1139481716457488690172217916278103335"},
            "height": {"raw": "175", "encoded": "175"},
            "age": {"raw": "28", "encoded": "28"}
        })
        (cred_json, _, _) = \
            await anoncreds.issuer_create_credential(issuer_wallet_handle,
                                                     cred_offer_json,
                                                     cred_req_json,
                                                     cred_values_json, None, None)
        UnityEngine.Debug.Log('Credential: ')
        UnityEngine.Debug.Log(json.loads(cred_json))

        # 17.
        UnityEngine.Debug.Log('\n17. Prover processes and stores received Credential\n')
        await anoncreds.prover_store_credential(prover_wallet_handle, None,
                                                cred_req_metadata_json,
                                                cred_json,
                                                cred_def_json, None)

        # 18.
        UnityEngine.Debug.Log('\n18. Prover gets Credentials for Proof Request\n')
        proof_request = {
            'nonce': '123432421212',
            'name': 'proof_req_1',
            'version': '0.1',
            'requested_attributes': {
                'attr1_referent': {
                    'name': 'name',
                    "restrictions": {
                        "issuer_did": trust_anchor_did,
                        "schema_id": issuer_schema_id
                    }
                }
            },
            'requested_predicates': {
                'predicate1_referent': {
                    'name': 'age',
                    'p_type': '>=',
                    'p_value': 18,
                    "restrictions": {
                       "issuer_did": trust_anchor_did
                    }
                }
            }
        }
        UnityEngine.Debug.Log('Proof Request: ')
        UnityEngine.Debug.Log(proof_request)

        # 19. 
        UnityEngine.Debug.Log('\n19. Prover gets Credentials for attr1_referent anf predicate1_referent\n')
        proof_req_json = json.dumps(proof_request)
        prover_cred_search_handle = \
            await anoncreds.prover_search_credentials_for_proof_req(prover_wallet_handle, proof_req_json, None)

        creds_for_attr1 = await anoncreds.prover_fetch_credentials_for_proof_req(prover_cred_search_handle,
                                                                                 'attr1_referent', 1)
        prover_cred_for_attr1 = json.loads(creds_for_attr1)[0]['cred_info']
        UnityEngine.Debug.Log('Prover credential for attr1_referent: ')
        UnityEngine.Debug.Log(prover_cred_for_attr1)

        creds_for_predicate1 = await anoncreds.prover_fetch_credentials_for_proof_req(prover_cred_search_handle,
                                                                                      'predicate1_referent', 1)
        prover_cred_for_predicate1 = json.loads(creds_for_predicate1)[0]['cred_info']
        UnityEngine.Debug.Log('Prover credential for predicate1_referent: ')
        UnityEngine.Debug.Log(prover_cred_for_predicate1)

        await anoncreds.prover_close_credentials_search_for_proof_req(prover_cred_search_handle)
        
        # 20.
        UnityEngine.Debug.Log('\n20. Prover creates Proof for Proof Request\n')
        prover_requested_creds = json.dumps({
            'self_attested_attributes': {},
            'requested_attributes': {
                'attr1_referent': {
                    'cred_id': prover_cred_for_attr1['referent'],
                    'revealed': True
                }
            },
            'requested_predicates': {
                'predicate1_referent': {
                    'cred_id': prover_cred_for_predicate1['referent']
                }
            }
        })
        UnityEngine.Debug.Log('Requested Credentials for Proving: ')
        UnityEngine.Debug.Log(json.loads(prover_requested_creds))

        prover_schema_id = json.loads(cred_offer_json)['schema_id']
        schemas_json = json.dumps({prover_schema_id: json.loads(issuer_schema_json)})
        cred_defs_json = json.dumps({cred_def_id: json.loads(cred_def_json)})
        proof_json = await anoncreds.prover_create_proof(prover_wallet_handle,
                                                         proof_req_json,
                                                         prover_requested_creds,
                                                         link_secret_id,
                                                         schemas_json,
                                                         cred_defs_json,
                                                         "{}")
        proof = json.loads(proof_json)
        UnityEngine.Debug.Log('\nproof : ')
        UnityEngine.Debug.Log(proof)
        assert 'Alex' == proof['requested_proof']['revealed_attrs']['attr1_referent']["raw"]

        # 21.
        UnityEngine.Debug.Log('\n21. Verifier is verifying proof from Prover\n')
        assert await anoncreds.verifier_verify_proof(proof_req_json,
                                                             proof_json,
                                                             schemas_json,
                                                             cred_defs_json,
                                                             "{}", "{}")

        # 22.
        UnityEngine.Debug.Log('\n22. Closing both wallet_handles and pool\n')
        await wallet.close_wallet(issuer_wallet_handle)
        await wallet.close_wallet(prover_wallet_handle)
        await pool.close_pool_ledger(pool_handle)

        # 23.
        UnityEngine.Debug.Log('\n23. Deleting created wallet_handles\n')
        await wallet.delete_wallet(issuer_wallet_config, issuer_wallet_credentials)
        await wallet.delete_wallet(prover_wallet_config, prover_wallet_credentials)

        # 24.
        UnityEngine.Debug.Log('\n24. Deleting pool ledger config\n')
        await pool.delete_pool_ledger_config(pool_name)

    except IndyError as e:
        UnityEngine.Debug.Log('Error occurred: %s' % e)

def main():
    #os.add_dll_directory("D:\libindy_1.16.0\lib")
    UnityEngine.Debug.Log("START!!!")
    loop = asyncio.get_event_loop()
    loop.run_until_complete(proof_negotiation())
    loop.close()

py_ver = int(f"{sys.version_info.major}{sys.version_info.minor}")
if py_ver > 37 and sys.platform.startswith('win'):
    asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())

main()
```

<aside>
💡 Python에서 indy-sdk를 사용하기 위해선 외부 라이브러리인 libindy를 가져올 수 있어야 한다. 이를 위해 위 코드에선 ‘os.add_dll_directory("D:\libindy_1.16.0\lib")’ 부분을 추가했다. ‘add_dll_directory’는 window 내부의 공유 라이브러리를 읽기 위한 함수이며 Ubuntu에선 동작하지 않는다. 또한 ‘add_dll_directory’의 입력 값은 사용자가 libindy를 다운로드한 뒤 해당 경로를 입력해 주면 정상적으로 동작한다.

</aside>

위 코드 실행 이후 로그가 출력되며 사용자 파일에 ‘.indy_client’ 파일이 추가된다.