# Hyperledger Aries 분석

Hyperledger Aries는 블록체인 기반 데이터 전송 및 P2P 메시징 기능 지원, 서로 다른 블록체인 및 분산 원장 간의 상호 운용성을 위해 만들어진 도구 및 기술이다.

Hyperledger Aries의 목표는 다음과 같다.

- P2P 상호작용, 분산 시스템을 위한 보안 메시징 코드 제공
- 표준화 작업으로 실용적인 상호 운용성을 촉진하고 단일 비즈니스 솔루션으로 확장

Hyperledger Aries는 다음을 포함한다.

- 블록체인 트랜잭션 생성 및 서명을 위한 인터페이스 계층
- 블록체인 클라이언트를 구현하는데 사용되는 암호화 비밀 및 기타 정보의 안전한 저장을 위한 암호화 지갑
- 여러 전송 프로토콜을 사용하는 클라이언트 간의 상호 작용을 위한 암호화 메시징 시스템
- Hyperledger Ursa의 ZKP 프리미티브를 사용한 ZKP 지원 W3C 검증 가능한 자격 증명(Verifiable Credentials) 구현
- Hyperledger Indy에서 진행 중인 분산형 키 관리 시스템(DKMS) 사양 구현
- 보안 메시지 기능을 기반으로 상위 수준 프로토콜 및 API와 유사한 사용 사례를 구축

![Untitled](Image/Untitled.png)

Hyperledger Aries는 통신을 위한 프로토콜 표준화를 진행하고 있으며 Hyperledger Aries RFC에 표준을 정리하고 있다. 

[GitHub - hyperledger/aries-rfcs: Hyperledger Aries is infrastructure for blockchain-rooted, peer-to-peer interactions](https://github.com/hyperledger/aries-rfcs/tree/main)

## Aries Interop Profile

Aries는 Aries Interop Profile을 정의해 버전을 관리한다. 이는 각각의 Aries 구현 코드들이 상호작용을 가지기 위해 정의하며 같은 버전의 AIP를 가진 코드들은 같은 세트의 기능을 구현하여 상호작용성을 확보해야 한다. AIP는 discover_features 프로토콜을 사용해 공개할 수 있다.

- Discover Features Message EX)
    
    ```json
    {
      "@type": "https://didcomm.org/discover-features/2.0/disclosures",
      "disclosures": [
        {
          "feature-type": "aip",
          "id": "AIP2.0",
        },
        {
          "feature-type": "aip",
          "id": "AIP2.0/INDYCRED"
        }
      ]
    }
    ```
    

