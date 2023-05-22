# ACA-PY 코드 분석

ACA-PY의 코드 내용을 분석

Hyperledger Aries 클라우드 에이전트 - Python : [https://github.com/hyperledger/aries-cloudagent-python](https://github.com/hyperledger/aries-cloudagent-python)

주요 코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent)

## admin

: 서버 및 사이트 동작 코드

Aries의 경우 동작 시 Swagger API를 사용하며 중간 Agent로 애플리케이션에서 보내는 요청을 받아 처리하기 위한 서버가 필요하며 admin은 해당 부분을 구현

## askar

 : Aries용 보안 저장소

askar의 경우 Hyperledger Aries Agent를 위해 설계된 보안 스토리지 및 암호화를 지원해주는 프로젝트이다.

<사이트> : [https://github.com/hyperledger/aries-askar](https://github.com/hyperledger/aries-askar)

## commands

 : 명령어별 기능 코드

Aries 스크립트에 전달하는 명령어를 처리하는 코드로 밑에 작성된 Hyperledger Aries 명령어를 처리한다.

## connections

 : 연결을 위한 초대장 생성 및 연결 기능 구현

Aries는 초대장을 통한 통신으로 채널을 생성해 연결하며 이부분에 대한 기능이 작성되어 있다. 

- base_manager.py
    
    BaseConnectionManager.class
    
    - 연결을 위한 기능들을 제공하며 이때 필요한 Key, DIDDoc 등의 정보를 조회한다.
    - Connection, DIDExchange, OutOfBand Manager에 사용되는 기본 매니저
    - 주요 기능
        - create_did_document : did_info를 통해 DIDDoc 생성
        - store_did_document
        - add_key_for_did
        - find_did_for_key
        - remove_keys_for_did
        - resolve_invitation
        - _extract_key_material_in_base58_format
        - fetch_connection_targets
        - diddoc_connection_targets
        - fetch_did_document

## core

 : Aries의 메인 기능 구현

Aries의 메인 기능들이 구현되어 있다. 구현되어 있는 각각들의 기능들에 명령 및 관리를 담당하며 그외 데이터 관리, 스레드 생성 등 Agent의 메인에 해당하는 기능들이 구현되어 있다.

- profile.py
    
    Profile.class (가상 클래스)
    
    - ID 관련 상태 처리를 위한 기본 추상화, ACA-PY에서 사용하는 모든 프로토콜들은 객체 생성 시 Profile 정보를 요구한다.
    - Profile은 실행 시 설정되는 Config 값들을 가져와 만들어지며 이때 InjectionContext 값을 사용한다. (InjectionContext는 Config에 존재)
    - Profile은 연결 정보를 가지고 있어 통신 연결 시 Profile을 거쳐 연결 정보를 조회한다.
    
    ProfileManager.class (가상 클래스)
    
    ProfileSession.class (가상 클래스)
    
    ProfileManagerProvider.class
    
    - BaseProvider를 상속하며 이는 Config의 base.py에 정의되어 있다.

코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/core](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/core)

## config

 : 사용자 정보 설정

injection_context.py

 : config 설정 및 클래스 공급자의 관리자, 

## messaging

 : ACA-PY 메시지의 기본 베이스 정의

ACA-PY에서 사용하는 메시지들은 모두 ‘base_message.py’를 상속해 사용하여 messagin에 있는 코드들은 ‘base_message.py’를 기반으로 하는 에러, 핸들러 등의 기능들을 구현한다. 

base_message.py

 : ACA-PY에서 사용하는 메시지의 최소 메시지, 어떤 방식으로든 확장이 가능하다. (BaseMessage)

models/base.py

 : 편리한 기능 제공을 위한 기본 모델, json 클래스를 json 파일로 변환하거나 json 파일 검증 등의 기능을 제공 (BaseModel)

models/base_record.py

 : 기본 저장소 기반 기록 관리를 위한 클래스, 

agent_message.py

 : ‘BaseMessage’와 ‘BaseModel’을 입력 받아 만드는 메시지로 상대방에게 전달하기 위한 기초적인 정보가 담긴 메시지와 기능들을 제공해준다. 

## did

 : did-key와 관련된 기능 구현

did 관련 key 생성에 중점을 두고 있으며 실제 did 생성과 관련된 기능들은 대부분 indy에 있다.

## indy

 : did와 관련된 기능 구현

Hperledger Indy가 가지고 있는 DID 관련 기능(지갑 생성, VC 생성 등)이 구현되어 있다.

## ledger

 : 원장 관련 기능 구현

Hperledger Indy가 가지고 있는 원장 관련 기능이 구현되어 있다. indy-sdk의 기능인 pool 연결, 트랜잭션 생성 및 전송, pool 설정 요청 등이 있다.

