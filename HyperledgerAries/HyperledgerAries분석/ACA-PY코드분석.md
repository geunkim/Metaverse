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

## core

 : Aries의 메인 기능 구현

Aries의 메인 기능들이 구현되어 있다. 구현되어 있는 각각들의 기능들에 명령 및 관리를 담당하며 그외 데이터 관리, 스레드 생성 등 Agent의 메인에 해당하는 기능들이 구현되어 있다.

profile.py

 : ACA-PY 사용자 정보 기록

ACA-PY에서 사용하는 모든 프로토콜들은 객체 생성 시 Profile 정보를 요구하며 모든 요청에 Profile 정보를 참조하여 정보를 가져와 메시지를 만든다.

Profile은 실행 시 설정되는 Config 값들을 가져와 만들어진다.

## messaging

 : ACA-PY 메시지의 기본 베이스 정의

ACA-PY에서 사용하는 메시지들은 모두 ‘base_message.py’를 상속해 사용하여 messagin에 있는 코드들은 ‘base_message.py’를 기반으로 하는 에러, 핸들러 등의 기능들을 구현한다.

base_message.py

 : ACA-PY에서 사용하는 모든 메시지들의 부모 클래스로 가상화되어 있다. ACA-PY에서 메시지를 정의하기 위해선 해당 클래스를 베이스로 사용한다.

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

## protocols

 : Aries 주요 Protocol 기능

실제 ACA-PY 실행 시 나오는 API들이 해당 폴더에 저장되어 있으며 각각 내부의 기능들은 다른 폴더 기능들을 가져와 사용한다.

기본적으로 모든 Protocol들은 core의 Profile 정보를 가져와 객체를 생성하며 

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

manager.py : ‘aries_cloudagent/connections/base_manager.py’의 ‘BaseConnectionManager’ 클래스 기반으로 동작하여 이때 DID Document 값을 받는다.

create_invitation : 이 상호 작용은 대역 외 통신 채널을 나타냅니다. 미래에는 실제로 이러한 종류의 초대가 SMS, 이메일, QR 코드, NFC 등과 같은 여러 채널을 통해 수신될 것입니다.

Aries RFC 0160 Connection Protocol : [Hyperledger Aries protocol](https://github.com/hyperledger/aries-rfcs/tree/main/features/0160-connection-protocol)

### coordinate_mediation

 : 중재자 기능

에이전트 연결 사이의 중재자를 위한 기능을 제공해준다.

Aries RFC 0211 Mediator Coordination Protocol : [0211-route-coordination](https://github.com/hyperledger/aries-rfcs/tree/main/features/0211-route-coordination)

### didexchange

 : DID 교환

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

- > 현재 V1과 V2가 구현되어 있다.

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
    

issue_credential/v2_0/manager.py : 실제 프로토콜 실행을 위해 API들이 정리되어 있는 클래스

issue_credential/v2_0/messages/ : V2 issuer protocol에 사용되는 메시지 정리

issue_credential/v2_0/messages/

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

## wallet

 : 지갑 관련 기능 및 indy의 지갑 및 지갑 관련 기술 구현

멀티 테넌시(multitenant) 기능이 구현되어 있다. 멀티 테넌시는 다양한 사용자에 맞춰 앱을 구성하는 것이 아닌 하나의 앱을 사용자가 같이 사용하는 기능이다. 이는 개발 입장에서 한번의 업데이트 및 구성으로 일 처리를 할 수 있다는 장점이 있다.

indy-sdk 확인 가능 부분 : [https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/indy/sdk/wallet_setup.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/indy/sdk/wallet_setup.py)

## 시작순서

[https://github.com/hyperledger/aries-cloudagent-python](https://github.com/hyperledger/aries-cloudagent-python)

위 사이트의 ‘aries_cloudagent’에서 코드 상 순서 기재

main.py (run) -> command.__init__ (run_command) -> command.start (execute) -> core.conductor.py (setup, start)

self.root_profile, self.setup_public_did = await wallet_config(context)

context 값을 입력 받아 프로필 생성!

config/default_context.py

주요 암호화 및 복호화 부분 : [https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/askar/didcomm/v1.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/askar/didcomm/v1.py)