[aries-rfcs/README.md at main · hyperledger/aries-rfcs](https://github.com/hyperledger/aries-rfcs/blob/main/concepts/0302-aries-interop-profile/README.md)

현재 AIP는 AIP 1.0과 AIP 2.0이 있으며 AIP 2.0은 현재 추가 중에 있다.

- AIP 1.0
    - Connection Protocol
    - Issue Credential Protocol 1.0
    - Present Proof Protocol 1.0

- AIP 2.0
    - DID Exchange Protocol 1.0
    - Issue Credential Protocol 2.0
    - Present Proof Protocol 2.0

[ACA-PY 코드 분석](https://www.notion.so/ACA-PY-b9109d90d79643bcafc28473ff17684f) 

Aries 저장소 : [https://github.com/hyperledger/aries](https://github.com/hyperledger/aries)

Aries 소개 : [https://www.hyperledger.org/blog/2019/05/14/announcing-hyperledger-aries-infrastructure-supporting-interoperable-identity-solutions](https://www.hyperledger.org/blog/2019/05/14/announcing-hyperledger-aries-infrastructure-supporting-interoperable-identity-solutions)

Aries 제안서 : [https://wiki.hyperledger.org/display/TSC/Hyperledger+Aries+Proposal](https://wiki.hyperledger.org/display/TSC/Hyperledger+Aries+Proposal)

Aries 주요 문서 : [https://github.com/hyperledger/aries-rfcs/tree/main/concepts](https://github.com/hyperledger/aries-rfcs/tree/main/concepts)

DIDComm 표준 사이트 : [https://didcomm.org/search/?page=1&q=](https://didcomm.org/search/?page=1&q=)

## Hyperledger Aries - Connection Protocol

Aries는 에이전트간 연결을 통해 보안 정보를 전달할 수 있는 에이전트를 요구하며 이를 위한 연결 프로토콜이 필요하다.

연결은 다음과 같은 정보를 요구한다.

- 추천 라벨
- 공개적으로 사용 가능한 DID (해당 정보가 있을 시 수신자 키와 서비스 엔드포인트 불필요)
- 수신자키
- 서비스 엔드포인트
- RoutingKeys (선택 사항)

초대자는 위와 같은 정보를 통해 초대장을 만들며 이를 초대받는 사람에게 전달한다.

초대장 URL 형태로 이메일, SMS, 웹사이트 게시, QR 코드 등 텍스트를 보낼 수 있는 모든 방법을 통해 전송할 수 있다. (Aries에선 Out-of-band Protocol을 추천한다.)

초대 받은 사람은 초대장을 받은 뒤 연결을 위해 본인의 DID 문서를 초대자에게 전달할 필요가 있다. 초대 받은 사람은 연결 요청 메시지를 만들어 이를 초대자에게 전달한다.

Connection은 다른 Aries 사용자와의 통신 채널을 만드는 Protocol이며 안전한 P2P 통신이 가능하다. Connection은 Inviter와 Invitee가 있으며 Inviter의 Invitation 전달로 시작한다. Invitation은 Inviter와 통신을 위한 정보들이 기록되어 있으며 통신을 위한 공개키, 앤드 포인트, DID가 담겨있다. Inviter는 Invitee에게 Invitation을 전달하며 이때 Inviter와 Invitee 사이의 명확한 연결이 없으므로 Inviter는 URL, QR코드와 같은 방법으로 Invitation을 전달한다. Invitee는 초대에 응할 시 ‘connection request’를 만들어 Inviter에게 보낸다. ‘connection request’에는 Inviter의 Invitation처럼 Invitee와 통신을 위한 정보를 가지고 있다. 이후 Inviter가 ‘connection response’으로 응답을 보낸 뒤, ‘ack’ 응답이 오면 Connection이 끝난다. 양쪽 사용자는 Invitation을 통해 서로에게 메시지를 전달할 앤드 포인트와 암호화를 위한 공개키, DID 정보를 알고 있으므로 P2P 통신이 가능하다. 이후 Verifiable Credential(이하 VC) 발급이나 Verifiable Presentation(이하 VP) 전달 등 통신이 필요할 때 기존의 Connection을 사용해 전달한다.

- Connection Protocol 상태머신

![Untitled](Hyperledger%20Aries%20%E1%84%87%E1%85%AE%E1%86%AB%E1%84%89%E1%85%A5%E1%86%A8%2010dfa3de368e43b2aff2001cf5cef71e/Untitled%201.png)

- Connection Protocol

![Untitled](Hyperledger%20Aries%20%E1%84%87%E1%85%AE%E1%86%AB%E1%84%89%E1%85%A5%E1%86%A8%2010dfa3de368e43b2aff2001cf5cef71e/Untitled%202.png)

1. Invitation
    
    Inviter가 Invitee에게 전달하는 메시지
    
    - Invitation Message Type
        
        ```json
        {
            "@type": "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/connections/1.0/invitation",
            "@id": "8f6bc703-3820-4078-bf62-f2a54b3380f3",
            "label": "Issuer",
            "serviceEndpoint": "http://220.68.5.140:8000",
            "recipientKeys": ["BibjG8NxDtC8oTyK4VwLRBy5Z3m6A165DfiU4qDCQzKn"]
        }
        ```
        
        - @type : 메시지 정보
        - @id : 메시지 식별 값
        - label : [옵션] 사용자가 메시지에 붙이는 이름표, 해당 이름표를 통해 빠르게 조회할 수 있다.
        - serviceEndpoint : 서비스 끝점, 통신을 위해 전달하며 상대방은 해당 정보를 통해 통신을 요청한다.
        - recipientKeys : Inviter의 공개키로 Invitee는 해당 공개키를 통해 암호화하여 메시지를 전달한다.
        
    
2. Connection Request
    
    Invitation을 받은 Invitee가 Inviter에게 보내는 연결 요청 메시지
    
    - Connection Request Message Type
        
        ```json
        {
          "@id": "5678876542345",
          "@type": "https://didcomm.org/connections/1.0/request",
          "label": "Bob",
          "connection": {
            "DID": "B.did@B:A",
            "DIDDoc": {
                "@context": "https://w3id.org/did/v1"
                // DID document contents here.
            }
          }
        }
        ```
        
        - connection : 연결 정보
            - DID : Invitee의 DID
            - DIDDoc : [옵션] 위 DID에 대한 DIDDoc 정보, DID를 통해 원장에서 DIDDoc를 확인할 수 있는 경우 생략할 수 있다.
        - DID Doc 예제 → [https://github.com/hyperledger/aries-rfcs/blob/main/features/0067-didcomm-diddoc-conventions/README.md](https://github.com/hyperledger/aries-rfcs/blob/main/features/0067-didcomm-diddoc-conventions/README.md)
            
            ```json
            {
              "@context": "https://w3id.org/did/v1",
              "id": "did:sov:QUmsj7xwB82QAuuzfmvhAi",
              "publicKey": [
                {
                  "id": "did:sov:QUmsj7xwB82QAuuzfmvhAi#1",
                  "type": "Ed25519VerificationKey2018",
                  "controller": "did:sov:QUmsj7xwB82QAuuzfmvhAi",
                  "publicKeyBase58": "DoDMNYwMrSN8ygGKabgz5fLA9aWV4Vi8SLX6CiyN2H4a"
                }
              ],
              "authentication": [
                {
                  "type": "Ed25519SignatureAuthentication2018",
                  "publicKey": "did:sov:QUmsj7xwB82QAuuzfmvhAi#1"
                }
              ],
              "service": [
                {
                  "id": "did:sov:QUmsj7xwB82QAuuzfmvhAi;indy",
                  "type": "IndyAgent",
                  "priority": 0,
                  "recipientKeys": [
                    "DoDMNYwMrSN8ygGKabgz5fLA9aWV4Vi8SLX6CiyN2H4a"
                  ],
                  "serviceEndpoint": "http://192.168.65.3:8030"
                }
              ]
            }
            ```
            
        
    
3. Connection Response
    
    Connection Request를 받은 Inviter가 Invitee에게 보내는 응답 메시지
    
    - Connection Response Message Type
        
        ```json
        {
          "@type": "https://didcomm.org/connections/1.0/response",
          "@id": "12345678900987654321",
          "~thread": {
            "thid": "<@id of request message>"
          },
          "connection": {
            "DID": "A.did@B:A",
            "DIDDoc": {
              "@context": "https://w3id.org/did/v1"
              // DID document contents here.
            }
          }
        }
        ```
        
        - ~thread : [옵션] 요청 메시지에 대한 참조, 추가 형식에서 확인
        - connection : 연결 정보
            - DID : Invitee의 DID
            - DIDDoc : [옵션] 위 DID에 대한 DIDDoc 정보, DID를 통해 원장에서 DIDDoc를 확인할 수 있는 경우 생략할 수 있다.
        
        - 위 메시지는 서명이 필요하며 서명은 아래와 같다. → [https://github.com/hyperledger/aries-rfcs/blob/main/features/0234-signature-decorator/README.md](https://github.com/hyperledger/aries-rfcs/blob/main/features/0234-signature-decorator/README.md)
            
            ```json
            {
              "@type": "https://didcomm.org/connections/1.0/response",
              "@id": "12345678900987654321",
              "~thread": {
                "thid": "<@id of request message>"
              },
              "connection~sig": {
                "@type": "https://didcomm.org/signature/1.0/ed25519Sha512_single",
                "signature": "<digital signature function output>",
                "sig_data": "<base64URL(64bit_integer_from_unix_epoch||connection_attribute)>",
                "signer": "<signing_verkey>"
              }
            }
            ```
            
    
- 추가 형식
    - Threaded Messages
        
        ```json
        {
            "@type": "did:example:12345...;spec/example_family/1.0/example_type",
            "@id": "98fd8d72-80f6-4419-abc2-c65ea39d0f38",
            "~thread": {
                "thid": "98fd8d72-80f6-4419-abc2-c65ea39d0f38",
                "pthid": "1e513ad4-48c9-444e-9e7e-5b8b45c5e325",
                "sender_order": 3,
                "received_orders": {"did:sov:abcxyz":1},
                "goal_code": "aries.vc.issue"
            },
            "example_attribute": "example_value"
        }
        ```
        
        - thid : 메시지의 ID
        - pthid : [옵션] 기존 메시지에 이어 보내거나 중첩이 필요할 때 사용
        - ~thread : 스레드 정보
            - sender_order : 현재 메시지의 순서를 알려주며 해당 스레드에 기여한 모든 메시지 중 맞는 위치를 알려준다.
            - received_orders : 보낸 사람이 스래드의 다른 보낸 사람에게서 가장 높은 sender_order 값을 전달한다.
        
        thread 정보 : [https://github.com/hyperledger/aries-rfcs/blob/main/concepts/0008-message-id-and-threading/README.md#thread-object](https://github.com/hyperledger/aries-rfcs/blob/main/concepts/0008-message-id-and-threading/README.md#thread-object)
        

- ACA-PY의 Connection
    
    ![Untitled](Image/Untitled%203.png)
    

Hyperledger Aries Connection Protocol :

[https://github.com/hyperledger/aries-rfcs/tree/main/features/0160-connection-protocol](https://github.com/hyperledger/aries-rfcs/tree/main/features/0160-connection-protocol)

## Hyperledger Aries - Issue Credential Protocol 2.0

자격 증명 발급에 필요한 메시지를 형식화 한다. 이는 자격 증명 종류에 영향을 받지 않으며 일정한 프로토콜을 제공한다. 

### Verifiable Credential 이란?

Verifiable Credential 이란 검증 가능한 자격 증명으로 인터넷 상에서 본인을 증명할 때 사용하는 JSON 형식의 문서이다. 

VC는 크게 3가지의 정보를 가지고 있다. 발급 기관 및 자격 증명에 대한 정보가 담긴 ‘Credential Metadata’, VC의 주체에 대한 정보가 담긴 ‘Claim’, VC의 증명에 대한 정보가 담긴 ‘Proof’가 있다. VC는 ‘Proof’에 있는 서명을 통해 발급 기관의 증명과 데이터의 무결성을 검증한다. ‘Claim’은 발급자를 식별할 수 있는 정보를 가지고 있으며 ‘Property’와 ‘Value’로 나눈다. ‘Property’는 ‘Value’가 가진 의미를 설명하며 ‘Value’는 ‘Property’와 매칭되는 발급자의 정보를 담고 있다.

- W3C에서 제시한 Verifiable Credential 예시
    
    ```json
    {
      "@context": [
        "https://www.w3.org/2018/credentials/v1",
        "https://www.w3.org/2018/credentials/examples/v1"
      ],
      "id": "http://example.edu/credentials/1872",
      "type": ["VerifiableCredential", "AlumniCredential"],
      "issuer": "https://example.edu/issuers/565049",
      "issuanceDate": "2010-01-01T19:23:24Z",
      "credentialSubject": {
        "id": "did:example:ebfeb1f712ebc6f1c276e12ec21",
        "alumniOf": {
          "id": "did:example:c276e12ec21ebfeb1f712ebc6f1",
          "name": [{
            "value": "Example University",
            "lang": "en"
          }, {
            "value": "Exemple d'Université",
            "lang": "fr"
          }]
        }
      },
      "proof": {
        "type": "RsaSignature2018",
        "created": "2017-06-18T21:19:10Z",
        "proofPurpose": "assertionMethod",
        "verificationMethod": "https://example.edu/issuers/565049#key-1",
        "jws": "eyJhbGciOiJSUzI1NiIsImI2NCI6ZmFsc2UsImNyaXQiOlsiYjY0Il19..TCYt5X
          sITJX1CxPCT8yAV-TVkIEq_PbChOMqsLfRoPsnsgw5WEuts01mq-pQy7UJiN5mgRxD-WUc
          X16dUEMGlv50aqzpqh4Qktb3rk-BuQy72IFLOqV0G_zS245-kronKb78cPN25DGlcTwLtj
          PAYuNzVBAh4vGHSrQyHUdBBPM"
      }
    }
    ```
    
    - @context : JSON 문서에 대부분 들어가는 속성으로 현재 JSON이 어떠한 속성 및 규칙들을 가지는지 설명하는 부분이다.
    - id : 식별값
    - type : 해당 Credential이 어떤 Credential 인지 설명한다. VC 생태계에선 VC와 VP를 구별할 때 해당 속성을 사용한다.
    - issuer : Issuer의 정보가 들어가는 곳으로 보통 URL이 들어가며 해당 URL은 Issuer를 설명하는 문서와 연결되어야 한다. 추가 정보를 작성할 수 있다.
    - issuanceDate : 발행 날짜
    - credentialSubject : VC 주체에 대한 정보가 들어간다.
        - id : VC 주체에 대한 정보가 들어가며 DID 정보가 들어간다.
        - 해당 부분의 경우 VC 어떤 VC에 따라 속성 값들이 달라지며 해당 값은 context가 설명한다. 위 예시의 경우 대학 정보를 표시한다.
    - proof : VC 증명을 위한 정보
        - type : 증
        - created : 생성 일자
        - proofPurpose :
        - verificationMethod : 증명에 사용할 방법 (공개 키)
        - jws : 서명 값

- Issue Credential Protocol 2.0 진행도

![Untitled](Image/Untitled%204.png)

![Untitled](Image/Untitled%205.png)

1. Propose Credential
    
    Holder가 Issuer에게 보내는 VC 제안 메시지 (옵션)
    
    - Propose Credential Message Type
        
        ```json
        {
            "@type": "https://didcomm.org/issue-credential/%VER/propose-credential",
            "@id": "<uuid of propose-message>",
            "goal_code": "<goal-code>",
            "comment": "<some comment>",
            "credential_preview": <json-ld object>,
            "formats" : [
                {
                    "attach_id" : "<attach@id value>",
                    "format" : "<format-and-version>"
                }
            ],
            "filters~attach": [
                {
                    "@id": "<attachment identifier>",
                    "mime-type": "application/json",
                    "data": {
                        "base64": "<bytes for base64>"
                    }
                }
            ],
            "supplements": [
                {
                    "type": "hashlink-data",
                    "ref": "<attachment identifier>",
                    "attrs": [{
                        "key": "field",
                        "value": "<fieldname>"
                    }]
                },
                {
                    "type": "issuer-credential",
                    "ref": "<attachment identifier>",
                }
            ],
            "~attach" : [] //attachments referred to in supplements
        }
        ```
        
        - @type : 메시지 정보
        - @id : 메시지 식별 값
        - goal_coad : [옵션] 메시지 발신자의 목표, 추가 형식의 goal_code 확인
        - comment : [옵션] 추가 메모 또는 메시지
        - credential_preview : [옵션] Holder가 증명하려는 정보가 담긴 JSON-LD 정보, Credential Preview 정보랑 같다. (아래 Preview Credential에 정리)
        - formats : filters~attach 값과 @id, 검증 가능한 자격 증명 형식 및 버전을 제공한다.
        - filters~attach : 제안되는 자격 증명을 추가로 정의하는 첨부 파일 정보
        - supplements : [옵션] 자격 증명에 대한 보충 내용을 위해 작성
        - ~attach : [옵션] 자격 증명과 관련된 선택적 첨부 파일, 해당 내용은 supplements에서 정의되어야 한다.
        
        Propose 첨부 파일 형식은 아래 표를 따른다.
        
        | Credential Format | Format Value | Link to Attachment Format |
        | --- | --- | --- |
        | DIF Credential Manifest | dif/credential-manifest@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#propose-credential-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#propose-credential-attachment-format |
        | Linked Data Proof VC Detail | aries/ld-proof-vc-detail@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-format |
        | Hyperledger Indy Credential Filter | hlindy/cred-filter@v2.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-filter-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-filter-format |
        | Hyperledger AnonCreds Credential Filter | anoncreds/credential-filter@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-filter-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-filter-format |
    
2. Offer Credential
    
    Issuer가 Holder에게 보내는 VC 제안 메시지 (옵션)
    
    - Offer Credential Message Type
        
        ```json
        {
            "@type": "https://didcomm.org/issue-credential/%VER/offer-credential",
            "@id": "<uuid of offer message>",
            "goal_code": "<goal-code>",
            "replacement_id": "<issuer unique id>",
            "comment": "<some comment>",
            "multiple_available": "<count>",
            "credential_preview": <json-ld object>,
            "formats" : [
                {
                    "attach_id" : "<attach@id value>",
                    "format" : "<format-and-version>",
                }
            ],
            "offers~attach": [
                {
                    "@id": "<attach@id value>",
                    "mime-type": "application/json",
                    "data": {
                        "base64": "<bytes for base64>"
                    }
                }
            ],
            "supplements": [
                {
                    "type": "hashlink-data",
                    "ref": "<attachment identifier>",
                    "attrs": [{
                        "key": "field",
                        "value": "<fieldname>"
                    }]
                },
                {
                    "type": "issuer-credential",
                    "ref": "<attachment identifier>",
                }
            ],
            "~attach" : [] //attachments referred to in supplements
        }
        ```
        
        - replacement_id : [옵션] 자격 증명 교체를 위한 정보, 이전에 발급한 자격 증명과 겹칠 경우 이전의 자격 증명을 대체함을 알리기 위해 사용된다.
        - multiple-available : [옵션] Holder가 Issuer에게 발급 받은 자격 증명의 개수를 나타내는 값
        - credential_preview : Holder가 증명하려는 정보가 담긴 JSON-LD 정보, Credential Preview 정보랑 같다. (아래 Preview Credential에 정리)
        - formats : offer~attach 값과 @id, 검증 가능한 자격 증명 형식 및 버전을 제공한다.
        - offer~attach : 제공되는 자격 증명을 추가로 정의하는 첨부 파일 정보
        
        Offer의 첨부 파일 형식은 아래 표를 따른다.
        
        | Credential Format | Format Value | Link to Attachment Format |
        | --- | --- | --- |
        | DIF Credential Manifest | dif/credential-manifest@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#propose-credential-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#propose-credential-attachment-format |
        | Linked Data Proof VC Detail | aries/ld-proof-vc-detail@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-format |
        | Hyperledger Indy Credential Filter | hlindy/cred-filter@v2.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-filter-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-filter-format |
        | Hyperledger AnonCreds Credential Filter | anoncreds/credential-filter@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-filter-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-filter-format |
    
3. Request Credential
    
    Holder가 Issuer에게 VC 발급을 요청하는 메시지
    
    - Request Credential Message Type
        
        ```json
        {
            "@type": "https://didcomm.org/issue-credential/%VER/request-credential",
            "@id": "<uuid of request message>",
            "goal_code": "<goal-code>",
            "comment": "<some comment>",
            "formats" : [
                {
                    "attach_id" : "<attach@id value>",
                    "format" : "<format-and-version>",
                }
            ],
            "requests~attach": [
                {
                    "@id": "<attachment identifier>",
                    "mime-type": "application/json",
                    "data": {
                        "base64": "<bytes for base64>"
                    }
                },
            ],
            "supplements": [
                {
                    "type": "hashlink-data",
                    "ref": "<attachment identifier>",
                    "attrs": [{
                        "key": "field",
                        "value": "<fieldname>"
                    }]
                },
                {
                    "type": "issuer-credential",
                    "ref": "<attachment identifier>",
                }
            ],
            "~attach" : [] //attachments referred to in supplements
        }
        ```
        
        - formats : request~attach 값과 @id, 검증 가능한 자격 증명 형식 및 버전을 제공한다.
        - requests~attach : 자격 증명에 요청된 형식을 정의하는 첨부 파일의 배열
        
        Request의 첨부 파일 형식은 아래 표를 따른다.
        
        | Credential Format | Format Value | Link to Attachment Format |
        | --- | --- | --- |
        | DIF Credential Manifest | dif/credential-manifest@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#offer-credential-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#offer-credential-attachment-format |
        | Hyperledger Indy Credential Abstract | hlindy/cred-abstract@v2.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-abstract-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-abstract-format |
        | Linked Data Proof VC Detail | aries/ld-proof-vc-detail@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-format |
        | Hyperledger AnonCreds Credential Offer | anoncreds/credential-offer@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-offer-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-offer-format |
        - ACA-PY
            
            ```json
            {
              "auto_issue": true,
              "auto_remove": true,
              "comment": "string",
              "connection_id": "dda5bdbc-7111-4634-aeda-c632a3671fd3",
              "credential_preview": {
                "@type": "issue-credential/2.0/credential-preview",
                "attributes": [
                  {"name": "name","value": "Alice Smith"},
                  {"name": "timestamp","value": "1234567890"},
                  {"name": "date","value": "2018-05-28"},
                  {"name": "degree","value": "Maths"},
                  {"name": "birthdate_dateint","value": "19640101"}
                ]
              },
              "filter": {
                "indy": {
                  "cred_def_id": "VV9pK5ZrLPRwYmotgACPkC:3:CL:10:default",
                  "issuer_did": "VV9pK5ZrLPRwYmotgACPkC",
                  "schema_id": "VV9pK5ZrLPRwYmotgACPkC:2:prefs:1.0",
                  "schema_issuer_did": "VV9pK5ZrLPRwYmotgACPkC",
                  "schema_name": "prefs",
                  "schema_version": "1.0"
                }
              },
              "trace": true
            }
            ```
            
    
4. Issue Credential
    
    Issuer가 Holder에게 VC 전달을 위해 사용하는 메시지
    
    - Issue Credential Message Type
        
        ```json
        {
            "@type": "https://didcomm.org/issue-credential/%VER/issue-credential",
            "@id": "<uuid of issue message>",
            "goal_code": "<goal-code>",
            "replacement_id": "<issuer unique id>",
            "comment": "<some comment>",
            "more_available": "<count>",
            "formats" : [
                {
                    "attach_id" : "<attachment identifier>",
                    "format" : "<format-and-version>",
                }
            ],
            "credentials~attach": [
                {
                    "@id": "<attachment identifier>",
                    "mime-type": "application/json",
                    "data": {
                        "base64": "<bytes for base64>"
                    }
                }
            ],
            "supplements": [
                {
                    "type": "hashlink-data",
                    "ref": "<attachment identifier>",
                    "attrs": [{
                        "key": "field",
                        "value": "<fieldname>"
                    }]
                },
                {
                    "type": "issuer-credential",
                    "ref": "<attachment identifier>",
                }
            ],
            "~attach" : [] //attachments referred to in supplements       
        }
        ```
        
        - more_available : [옵션]
        - formats : request~attach 값과 @id, 검증 가능한 자격 증명 형식 및 버전을 제공한다.
        - requests~attach : 자격 증명에 요청된 형식을 정의하는 첨부 파일의 배열
        
        Request의 첨부 파일 형식은 아래 표를 따른다.
        
        | Credential Format | Format Value | Link to Attachment Format |
        | --- | --- | --- |
        | DIF Credential Manifest | dif/credential-manifest@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#offer-credential-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0511-dif-cred-manifest-attach/README.md#offer-credential-attachment-format |
        | Hyperledger Indy Credential Abstract | hlindy/cred-abstract@v2.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-abstract-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#cred-abstract-format |
        | Linked Data Proof VC Detail | aries/ld-proof-vc-detail@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0593-json-ld-cred-attach/README.md#ld-proof-vc-detail-attachment-format |
        | Hyperledger AnonCreds Credential Offer | anoncreds/credential-offer@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-offer-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#credential-offer-format |
        - ACA-PY
            
            ```json
            {
              "auto_issue": true,
              "auto_remove": true,
              "comment": "string",
              "connection_id": "dda5bdbc-7111-4634-aeda-c632a3671fd3",
              "credential_preview": {
                "@type": "issue-credential/2.0/credential-preview",
                "attributes": [
                  {"name": "name","value": "Alice Smith"},
                  {"name": "timestamp","value": "1234567890"},
                  {"name": "date","value": "2018-05-28"},
                  {"name": "degree","value": "Maths"},
                  {"name": "birthdate_dateint","value": "19640101"}
                ]
              },
              "filter": {
                "indy": {
                  "cred_def_id": "VV9pK5ZrLPRwYmotgACPkC:3:CL:10:default",
                  "issuer_did": "VV9pK5ZrLPRwYmotgACPkC",
                  "schema_id": "VV9pK5ZrLPRwYmotgACPkC:2:prefs:1.0",
                  "schema_issuer_did": "VV9pK5ZrLPRwYmotgACPkC",
                  "schema_name": "prefs",
                  "schema_version": "1.0"
                }
              },
              "trace": true
            }
            ```
            

- 추가 형식
    - Preview Credential
        
        ```json
        {
            "@type": "https://didcomm.org/issue-credential/%VER/credential-preview",
            "attributes": [
                {
                    "name": "<attribute name>",
                    "mime-type": "<type>",
                    "value": "<value>"
                },
                // more attributes
            ]
        }
        ```
        
        Preview Credential : [https://github.com/hyperledger/aries-rfcs/blob/main/features/0453-issue-credential-v2/README.md#preview-credential](https://github.com/hyperledger/aries-rfcs/blob/main/features/0453-issue-credential-v2/README.md#preview-credential)
        
    - Supplements
        
        ```json
        {
            "type": "<supplement_type>",
            "ref": "<attachment_id>",
            "attrs": [
                {
                    "key": "<attr_key>",
                    "value": "<attr_value>"
                }
            ]
        }
        ```
        
        Supplements : [https://github.com/hyperledger/aries-rfcs/blob/main/features/0453-issue-credential-v2/README.md#supplements](https://github.com/hyperledger/aries-rfcs/blob/main/features/0453-issue-credential-v2/README.md#supplements)
        
    - goal_code
        
        
        Goal Codes : [https://github.com/hyperledger/aries-rfcs/blob/main/concepts/0519-goal-codes/README.md](https://github.com/hyperledger/aries-rfcs/blob/main/concepts/0519-goal-codes/README.md)
        

Hyperledger Aries issuer-credential-v2 : [https://github.com/hyperledger/aries-rfcs/blob/main/features/0453-issue-credential-v2/README.md](https://github.com/hyperledger/aries-rfcs/blob/main/features/0453-issue-credential-v2/README.md)

W3C Verifiable Credential Data Model v1.1 : [https://www.w3.org/TR/vc-data-model/#core-data-model](https://www.w3.org/TR/vc-data-model/#core-data-model)

## Hyperledger Aries - Present Proof Protocol 2.0



- Present Proof Protocol 2.0 상태머신
    
    ![Untitled](Image/Untitled%206.png)
    

- Present Proof Protocol 2.0 동작
    
    ![Untitled](Image/Untitled%207.png)
    
    ![Untitled](Image/Untitled%208.png)
    

1. Propose Presentation
    
    Holder가 Verifier에게 보내는 프레젠테이션 제안 메시지 (옵션)
    
    - Propose Presentation Message
        
        ```json
        {
            "@type": "https://didcomm.org/present-proof/%VER/propose-presentation",
            "@id": "<uuid-propose-presentation>",
            "goal_code": "<goal-code>",
            "comment": "some comment",
            "formats" : [
                {
                    "attach_id" : "<attach@id value>",
                    "format" : "<format-and-version>",
                }
            ],
            "proposals~attach": [
                {
                    "@id": "<attachment identifier>",
                    "mime-type": "application/json",
                    "data": {
                        "json": "<json>"
                    }
                }
            ]
        }
        ```
        
        - formats : proposals~attach 값과 @id, 검증 가능한 자격 증명 형식 및 버전을 제공한다.
        - proposals~attach : 제안되는 프레젠테이션 요청을 추가로 정의하는 첨부 파일 정보
        
        Propose의 첨부 파일 형식은 아래 표를 따른다.
        
        | Presentation Format | Format Value | Link to Attachment Format | Comment |
        | --- | --- | --- | --- |
        | Hyperledger Indy Proof Req | hlindy/proof-req@v2.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#proof-request-format | Used to propose as well as request proofs. |
        | DIF Presentation Exchange | dif/presentation-exchange/definitions@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0510-dif-pres-exch-attach/README.md#propose-presentation-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0510-dif-pres-exch-attach/README.md#propose-presentation-attachment-format |  |
        | Hyperledger AnonCreds Proof Request | anoncreds/proof-request@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#proof-request-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#proof-request-format | Used to propose as well as request proofs. |
    
2. Request Presentation
    
    Verifier가 Holder에게 프레젠테이션을 요청하는 메시지
    
    - Request Presentation Message
        
        ```json
        {
            "@type": "https://didcomm.org/present-proof/%VER/request-presentation",
            "@id": "<uuid-request>",
            "goal_code": "<goal-code>",
            "comment": "some comment",
            "will_confirm": true,
            "present_multiple": false,
            "formats" : [
                {
                    "attach_id" : "<attach@id value>",
                    "format" : "<format-and-version>",
                }
            ],
            "request_presentations~attach": [
                {
                    "@id": "<attachment identifier>",
                    "mime-type": "application/json",
                    "data":  {
                        "base64": "<base64 data>"
                    }
                }
            ]
        }
        ```
        
        - will_confirm : Verifier가 프레젠테이션을 받은 후 확인 메시지를 보낼 것인지 나타내는 정보
        - present_multiple : Verifier가 여러 개의 프레젠테이션을 요청할 것인지 나타내는 정보
        - formats : request_presentations~attach 값과 @id, 검증 가능한 자격 증명 형식 및 버전을 제공한다.
        - request_presentations~attach : 제안되는 프레젠테이션 요청을 추가로 정의하는 첨부 파일 정보
        
        Presentation Request의 첨부 파일 형식은 아래 표를 따른다.
        
        | Presentation Format | Format Value | Link to Attachment Format | Comment |
        | --- | --- | --- | --- |
        | Hyperledger Indy Proof Req | hlindy/proof-req@v2.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#proof-request-format | Used to propose as well as request proofs. |
        | DIF Presentation Exchange | dif/presentation-exchange/definitions@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0510-dif-pres-exch-attach/README.md#request-presentation-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0510-dif-pres-exch-attach/README.md#request-presentation-attachment-format |  |
        | Hyperledger AnonCreds Proof Request | anoncreds/proof-request@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#proof-request-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#proof-request-format | Used to propose as well as request proofs. |
        - ACA-PY
            
            ```json
            {
              "comment": "This is a comment about the reason for the proof",
              "connection_id": "69fd8c81-3bdd-4881-8a4d-3719ee11a466",
              "presentation_request": {
                "indy": {
                  "name": "Proof of Education",
                  "version": "1.0",
                  "requested_attributes": {
                    "0_name_uuid": {
                      "name": "name",
                      "restrictions": [
                        {
                          "cred_def_id": "VV9pK5ZrLPRwYmotgACPkC:3:CL:10:default"
                        }
                      ]
                    },
                    "0_date_uuid": {
                      "name": "date",
                      "restrictions": [
                        {
                          "cred_def_id": "VV9pK5ZrLPRwYmotgACPkC:3:CL:10:default"
                        }
                      ]
                    },
                    "0_degree_uuid": {
                      "name": "degree",
                      "restrictions": [
                        {
                          "cred_def_id": "VV9pK5ZrLPRwYmotgACPkC:3:CL:10:default"
                        }
                      ]
                    }
                  },
                  "requested_predicates": {       
                  }
                }
              }
            }
            ```
            
    
3. Presentation
    
    Holder가 Verifier에게 프레젠테이션을 전달하는 메시지
    
    - Presentation Message
        
        ```json
        {
            "@type": "https://didcomm.org/present-proof/%VER/presentation",
            "@id": "<uuid-presentation>",
            "goal_code": "<goal-code>",
            "comment": "some comment",
            "last_presentation": true,
            "formats" : [
                {
                    "attach_id" : "<attach@id value>",
                    "format" : "<format-and-version>",
                }
            ],
            "presentations~attach": [
                {
                    "@id": "<attachment identifier>",
                    "mime-type": "application/json",
                    "data": {
                        "sha256": "f8dca1d901d18c802e6a8ce1956d4b0d17f03d9dc5e4e1f618b6a022153ef373",
                        "links": ["https://ibb.co/TtgKkZY"]
                    }
                }
            ],
            "supplements": [
                {
                    "type": "hashlink-data",
                    "ref": "<attachment identifier>",
                    "attrs": [{
                        "key": "field",
                        "value": "<fieldname>"
                    }]
                },
                {
                    "type": "issuer-credential",
                    "ref": "<attachment identifier>",
                }
            ],
            "~attach" : [] //attachments referred to in supplements   
        }
        ```
        
        - last_presentation : 프레젠테이션 요청을 위한 마지막 메시지인지 확인, false인 경우 추가 프레젠테이션을 전달한다.
        - formats : presentations~attach 값과 @id, 검증 가능한 자격 증명 형식 및 버전을 제공한다.
        - presentations~attach : 제안되는 프레젠테이션 요청을 추가로 정의하는 첨부 파일 정보
        
        Presentation의 첨부 파일 형식은 아래 표를 따른다.
        
        | Presentation Format | Format Value | Link to Attachment Format |
        | --- | --- | --- |
        | Hyperledger Indy Proof | hlindy/proof@v2.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0592-indy-attachments/README.md#proof-format |
        | DIF Presentation Exchange | dif/presentation-exchange/submission@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0510-dif-pres-exch-attach/README.md#presentation-attachment-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0510-dif-pres-exch-attach/README.md#presentation-attachment-format |
        | Hyperledger AnonCreds Proof | anoncreds/proof@v1.0 | https://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#proof-formathttps://github.com/hyperledger/aries-rfcs/blob/main/features/0771-anoncreds-attachments/README.md#proof-format |
        - ACA-PY
            
            ```json
            {
              "indy": {
                "requested_predicates": {      
                },
                "requested_attributes": {
                  "0_name_uuid": {
                    "cred_id": "d33f5508-60cc-4e19-9d59-0fd412e2ba04",
                    "revealed": false
                  },
                  "0_date_uuid": {
                    "cred_id": "d33f5508-60cc-4e19-9d59-0fd412e2ba04",
                    "revealed": true
                  },
                  "0_degree_uuid": {
                    "cred_id": "d33f5508-60cc-4e19-9d59-0fd412e2ba04",
                    "revealed": true
                  }
                },
                "self_attested_attributes": { 
                }
              }
            }
            ```
            
        

Hyperledger Aries present-proof-v2 : [0454-present-proof-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0454-present-proof-v2)

W3C Verifiable Credential Data Model v1.1 : [https://www.w3.org/TR/vc-data-model/#core-data-model](https://www.w3.org/TR/vc-data-model/#core-data-model)

## Hyperledger Aries - Out of Band Protocol 1.1

특정 에이전트와의 연결을 모를 때 사용한다. 새 연결을 설정하거나 연결되어 있지만 상대방이 누군지 모르거나, 연결 없는 상호작용을 원할 때 사용된다. 해당 연결은 DIDComm 연결이 없기 때문에 일반 메시지를 사용하며 QR코드, 이메일 또는 기타 사용 가능한 채널과 같이 ‘Out of Band’로 전송된다.

- 발신자의 상태머신

![state-machine-sender.png](Image/state-machine-sender.png)

- 수신자의 상태머신

![state-machine-receiver.png](Image/state-machine-receiver.png)

1. 발신자 메시지
    
    asd
    
    - Message Type
        
        ```json
        {
          "@type": "https://didcomm.org/out-of-band/%VER/invitation",
          "@id": "<id used for context as pthid>",
          "label": "Faber College",
          "goal_code": "issue-vc",
          "goal": "To issue a Faber College Graduate credential",
          "handshake_protocols": [
              "https://didcomm.org/didexchange/1.0",
              "https://didcomm.org/connections/1.0"
              ],
          "request~attach": [
            {
                "@id": "request-0",
                "mime-type": "application/json",
                "data": {
                    "json": "<json of protocol message>"
                }
            }
          ],
          "service": ["did:sov:LjgpST2rjsoxYegQDRm7EL"]
        }
        ```
        
        - @type : DIDComm 메시지 유형
        - @id : 메시지 고유 id
        - label : [옵션]
        - goal_code : [옵션]
        - goal : [옵션]
        - handshake_protocols : [옵션]
        - request~attach : [옵션]
        - service : 수신자가 메시지 응답에 사용할 DIDDoc 내용이 담긴 곳

Hyperledger Aries outofband : [0434-outofband](https://github.com/hyperledger/aries-rfcs/tree/main/features/0434-outofband)

## Hyperledger Aries - DID Exchange Protocol 1.0

DID를 사용한 통신을 위해선 기반 데이터가 필요하며 이를 가져올 수 있는 DID를 교환하는 명확한 프로토콜을 정의할 필요가 있다. 요청자는 메시지 수신 이후 초대(Out of Band) 또는 공개 DID의 묵시적 초대를 사용해 프로토콜을 시작하는 당사자이다.

- DID Exchange 상태 머신
    
    ![did-exchange-states.png](Image/did-exchange-states.png)
    

1. Exchange Request
    
    DID 교환을 원하는 요청자가 DID 문서를 응답자에게 전달하기 위해 사용하는 메시지
    
    - Exchange Request Message Type
        
        ```json
        {
          "@id": "5678876542345",
          "@type": "https://didcomm.org/didexchange/1.0/request",
          "~thread": { 
              "thid": "5678876542345",
              "pthid": "<id of invitation>"
          },
          "label": "Bob",
          "goal_code": "aries.rel.build",
          "goal": "To create a relationship",
          "did": "B.did@B:A",
          "did_doc~attach": {
              "@id": "d2ab6f2b-5646-4de3-8c02-762f553ab804",
              "mime-type": "application/json",
              "data": {
                 "base64": "eyJ0eXAiOiJKV1Qi... (bytes omitted)",
                 "jws": {
                    "header": {
                       "kid": "did:key:z6MkmjY8GnV5i9YTDtPETC2uUAW6ejw3nk5mXF5yci5ab7th"
                    },
                    "protected": "eyJhbGciOiJFZERTQSIsImlhdCI6MTU4Mzg4... (bytes omitted)",
                    "signature": "3dZWsuru7QAVFUCtTd0s7uc1peYEijx4eyt5... (bytes omitted)"
                    }
              }
           }
        }
        ```
        
        - ~thread : 요청 메시지에 대한 참조
            - thread message 정보 : [https://www.notion.so/Hyperledger-Aries-10dfa3de368e43b2aff2001cf5cef71e?pvs=4#63d24a517d2349329a519b930b56556a](https://www.notion.so/Hyperledger-Aries-10dfa3de368e43b2aff2001cf5cef71e)
        - goal : [옵션]
        - did : Invitee의 DID
        - did_doc~attach : [옵션] 위 DID에 대한 DIDDoc 정보, DID를 통해 원장에서 DIDDoc를 확인할 수 있는 경우 생략할 수 있다.
        
        초대장은 두 가지 형식으로 나뉘며 ‘out-of-band’를 사용한 명시적 초대와 DIDComm 규칙을 지키는 DIDDoc를 사용한 암묵적 초대로 나뉜다. 
        
        - 명시적 초대
            
            ```json
            {
              "@id": "a46cdd0f-a2ca-4d12-afbf-2e78a6f1f3ef",
              "@type": "https://didcomm.org/didexchange/1.0/request",
              "~thread": { 
                  "thid": "a46cdd0f-a2ca-4d12-afbf-2e78a6f1f3ef",
                  "pthid": "032fbd19-f6fd-48c5-9197-ba9a47040470" 
              },
              "label": "Bob",
              "goal_code": "aries.rel.build",
              "goal": "To create a relationship",
              "did": "B.did@B:A",
              "did_doc~attach": {
                  "@id": "d2ab6f2b-5646-4de3-8c02-762f553ab804",
                  "mime-type": "application/json",
                  "data": {
                     "base64": "eyJ0eXAiOiJKV1Qi... (bytes omitted)",
                     "jws": {
                        "header": {
                           "kid": "did:key:z6MkmjY8GnV5i9YTDtPETC2uUAW6ejw3nk5mXF5yci5ab7th"
                        },
                        "protected": "eyJhbGciOiJFZERTQSIsImlhdCI6MTU4Mzg4... (bytes omitted)",
                        "signature": "3dZWsuru7QAVFUCtTd0s7uc1peYEijx4eyt5... (bytes omitted)"
                        }
                  }
               }
            }
            ```
            
        - 암묵적 초대
            
            ```json
            {
              "@id": "a46cdd0f-a2ca-4d12-afbf-2e78a6f1f3ef",
              "@type": "https://didcomm.org/didexchange/1.0/request",
              "~thread": { 
                  "thid": "a46cdd0f-a2ca-4d12-afbf-2e78a6f1f3ef",
                  "pthid": "did:example:21tDAKCERh95uGgKbJNHYp#didcomm" 
              },
              "label": "Bob",
              "goal_code": "aries.rel.build",
              "goal": "To create a relationship",
              "did": "B.did@B:A",
              "did_doc~attach": {
                  "@id": "d2ab6f2b-5646-4de3-8c02-762f553ab804",
                  "mime-type": "application/json",
                  "data": {
                     "base64": "eyJ0eXAiOiJKV1Qi... (bytes omitted)",
                     "jws": {
                        "header": {
                           "kid": "did:key:z6MkmjY8GnV5i9YTDtPETC2uUAW6ejw3nk5mXF5yci5ab7th"
                        },
                        "protected": "eyJhbGciOiJFZERTQSIsImlhdCI6MTU4Mzg4... (bytes omitted)",
                        "signature": "3dZWsuru7QAVFUCtTd0s7uc1peYEijx4eyt5... (bytes omitted)"
                        }
                  }
               }
            }
            ```
            
    
2. Exchange Response
    
    Exchange Request에 대한 응답을 위해 전달하는 메시지
    
    - Exchange Response Message Type
        
        ```json
        {
          "@type": "https://didcomm.org/didexchange/1.0/response",
          "@id": "12345678900987654321",
          "~thread": {
            "thid": "<The Thread ID is the Message ID (@id) of the first message in the thread>"
          },
          "did": "B.did@B:A",
          "did_doc~attach": {
              "@id": "d2ab6f2b-5646-4de3-8c02-762f553ab804",
              "mime-type": "application/json",
              "data": {
                 "base64": "eyJ0eXAiOiJKV1Qi... (bytes omitted)",
                 "jws": {
                    "header": {
                       "kid": "did:key:z6MkmjY8GnV5i9YTDtPETC2uUAW6ejw3nk5mXF5yci5ab7th"
                    },
                    "protected": "eyJhbGciOiJFZERTQSIsImlhdCI6MTU4Mzg4... (bytes omitted)",
                    "signature": "3dZWsuru7QAVFUCtTd0s7uc1peYEijx4eyt5... (bytes omitted)"
                    }
              }
           }
        }
        ```
        
        - ~thread : 요청 메시지에 대한 참조
        - did : Invitee의 DID
        - did_doc~attach : [옵션] 위 DID에 대한 DIDDoc 정보, DID를 통해 원장에서 DIDDoc를 확인할 수 있는 경우 생략할 수 있다.
    
3. Exchange Complete
    
    연결 확인을 위해 요청자가 보내는 완료 메시지
    
    - Exchange Complete Message Type
        
        ```json
        {
          "@type": "https://didcomm.org/didexchange/1.0/complete",
          "@id": "12345678900987654321",
          "~thread": {
            "thid": "<The Thread ID is the Message ID (@id) of the first message in the thread>",
            "pthid": "<pthid used in request message>"
          }
        }
        ```
        
        - ~thread : 요청 메시지에 대한 참조

Hyperledger Aries did-exchange : [Aries RFC 0023: DID Exchange Protocol 1.0](https://github.com/hyperledger/aries-rfcs/blob/main/features/0023-did-exchange/README.md)

## Hyperledger Aries - Basic Message Protocol 1.0

메시지를 전달하는 기본적인 기능이다. 메시지 통신의 가장 기본적인 형태이며 구현을 쉽게하기 위해 고급 기능이 제외되어있다.

Hyperledger Aries basic message : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0095-basic-message](https://github.com/hyperledger/aries-rfcs/tree/main/features/0095-basic-message)

## Hyperledger Aries - Trust Ping Protocol 1.0

각각의 에이전트가 동일하게 동작한다는 보장이 없으며 이로인해 두 에이전트가 기능적인 쌍방향 채널을 가지고 있다는 증명이 어려울 수 있다. Trust Ping은 전송과 응답을 통해 채널이 제대로 동작하는지, 안전한지 등을 테스트할 수 있다. 

Hyperledger Aries trust ping : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0048-trust-ping](https://github.com/hyperledger/aries-rfcs/tree/main/features/0048-trust-ping)

## Hyperledger Aries - Action Menu Protocol

상대 에이전트가 가진 기능을 확인하기 위해 요청하는 메시지

Hyperledger Aries action menu : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0509-action-menu](https://github.com/hyperledger/aries-rfcs/tree/main/features/0509-action-menu)

## Hyperledger Aries - Discover Features Protocol v2.x

서로 다른 Agent의 통신을 위해 다양한 Protocol이 Hyperledger Aries에서 표준화되고 있다. 그러나 Agent에는 다양한 기능들이 추가될 수 있으며 이는 다른 Agent들이 확인하기 어렵다. Discover Features Protocol은 Agent가 가진 기능을 

- Queries Message
    
    ```json
    {
      "@type": "https://didcomm.org/discover-features/2.0/queries",
      "@id": "yWd8wfYzhmuXX3hmLNaV5bVbAjbWaU",
      "queries": [
        { "feature-type": "protocol", "match": "https://didcomm.org/tictactoe/1.*" },
        { "feature-type": "goal-code", "match": "aries.*" }
      ]
    }
    ```
    

Hyperledger Discover Features Protocol v2.x : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0557-discover-features-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0557-discover-features-v2)

## Hyperledger Aries - Revocation Notification 2.0

Issuer가 Holder에게 VC가 취소되었음을 알리기 위해 사용하는 Protocol이다.

예를 들어, 여권 대행사(Issuer)가 Alice(Holder)의 여권(VC)을 취소할 때 여권이 취소되어 사용할 수 없음을 알릴 수 있다.

- 메시지
    
    asd
    
    - Revocation Notification Message Type
        
        ```json
        {
          "@type": "https://didcomm.org/revocation_notification/2.0/revoke",
          "@id": "<uuid-revocation-notification>",
          "~please_ack": ["RECEIPT","OUTCOME"],
          "revocation_format": "<revocation_format>",
          "credential_id": "<credential_id>",
          "comment": "Some comment"
        }
        ```
        
        - ~please_ack : [옵션] 임의의 ack를 보내기 위해 사용
        - revocation_format :
        - credential_ai :
        - comment :

Hyperledger Aries Revocation Notification 2.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0721-revocation-notification-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0721-revocation-notification-v2)

## Hyperledger Aries - Mediator Coordination Protocol

Agent 사이의 중재자가 필요할 때 사용하는 Protocol으로 전달을 위한 메시지 형식을 정의한다.

Hyperledger Aries Mediator Coordination Protocol : [](https://github.com/hyperledger/aries-rfcs/tree/fa8dc4ea1e667eb07db8f9ffeaf074a4455697c0/features/0211-route-coordination)[https://github.com/hyperledger/aries-rfcs/tree/main/features/0211-route-coordination](https://github.com/hyperledger/aries-rfcs/tree/main/features/0211-route-coordination)

## Hyperledger Aries - Encryption Envelope v2

DIF에서 정의하는 DIDComm Messaging을 지원하기 위해 진행하는 표준화로 암호화 메시지를 정의한다.

Hyperledger Aries Encryption Envelope v2 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0587-encryption-envelope-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0587-encryption-envelope-v2)

DIF - DIDComm Messaging : [https://identity.foundation/didcomm-messaging/spec/](https://identity.foundation/didcomm-messaging/spec/)

[ACA-PY 실행 테스트](https://www.notion.so/ACA-PY-75c3ce873aa641cfbd86f885f01b052d)

[ACA-PY 코드 분석](https://www.notion.so/ACA-PY-b9109d90d79643bcafc28473ff17684f)