## multitenant

 : 동시에 여러 요청 수행

멀티 테넌시(multitenant) 기능이 구현되어 있다. 멀티 테넌시는 다양한 사용자에 맞춰 앱을 구성하는 것이 아닌 하나의 앱을 사용자가 같이 사용하는 기능이다. 이는 개발 입장에서 한번의 업데이트 및 구성으로 일 처리를 할 수 있다는 장점이 있다.

다중 지갑 :

[https://github.com/hyperledger/aries-cloudagent-python/blob/main/Multitenancy.md](https://github.com/hyperledger/aries-cloudagent-python/blob/main/Multitenancy.md)

# protocols

 : Aries 주요 Protocol 기능

실제 ACA-PY 실행 시 나오는 API들이 해당 폴더에 저장되어 있으며 각각 내부의 기능들은 다른 폴더 기능들을 가져와 사용한다.

기본적으로 모든 Protocol들은 core의 Profile 정보를 가져와 객체를 생성하며 manager를 통해 해당 Protocol 기능을, message를 통해 Protocol에 사용하는 메시지를 정의한다.

코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols)

Aries RFC 0003 Protocols : [https://github.com/hyperledger/aries-rfcs/tree/main/concepts/0003-protocols](https://github.com/hyperledger/aries-rfcs/tree/main/concepts/0003-protocols)

### actionmenu

 : 서버 조정 관련 기능

Aries RFC 0509 Action Menu Protocol : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0509-action-menu](https://github.com/hyperledger/aries-rfcs/tree/main/features/0509-action-menu)

### basicmessage

 : 기본 메시지 보내기

‘connections’을 통한 다른 에이전트와의 연결 이후 일반 메시지 전송을 위한 기능을 제공

Aries RFC 0095 Basic Message Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0095-basic-message](https://github.com/hyperledger/aries-rfcs/tree/main/features/0095-basic-message)

### connections

 : 다른 에이전트와의 연결 생성

다른 에이전트와의 통신을 위해 제일 처음 실행하는 부분으로 초대장을 만들거나 읽어 상대방과 통신을 위한 peer did 생성 및 연결이 이루어진다.

- manager.py (BaseConnectionManager 상속)
    
    ConnectionManager.class
    
    - ‘aries_cloudagent/connections/base_manager.py’의 ‘BaseConnectionManager’ 클래스를 상속
    - 객체 생성 시 Profile 값을 가져와 생성 (core의 Profile 확인)
    - 가지고 있는 기능
        - create_invitation : 이 상호 작용은 대역 외 통신 채널을 나타냅니다. 미래에는 실제로 이러한 종류의 초대가 SMS, 이메일, QR 코드, NFC 등과 같은 여러 채널을 통해 수신될 것입니다.
        - receive_invitation
        - create_request
        - receive_request
        - create_response
        - accept_response
        - get_endpoints
        - create_static_connection
        - find_connection
        - find_inbound_connection
        - resolve_inbound_connection
        - get_connection_targets
        - establish_inbound
        - update_inbound
        
- message_types.py
    - 메시지에 사용할 설정 및 고정 값들 정의 (type, version 등)
    
- message
    
    Connection에서 사용하는 메시지 정의
    
    - connection_invitation.py
        
         : ConnectionInvitation.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 Connection Invitation 메시지에 필요한 값들을 추가 호출 후 적용
        
         : ConnectionInvitationSchema.class (AgentMessageSchema상속)
        
        - Connection Invitation 메시지의 속성 값들 정의
        
    - connection_request.py
        
         : ConnectionRequest.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 Connection Request 메시지에 필요한 값들을 추가 호출 후 적용
        
         : ConnectionRequestSchema.class (AgentMessageSchema상속)
        
        - Connection Request 메시지의 속성 값들 정의
        
    - connection_response.py
        
         : ConnectionResponse.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 Connection Response 메시지에 필요한 값들을 추가 호출 후 적용
        
         : ConnectionResponseSchema.class (AgentMessageSchema상속)
        
        - Connection Response 메시지의 속성 값들 정의
        
    

코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/connections](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/connections)

Aries RFC 0160 Connection Protocol : [Hyperledger Aries protocol](https://github.com/hyperledger/aries-rfcs/tree/main/features/0160-connection-protocol)

### coordinate_mediation

 : 중재자 기능

에이전트 연결 사이의 중재자를 위한 기능을 제공해준다.

Aries RFC 0211 Mediator Coordination Protocol : [0211-route-coordination](https://github.com/hyperledger/aries-rfcs/tree/main/features/0211-route-coordination)

### didexchange

 : 상대방과 DID를 교환하여 연결을 만드는 기능을 제공해준다.

- manager.py
    
     : DIDXManager.class (BaseConnectionManager 상속)
    
    - ‘aries_cloudagent/connections/base_manager.py’의 ‘BaseConnectionManager’ 클래스를 상속
    - 객체 생성 시 Profile 값을 가져와 생성 (core의 Profile 확인)
    - 가지고 있는 기능
        - receive_invitation
        - create_request_implicit
        - create_request
        - receive_request
        - create_response
        - accept_response
        - accept_complete
        - verify_diddoc
        - get_resolved_did_document
        - get_first_applicable_didcomm_service
    
- message_types.py
    - 메시지에 사용할 설정 및 고정 값들 정의 (type, version 등)
    
- message
    
    DID Exchange에서 사용하는 메시지 정의
    
    - request.py
        
         : DIDXRequest.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 DID Exchange Request 메시지에 필요한 값들을 추가 호출 후 적용
        
         : DIDXRequestSchema.class (AgentMessageSchema상속)
        
        - DID Exchange Request 메시지의 속성 값들 정의
        
    - response.py
        
         : DIDXResponse.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 DID Exchange Response 메시지에 필요한 값들을 추가 호출 후 적용
        
         : DIDXResponseSchema.class (AgentMessageSchema상속)
        
        - DID Exchange Response 메시지의 속성 값들 정의
        
    - complete.py
        
         : DIDXRequest.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 DID Exchange Request 메시지에 필요한 값들을 추가 호출 후 적용
        
         : DIDXRequestSchema.class (AgentMessageSchema상속)
        
        - DID Exchange Request 메시지의 속성 값들 정의
        
    

코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/didexchange](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/didexchange)

Aries RFC 0023 DID Exchange Protocol 1.0 : [RFC 0023](https://github.com/hyperledger/aries-rfcs/tree/main/features/0023-did-exchange)

### discovery

 :

Aries RFC 0557 Discover Features Protocol 2.x : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0557-discover-features-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0557-discover-features-v2)

### endorse_transaction

 :

### introduction

 : 자기소개 기능

Aries RFC 0028 Introduce Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0028-introduce](https://github.com/hyperledger/aries-rfcs/tree/main/features/0028-introduce)

### issue_credential

 : VC의 생성 발급 저장 기능

Aries에서 제공하는 issuer 관련 기능들이 구현되어 있으며 크게 자격 증명 제안, 확인, 발급, 답장 등의 기능을 수행할 수 있다.

- 디렉토리
    
    ```bash
    issue_credential
    |
    +---v1_0
    |   |   controller.py
    |   |   manager.py
    |   |   message_types.py
    |   |   routes.py
    |   |
    |   \---handlers
    |   |       credential_ack_handler.py
    |   |       credential_issue_handler.py
    |   |       credential_offer_handler.py
    |   |       credential_problem_report_handler.py
    |   |       credential_proposal_handler.py
    |   |       credential_request_handler.py
    |   |   
    |   |
    |   +---messages
    |   |   |   credential_ack.py
    |   |   |   credential_exchange_webhook.py
    |   |   |   credential_issue.py
    |   |   |   credential_offer.py
    |   |   |   credential_problem_report.py
    |   |   |   credential_proposal.py
    |   |   |   credential_request.py
    |   |   |
    |   |   \---inner
    |   |           credential_preview.py
    |   |
    |   \---models
    |           credential_exchange.py
    |
    \---v2_0
        |   controller.py
        |   manager.py
        |   message_types.py
        |   routes.py
        |
        +---formats
        |   |   handler.py
        |   |
        |   +---indy
        |   |       handler.py
        |   |
        |   \---ld_proof
        |       |   handler.py
        |       |
        |       \---models
        |              cred_detail.py
        |              cred_detail_options.py
        |
        +---handlers
        |       cred_ack_handler.py
        |       cred_issue_handler.py
        |       cred_offer_handler.py
        |       cred_problem_report_handler.py
        |       cred_proposal_handler.py
        |       cred_request_handler.py
        |
        +---messages
        |   |   cred_ack.py
        |   |   cred_ex_record_webhook.py
        |   |   cred_format.py
        |   |   cred_issue.py
        |   |   cred_offer.py
        |   |   cred_problem_report.py
        |   |   cred_proposal.py
        |   |   cred_request.py
        |   |
        |   \---inner
        |           cred_preview.py
        |
        \---models
            |   cred_ex_record.py
            |
            \---detail
                  indy.py
                  ld_proof.py
    ```
    

V2

manager.py : 실제 프로토콜 실행을 위해 API들이 정리되어 있는 클래스

models/cred_ex_record.py : issue_credential에서 발생하는 메시지를 저장하기 위한 클래스 (/messaging/models/base_record.py를 상속해 사용)

messages/ : issue_credential v2에 사용되는 메시지 포멧 정리 

messages/cred_format.py : issue_credential v2의 메시지의 기반이 되는 메시지 포멧을 지원한다. (/messaging/models/base.py를 상속해 사용)

messages/cred_proposal.py : issue_credential v2의 Proposal 메시지 포멧(/messaging/agent_message.py를 상속해 사용)

messages/cred_offer.py

코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/issue_credential](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/issue_credential)

Aries RFC 0453 Issue Credential Protocol 2.0 : [0453-issue-credential-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0453-issue-credential-v2)

### notification

 :

Aries RFC 0734 Push Notifications fcm Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0734-push-notifications-fcm](https://github.com/hyperledger/aries-rfcs/tree/main/features/0734-push-notifications-fcm)

### out_of_band

 :

Aries RFC 0434  2.0 Out-of-Band Protocol 1.1 : [0434-outofband](https://github.com/hyperledger/aries-rfcs/tree/main/features/0434-outofband)

### present_proof

 : VP의 생성 발급 저장 기능

Aries RFC 0454 Present Proof Protocol 2.0 : [0454-present-proof-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0454-present-proof-v2)

### problem_report

 : 에러 메시지 기능

Aries RFC 0035 Report Problem Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0035-report-problem](https://github.com/hyperledger/aries-rfcs/tree/main/features/0035-report-problem)

### revocation_notification

 : 폐기 확인 기능

Aries RFC 0721 Revocation Notification 2.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0721-revocation-notification-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0721-revocation-notification-v2)

### routing

 :

### trustping

 : 에이전트 간 신뢰

Aries RFC 0048 Trust Ping Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0048-trust-ping](https://github.com/hyperledger/aries-rfcs/tree/main/features/0048-trust-ping)

# wallet

 : 지갑 관련 기능 및 indy의 지갑 및 지갑 관련 기술 구현

멀티 테넌시(multitenant) 기능이 구현되어 있다. 멀티 테넌시는 다양한 사용자에 맞춰 앱을 구성하는 것이 아닌 하나의 앱을 사용자가 같이 사용하는 기능이다. 이는 개발 입장에서 한번의 업데이트 및 구성으로 일 처리를 할 수 있다는 장점이 있다.

- base.py
    
    BaseWallet.class (가상 클래스)
    
    - wallet Interface를 정의한다.
    - 객체 생성 시 Profile 값을 가져와 생성 (core의 Profile 확인)
    - 기능의 대부분은 DID, Key와 관련되어 있으며 해당 정보들은 wallet 내부의 다른 클래스로 부터 가져온다. (did_info.py, key_type.py 등)
    - [https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/base.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/base.py)
    - 가지고 있는 기능
        - create_signing_key : 서명을 위한 키 쌍을 생성한다.
        - get_signing_key
        - replace_signing_key_metadata
        - rotate_did_keypair_start
        - rotate_did_keypair_apply
        - create_local_did
        - create_public_did
        - get_public_did
    
- did_info.py
    - KeyInfo, DIDInfo 정의
    - NamedTuple을 사용해 키와 타입을 정의
    - [https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/did_info.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/did_info.py)
    - 정의 모습
        
        ```python
        KeyInfo = NamedTuple(
            "KeyInfo", [("verkey", str), ("metadata", dict), ("key_type", KeyType)]
        )
        DIDInfo = NamedTuple(
            "DIDInfo",
            [
                ("did", str),
                ("verkey", str),
                ("metadata", dict),
                ("method", DIDMethod),
                ("key_type", KeyType),
            ],
        )
        ```
        

- crypto.py
    - BasicWallet에서 암호화에 필요한 함수들 제공
    - 클래스 없이 함수만을 정의
    - [https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/did_info.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/crypto.py)
    - 가지고 있는 기능
        - create_keypair : 서명을 위한 키 쌍을 생성한다.
        - create_ed25519_keypair
        - seed_to_did
        - rotate_did_keypair_start
        - rotate_did_keypair_apply
        - create_local_did
        - create_public_did
        - get_public_did

## crypto.py

## 시작순서

[https://github.com/hyperledger/aries-cloudagent-python](https://github.com/hyperledger/aries-cloudagent-python)

위 사이트의 ‘aries_cloudagent’에서 코드 상 순서 기재

main.py (run) -> command.__init__ (run_command) -> command.start (execute) -> core.conductor.py (setup, start)

self.root_profile, self.setup_public_did = await wallet_config(context)

context 값을 입력 받아 프로필 생성!

config/default_context.py

주요 암호화 및 복호화 부분 : [https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/askar/didcomm/v1.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/askar/didcomm/v1.py)