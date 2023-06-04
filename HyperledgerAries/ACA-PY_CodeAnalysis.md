# ACA-PY 코드 분석

ACA-PY의 코드 내용을 분석

Hyperledger Aries 클라우드 에이전트 - Python : [https://github.com/hyperledger/aries-cloudagent-python](https://github.com/hyperledger/aries-cloudagent-python)

주요 코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent)


## admin

: 서버 및 사이트 동작 코드

- server.py
    
    AdminServer.class (BaseAdminServer 상속)
    
    - HTTP 서버 관련 기능을 제공해주며 ACA-PY에선 Swagger를 사용한 HTTP API 환경을 제공해준다.
    - HTTP 서버를 통해 API를 호출하면 호출한 함수에 맞는 API를 찾아 실행시킨다. 
    - 주요 기능
        - additional_routes_pattern
        - _matches_additional_routes
        - make_application
        - start
        - stop
        - plugins_handler
        - config_handler
        - status_handler
        - status_reset_handler
        - liveliness_handler
        - readiness_handler
        - shutdown_handler
        - notify_fatal_error
        - websocket_handler
        - _on_webhook_event
        - _on_record_event
        - send_webhook

    AdminResponser.class (BaseResponder 상속)
    
    - 작성 중
    - 주요 기능
        - send_outbound
        - send_webhook

Aries의 경우 동작 시 Swagger API를 사용하며 중간 Agent로 애플리케이션에서 보내는 요청을 받아 처리하기 위한 서버가 필요하며 admin은 해당 부분을 구현

## askar

 : Aries용 보안 저장소

askar의 경우 Hyperledger Aries Agent를 위해 설계된 보안 스토리지 및 암호화를 지원해주는 프로젝트이다.

<사이트> : [https://github.com/hyperledger/aries-askar](https://github.com/hyperledger/aries-askar)

## cache

 : Aries의 메인 기능 구현

- base.py
    
    BaseCache.class (가상 클래스)
    
    - cache 사용을 위한 인터페이스 정의
    - 멤버 변수
        - _key_locks: {}
    - 맴버 함수
        - get: (가상 함수) 
        - set: (가상 함수) 
        - clear : (가상 함수) 
        - flush : (가상 함수) 
        - acquire : 
        - release : 
        - repr : 
    
    CacheKeyLock.class 

    - 특정 캐시 키에 대한 잠금
    - 여러 비동기 스레드가 동일한 약간 비싼 데이터를 생성하거나 쿼리하는 것을 방지하는 데 사용됩니다. 스레드로부터 안전하지 않습니다.
    - 멤버 변수
        - cache: BaseCache
        - exception: BaseException
        - key: Text
        - released: bool
        - _future:
        - _parent:
    - 맴버 함수
        - done(self) -> bool: 

## commands

 : 명령어별 기능 코드

Aries 스크립트에 전달하는 명령어를 처리하는 코드로 밑에 작성된 Hyperledger Aries 명령어를 처리한다.

## connections

 : 연결을 위한 초대장 생성 및 연결 기능 구현

Aries는 초대장을 통한 통신으로 채널을 생성해 연결하며 이부분에 대한 기능이 작성되어 있다. 

- [base_manager.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/connections/base_manager.py)
    
    BaseConnectionManager.class
    
    - 연결을 위한 기능들을 제공하며 이때 필요한 Key, DIDDoc 등의 정보를 조회한다.
    - Connection, DIDExchange, OutOfBand Manager에 사용되는 기본 매니저
    - 멤버 변수
        - _logger: logger
        - profile: Profile
        - route_manager: RouteManager
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


- models

    - [conn_record.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/connections/models/conn_record.py)

        ConnRecord.class (BaseRecord 상속)
        - Connection과 관련된 내용 저장을 위한 클래스
        - 멤버 변수
            - connection_id: str


## [core](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/core)

 : Aries의 메인 기능 구현

Aries의 메인 기능들이 구현되어 있다. 구현되어 있는 각각들의 기능들에 명령 및 관리를 담당하며 그외 데이터 관리, 스레드 생성 등 Agent의 메인에 해당하는 기능들이 구현되어 있다.

- [profile.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/core/profile.py)
    
    Profile.class (가상 클래스)
    
    - ID 관련 상태 처리를 위한 기본 추상화, ACA-PY에서 사용하는 모든 프로토콜들은 객체 생성 시 Profile 정보를 요구한다.
    - Profile은 실행 시 설정되는 Config 값들을 가져와 만들어지며 이때 InjectionContext 값을 사용한다. (InjectionContext는 Config에 존재)
    - Profile은 사용자의 정보를 가지고 있다 필요할 때마다 설정 정보를 가져와 전달한다.
    - 데이터의 변환은 일어나지 않으며 데이터 조회만 가능하다.
    - 멤버 변수
        - context: InjectionContext
        - name: str
        - created: bool
    - 맴버 함수
        - session: (가상 함수) 트랜잭션 지원이 요청되지 않은 새 대화식 세션 시작
        - transaction: (가상 함수) 커밋 및 롤백 지원으로 새로운 대화형 세션을 시작
        - inject : 주어진 클래스 식별자의 인스턴스를 제공
    
    ProfileManager.class (가상 클래스)
    
    ProfileSession.class (가상 클래스)
    - 프로필 관리 및 연결을 활성한다.
    - ACA-PY는 연결마다 Session을 만들어 관리하며 연결에 필요한 객체들을 InjectionContext에서 관리한다.
    - 멤버 변수
        - context: InjectionContext
        - profile: Profile
        - active: Bool
        - awaited: Bool
        - entered: 0
    
    ProfileManagerProvider.class
    
    - BaseProvider를 상속하며 이는 Config의 base.py에 정의되어 있다.


- in_memory : 여러 개의 Profile을 관리하기 위한 기능 폴더

    - [profile.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/core/in_memory/profile.py)

        InMemoryProfile.class (Profile 상속)

        - 여러 개의 Profile 관리 기능을 가지고 있으며 대부분 테스트에 사용
        - 멤버 변수
            - keys: {}
            - local_dids: {}
            - pair_dids: {}
            - records: OrderedDict
        - 맴버 함수
            - test_profile: 테스트를 위한 기본 InMemoryProfile을 만들때 사용
            - test_session: 테스트용 InMemoryProfileSession을 만들때 사용

        InMemoryProfileSession.class (ProfileSession 상속)

        - ProfileSession을 구현한 클래스

        InMemoryProfileManager.class (ProfileManager 상속)
        

- oob_processor.py : Oot of band message 기능

    OobMessageProcessor.class 

    - Out of band message processor
    - 멤버 변수
        - inbound_message_router: Callable[Profile, InboundMessage, Optional[bool]]
    - 맴버 함수
        - clean_finished_oob_record


## config

 : 사용자 정보 설정

- [injection_context.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/config/injection_context.py)
    
    InjectionContext.class (BaseInjector 상속)
    
    - 설정 값과 클래스 제공자를 관리하는 클래스
    - scope_name과 Injector를 연결시켜 관리한다.
    - 멤버 변수
        - injector: Injector
        - scope_name: str
        - scopes: [] (배열)

- [injector.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/config/injector.py)
    
    Injector.class (BaseInjector 상속)
    
    - 정적 및 동적 바인딩을 사용한 인젝터 구현
    - 클래스와 객체를 묶어 provider 리스트에 저장한 뒤, 특정 클래스의 객체 요청이 들어오면 요청에 맞는 클래스를 provider 리스트에서 찾아 전달한다.
    - 멤버 변수
        - enforce_type: bool
        - _providers: {}
        - _settings: Setting
    - 멤버 함수
        - bind_instance: 정적 객체를 클래스와 함께 저장, 'InstanceProvider'를 사용해 클래스 네임과 객체를 Provider 형태로 저장한다.
        - bind_provider: 생성자를 지정한다. 기본 'InstanceProvider'를 사용해 객체를 제공하나 이를 임의의 Provider로 지정할 수 있다. 
        - inject: 클래스는 해당 함수를 사용해 객체를 요청하며 이때 필요한 클래스 이름을 입력 값으로 사용한다. 'indject_or'을 실행한다.
        - indject_or: 제공된 클래스 식별자의 인스턴스를 가져오며 이때 Provider를 사용한다. 

- settings.py

    Settings.class (BaseSettings, MutableMapping[str, Any] 상속)

    - 변경 가능한 설정 구현
    - 멤버 변수
        - values: Optional[Mapping[str, Any]]
    - 멤버 함수
        - get_value (*var_name, default): 
        - set_value (var_name, value): 
        - set_default 
        - clear_value
        - contains
        - copy
        - extend
        - update
        - for_plugin

- [provider.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/config/provider.py)

    InstanceProvider.class (BaseProvider 상속)
    - 객체 공급자
    - Provider는 Injector를 통해 사용되며 일관된 객체 공급자 인터페이스를 위해 사용한다.
    - 멤버 변수
        - instance: instance
    - 멤버 함수
        - provide: 설정 및 injector가 지정된 객체 제공

    ClassProvider.class (BaseProvider 상속)

    CachedProvider.class (BaseProvider 상속)

- [base.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/config/base.py)

    BaseSettings.class (가상 클래스)
    
    - 기본 Settings 사용을 위한 클래스로 인터페이스 선언
    - Setting은 프로그램에 필요한 환경 변수 및 다양한 설정 값들을 관리하기 위해 사용한다.
    - 멤버 함수
        - get_value
        - get_bool

    BaseInjector.class (가상 클래스)
    
    - 기본 Injector 사용을 위한 클래스로 인터페이스 선언
    - Injector는 프로그램 실행 당시 생성된 객체들을 저장하여 리스트 형태로 저장한 뒤 Provider를 통해 객체를 찾아 제공한다. 이는 각각의 분리되어 있는 기능들의 연결을 위해 사용된다.
    - 멤버 함수
        - inject (Type[InjectType], Optional[Mapping[str, Any]]) -> InjectType: 주어진 클래스 식별자의 제공된 객체를 가져온다.
        - inject_or (Type[InjectType], Optional[Mapping[str, Any]], Optional[InjectType]) -> Optional[InjectType]: 주어진 클래스 식별자의 제공된 객체를 가져오며 없으면 기본 값을 가져온다.

    BaseProvider.class (가상 클래스)
    
    - 기본 Provider 사용을 위한 클래스로 인터페이스 선언
    - Provider는 객체 제공을 위한 동일한 인터페이스 정의를 통해 어떤 객체를 요구하더라도 일관된 인터페이스를 제공한다.
    - 멤버 함수
        - provider (BaseSetting, BaseInjector): 객체 인스턴스 제공 기능

- [base_context.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/config/base_context.py)

    ContextBuilder.class (가상 클래스)
    
    - 기본 Context 사용을 위한 클래스로 인터페이스 선언
    - Context는 설정들을 관리하는 객체로 로컬에 저장된 특정 파일 경로 값이나 환경 설정 값을 가진다.
    - 멤버 변수 
        - settings: Settings (Optional[Mapping[str, Any]])
    - 멤버 함수
        - build_context(self) -> InjectionContext: 
        - update_settings(self, settings: Mapping[str, object]): 


- [default_context.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/config/default_context.py)

    DefaultContextBuilder.class (ContextBuilder 상속)

    - 기본 Context 생성자
    - 멤버 함수
        - build_context(self) -> InjectionContext: 기본 주입 컨텍스트를 빌드합니다. 내보낼 DIDComm 접두사를 설정합니다.
            - context.injector.bind_instance를 통해 Context 사용을 위한 객체들 저장, 이때 저장하는 객체들은 기본 값들을 설정한다.
            - 사용 객체 : BaseCache, ProtocolRegistry, GoalCodeRegistry, EventBus, DIDResolver, DIDMethods, KeyType
        - bind_providers(self, context): 다양한 클래스 공급자를 정의한다.
        - load_plugins(self, context): 플러그인 저장소를 정의하고 가져온다. 


## messaging

 : ACA-PY 메시지의 기본 베이스 정의

ACA-PY에서 사용하는 메시지들은 모두 ‘base_message.py’를 상속해 사용하여 messagin에 있는 코드들은 ‘base_message.py’를 기반으로 하는 에러, 핸들러 등의 기능들을 구현한다. 

- [base_message.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/messaging/base_message.py)
    BaseMessage.class (가상클래스)
    - ACA-PY에서 사용하는 메시지의 최소 메시지, 어떤 방식으로든 확장이 가능하다. 
    - 멤버 변수
        - type: str
        - id: str
        - thread_id: Optional[str]
        - Handler: Type["BaseHandler"]
    - 멤버 함수
        - serialize: 
        - deserialize

- models

    - base.py
        BaseModel.class (가상클래스)
        - 편리한 기능 제공을 위한 기본 모델, json 클래스를 json 파일로 변환하거나 json 파일 검증 등의 기능을 제공

        BaseModelSchema.class (Schema 상속)
        - BaseModel의 Schema 관리를 위한 클래스

    - base_record.py
        BaseRecord.class (가상클래스)
        - 기본 저장소 기반 기록 관리를 위한 가상 클래스, 

- agent_message.py

    AgentMessage.class (BaseModel, BaseMessage 상속)
    - 상대방에게 전달하기 위한 기초적인 정보가 담긴 메시지와 기능들을 제공해준다. 
    - 모든 메시지들은 AgentMessage를 기반으로 속성 값을 추가하여 메시지를 만든다.
    - 멤버 변수 
        - _id: str
        - _type: Optional[Text]
        - _version: Optional[Text]
        - _decorators: BaseDecoratorSet
        - handler_class
        - schema_class 
        - message_type
    - 멤버 함수
        - _get_handler_class

    AgentMessageSchema.class
    - AgentMessage의 속성들을 관리하는 클래스
    - 멤버 변수 
        - model_class:
        - signed_fields:
        - unknown:
        - _type:
        - _id:
        - _decorators: DecoratorSet
        - _decorators_dict:
        - _signatures: {}

- responder.py

    BaseResponder.class (가상클래스)

    - 응답자는 처리 중인 메시지에 대한 응답으로 새 메시지를 보낼 수 있도록 메시지 처리기에 제공됩니다.
    - 메시지 응답을 위한 인터페이스를 정의한다.
    - 멤버 변수 
        - connection_id: str
        - reply_session_id: str
        - reply_to_verkey: str
    - 멤버 함수
        - create_outbound(self, message, ) -> OutboundMessage : OutboundMessage를 만든다. 
            - 사용 객체 : BaseCache, ProtocolRegistry, GoalCodeRegistry, EventBus, DIDResolver, DIDMethods, KeyType
        - send(self, message, ) -> OutboundSendStatus : 특정 메시지를 OutboundMessage로 변환한 뒤, 보낸다.
        - send_reply(self, message, )
        - conn_rec_active_state_check
        - send_outbound
        - send_webhook

    MockResponder.class (BaseResponder 상속)

    - 테스트를 위한 Responder 클래스
    - 멤버 변수 
        - message: [] 
    - 멤버 함수
        - send
        - send_reply
        - send_outbound
        - send_webhook

## did

 : did-key와 관련된 기능 구현

did 관련 key 생성에 중점을 두고 있으며 실제 did 생성과 관련된 기능들은 대부분 indy에 있다.

## indy

 : Hyperledger Aries에서 사용하는 indy 관련 기능 구현

Hperledger Indy가 가지고 있는 DID 관련 기능(지갑 생성, VC 생성 등)이 구현되어 있다.

- sdk
    
     : indy-sdk 기능이 요구되는 Aries 주요 기능들 구현
    
    - [profile.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/indy/sdk/profile.py)
    
        IndySdkProfile.class (Profile 상속)
    
        - Indy 기반의 Profile 설정을 지원한다. 

    - [wallet_setup.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/indy/sdk/wallet_setup.py
)
    
        IndyWalletConfig.class
    
        - Indy 기반의 wallet을 가져오기 위한 값들을 설정한다.
        - 해당 정보 기반으로 IndyOpenWallet 객체를 만들어 wallet 정보를 가져와 사용한다.
        - 멤버 변수
            - config: Mapping[str, Any]

        IndyOpenWallet.class

        - Indy 기반의 wallet 값을 가져온다.
        - 멤버 변수
            - config: IndyWalletConfig
            - created
            - handle
            - master_secret_id: str

## ledger

 : 원장 관련 기능 구현

Hperledger Indy가 가지고 있는 원장 관련 기능이 구현되어 있다. indy-sdk의 기능인 pool 연결, 트랜잭션 생성 및 전송, pool 설정 요청 등이 있다.

## multitenant

 : 동시에 여러 요청 수행

멀티 테넌시(multitenant) 기능이 구현되어 있다. 멀티 테넌시는 다양한 사용자에 맞춰 앱을 구성하는 것이 아닌 하나의 앱을 사용자가 같이 사용하는 기능이다. 이는 개발 입장에서 한번의 업데이트 및 구성으로 일 처리를 할 수 있다는 장점이 있다.

다중 지갑 :

[https://github.com/hyperledger/aries-cloudagent-python/blob/main/Multitenancy.md](https://github.com/hyperledger/aries-cloudagent-python/blob/main/Multitenancy.md)

---
# [protocols]((https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols))

 : Aries 주요 Protocol 기능

실제 ACA-PY 실행 시 나오는 API들이 해당 폴더에 저장되어 있으며 각각 내부의 기능들은 다른 폴더 기능들을 가져와 사용한다.

기본적으로 모든 Protocol들은 core의 Profile 정보를 가져와 객체를 생성하며 manager를 통해 해당 Protocol 기능을, message를 통해 Protocol에 사용하는 메시지를 정의한다.

Aries RFC 0003 Protocols : [https://github.com/hyperledger/aries-rfcs/tree/main/concepts/0003-protocols](https://github.com/hyperledger/aries-rfcs/tree/main/concepts/0003-protocols)

## actionmenu

 : 서버 조정 관련 기능

Aries RFC 0509 Action Menu Protocol : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0509-action-menu](https://github.com/hyperledger/aries-rfcs/tree/main/features/0509-action-menu)

## basicmessage

 : 기본 메시지 보내기

‘connections’을 통한 다른 에이전트와의 연결 이후 일반 메시지 전송을 위한 기능을 제공

Aries RFC 0095 Basic Message Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0095-basic-message](https://github.com/hyperledger/aries-rfcs/tree/main/features/0095-basic-message)

## connections

 : 다른 에이전트와의 연결 생성

다른 에이전트와의 통신을 위해 제일 처음 실행하는 부분으로 초대장을 만들거나 읽어 상대방과 통신을 위한 peer did 생성 및 연결이 이루어진다.

- [manager.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/protocols/connections/v1_0/manager.py)
    
    ConnectionManager.class (BaseConnectionManager 상속)
    
    - ‘aries_cloudagent/connections/base_manager.py’의 ‘BaseConnectionManager’ 클래스를 상속
    - 객체 생성 시 Profile 값을 가져와 생성 (core의 Profile 확인)
    - 멤버 함수
        - create_invitation : 연결을 위한 초대장 생성
        - receive_invitation : 초대장 확인 및 저장
        - create_request : 초대장에 대한 연결 요청 메시지 작성 및 전송
        - receive_request : 연결 요청 메시지 확인 및 저장
        - create_response : 연결 요청 메시지 대한 연결 응답 메시지 작성
        - accept_response : 연결 응답 메시지 확인 및 저장
        - get_endpoints : 특정 연결에 대한 endpoint 조회
        - create_static_connection
        - find_connection : 연결 조회
        - find_inbound_connection
        - resolve_inbound_connection
        - get_connection_targets
        - establish_inbound
        - update_inbound
    - [테스트 코드](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/protocols/connections/v1_0/tests/test_manager.py)
        
- message_types.py
    - 메시지에 사용할 설정 및 고정 값들 정의 (type, version 등)
    
- message
    
    Connection에서 사용하는 메시지 정의
    
    - connection_invitation.py
        
         : ConnectionInvitation.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 Connection Invitation 메시지에 필요한 값들을 추가 호출 후 적용
        
         : ConnectionInvitationSchema.class (AgentMessageSchema 상속)
        
        - Connection Invitation 메시지의 속성 값들 정의
        
    - connection_request.py
        
        ConnectionRequest.class (AgentMessage 상속)
        - 기존의 AgentMessage에 Connection Request 메시지에 필요한 값들을 추가 호출 후 적용
        
        ConnectionRequestSchema.class (AgentMessageSchema 상속)
        - Connection Request 메시지의 속성 값들 정의
        
    - connection_response.py
        
         : ConnectionResponse.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 Connection Response 메시지에 필요한 값들을 추가 호출 후 적용
        
         : ConnectionResponseSchema.class (AgentMessageSchema 상속)
        
        - Connection Response 메시지의 속성 값들 정의
        

Aries RFC 0160 Connection Protocol : [Hyperledger Aries protocol](https://github.com/hyperledger/aries-rfcs/tree/main/features/0160-connection-protocol)

## coordinate_mediation

 : 서로 다른 에이전트 사이의 연결을 관리한다.

- [route_manager.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/protocols/coordinate_mediation/v1_0/route_manager.py)

    RouteManager.class (가상 클래스)
    - RouteManager 인터페이스를 위한 기본 클래스
    - 멤버 함수
        - get_or_create_my_did: 연결을 위한 DID 정보 생성 또는 검색
        - mediation_record_if_id: 중개 레코드 상태의 유효성을 검사하고 레코드를 반환하고 그렇지 않으면 None을 반환합니다.
        - mediation_record_for_connection: 연결을 위한 관련 중재자 반환
        - route_connection: 연결을 위한 라우팅을 설정합니다.

    CoordinateMediationV1RouteManager.class (RouteManager 상속)
    - Coordinate Mediation 프로토콜을 사용하여 경로 관리
    - 멤버 함수
        - routing_info: 

- [manager.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/protocols/coordinate_mediation/v1_0/manager.py)

    MediationManager.class
    - MediationManager는 일관된 라우팅 키를 중재 클라이언트에 전달하는 수단으로 라우팅 DID를 생성하거나 검색합니다.
    - 멤버 변수
        - profile: Profile
    - 멤버 함수
        - update_keylist: 

- [route_manager_provider.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/protocols/coordinate_mediation/v1_0/route_manager_provider.py)
    
    RouteManagerProvider.class (BaseProvider 상속)
    - 설정에 따라 사용할 경로 관리자를 결정합니다.
    - 다중 연결(multitenant)을 사용할 때 각 연결에 따른 RouteManager를 제공한다.
    - 멤버 변수
        - root_profile: Profile
    - 멤버 함수
        - provide: 적절한 경로 관리자 인스턴스 생성

- models
    - [mediation_record.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/protocols/coordinate_mediation/v1_0/models/mediation_record.py)

        MediationRecord(BaseRecord 상속)
        - 저장된 중개 정보를 나타내는 클래스
        - 멤버 변수
            - connection_id: str
            - state: str
            - role: str

Aries RFC 0211 Mediator Coordination Protocol : [0211-route-coordination](https://github.com/hyperledger/aries-rfcs/tree/main/features/0211-route-coordination)

## didexchange

 : 상대방과 DID를 교환하여 연결을 만드는 기능을 제공해준다.

- manager.py
    
     : DIDXManager.class (BaseConnectionManager 상속)
    
    - ‘aries_cloudagent/connections/base_manager.py’의 ‘BaseConnectionManager’ 클래스를 상속
    - 객체 생성 시 Profile 값을 가져와 생성 (core의 Profile 확인)
    - 멤버 함수
        - receive_invitation : 초대장을 받아 새로운 연결을 확인 및 저장한다.
        - create_request_implicit : 공개 DID에 대해서만 연결 요청 메시지 작성 및 전송
        - create_request : 초대장에 대한 요청 메시지를 작성 및 전송
        - receive_request : 요청 메시지 확인 및 저장
        - create_response : 요청에 대한 응답 메시지를 작성 및 전송
        - accept_response : 응답 메시지 확인 및 저장
        - accept_complete : 연결 완료 메시지를 확인한다.
        - verify_diddoc : diddoc 내용 및 서명 확인 
        - get_resolved_did_document : diddoc 문서 조회
        - get_first_applicable_didcomm_service : 
    
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
        
         : DIDXComplete.class (AgentMessage 상속)
        
        - 기존의 AgentMessage에 DID Exchange Request 메시지에 필요한 값들을 추가 호출 후 적용
        
         : DIDXCompleteSchema.class (AgentMessageSchema상속)
        
        - DID CompleteSchema Request 메시지의 속성 값들 정의
        
    

코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/didexchange](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/didexchange)

Aries RFC 0023 DID Exchange Protocol 1.0 : [RFC 0023](https://github.com/hyperledger/aries-rfcs/tree/main/features/0023-did-exchange)

## discovery

 :

Aries RFC 0557 Discover Features Protocol 2.x : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0557-discover-features-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0557-discover-features-v2)

## endorse_transaction

 :

## introduction

 : 자기소개 기능

Aries RFC 0028 Introduce Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0028-introduce](https://github.com/hyperledger/aries-rfcs/tree/main/features/0028-introduce)

## issue_credential

 : VC의 생성 발급 저장 기능

Aries에서 제공하는 issuer 관련 기능들이 구현되어 있으며 크게 자격 증명 제안, 확인, 발급, 답장 등의 기능을 수행할 수 있다.

manager.py : 실제 프로토콜 실행을 위해 API들이 정리되어 있는 클래스

models/cred_ex_record.py : issue_credential에서 발생하는 메시지를 저장하기 위한 클래스 (/messaging/models/base_record.py를 상속해 사용)

messages/ : issue_credential v2에 사용되는 메시지 포멧 정리 

messages/cred_format.py : issue_credential v2의 메시지의 기반이 되는 메시지 포멧을 지원한다. (/messaging/models/base.py를 상속해 사용)

messages/cred_proposal.py : issue_credential v2의 Proposal 메시지 포멧(/messaging/agent_message.py를 상속해 사용)

messages/cred_offer.py

코드 링크 : [https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/issue_credential](https://github.com/hyperledger/aries-cloudagent-python/tree/main/aries_cloudagent/protocols/issue_credential)

Aries RFC 0453 Issue Credential Protocol 2.0 : [0453-issue-credential-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0453-issue-credential-v2)

## notification

 :

Aries RFC 0734 Push Notifications fcm Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0734-push-notifications-fcm](https://github.com/hyperledger/aries-rfcs/tree/main/features/0734-push-notifications-fcm)

## out_of_band

 :

Aries RFC 0434  2.0 Out-of-Band Protocol 1.1 : [0434-outofband](https://github.com/hyperledger/aries-rfcs/tree/main/features/0434-outofband)

## present_proof

 : VP의 생성 발급 저장 기능

Aries RFC 0454 Present Proof Protocol 2.0 : [0454-present-proof-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0454-present-proof-v2)

## problem_report

 : 에러 메시지 기능

Aries RFC 0035 Report Problem Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0035-report-problem](https://github.com/hyperledger/aries-rfcs/tree/main/features/0035-report-problem)

## revocation_notification

 : 폐기 확인 기능

Aries RFC 0721 Revocation Notification 2.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0721-revocation-notification-v2](https://github.com/hyperledger/aries-rfcs/tree/main/features/0721-revocation-notification-v2)

## routing

 :

## trustping

 : 에이전트 간 신뢰 핑

Aries RFC 0048 Trust Ping Protocol 1.0 : [https://github.com/hyperledger/aries-rfcs/tree/main/features/0048-trust-ping](https://github.com/hyperledger/aries-rfcs/tree/main/features/0048-trust-ping)

---

## transport

 : 연결을 위한 inbound 및 outbound 기능

- inbound
    - [receipt.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/transport/inbound/receipt.py)

        MessageReceipt.class
        - 에이전트 메시지 전달의 속성입니다.
        - 멤버 변수
            - connection_id: str
            - direct_response_mode: str
            - in_time: datetime
            - raw_message: str


## wallet

 : 지갑 관련 기능 및 indy의 지갑 및 지갑 관련 기술 구현

멀티 테넌시(multitenant) 기능이 구현되어 있다. 멀티 테넌시는 다양한 사용자에 맞춰 앱을 구성하는 것이 아닌 하나의 앱을 사용자가 같이 사용하는 기능이다. 이는 개발 입장에서 한번의 업데이트 및 구성으로 일 처리를 할 수 있다는 장점이 있다.

- [base.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/base.py)
    
    BaseWallet.class (가상 클래스)
    
    - wallet Interface를 정의한다.
    - 객체 생성 시 Profile 값을 가져와 생성 (core의 Profile 확인)
    - 기능의 대부분은 DID, Key와 관련되어 있으며 해당 정보들은 wallet 내부의 다른 클래스로 부터 가져온다. (did_info.py, key_type.py 등)
    - 멤버 함수
        - create_signing_key : 서명을 위한 키 쌍을 생성한다.
        - get_signing_key
        - replace_signing_key_metadata
        - rotate_did_keypair_start
        - rotate_did_keypair_apply
        - create_local_did
        - create_public_did
        - get_public_did
        - set_public_did
        - get_local_dids
        - get_local_did
        - get_local_did_for_verkey
        - replace_local_did_metadata
        - get_posted_dids
        - set_did_endpoint
        - sign_message
        - verify_message
        - pack_message
        - unpack_message

- [in_memory.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/in_memory.py)
    
    InMemoryWallet.class (BaseWallet 상속)
    
    - BaseWallet 인터페이스의 메모리 내 구현
    - 멤버 변수
        - profile
    
- [did_info.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/did_info.py)
    - KeyInfo, DIDInfo 정의
    - NamedTuple을 사용해 키와 타입을 정의
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
        
- [did_method.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/did_method.py)

    DIDMethod.class
    - DID Method 생성 클래스
    - DID Method란 did 뒤에 붙어 특정 기능을 가지고 있음을 알리는데 사용한다. (예시 : did 뒤에 key를 붙여 해당 did의 key임을 알림)
    - 멤버 변수
        - name: str
        - key_types: List[KeyType]
        - rotation: bool
        - holder_defined_did: HolderDefinedDid

    DIDMethods.class
    - 지원되는 키 유형으로 DID Method를 지정하는 DID Method 클래스
    - 멤버 변수
        - registry: Dict[str, DIDMethod] = {
            SOV.method_name: SOV,
            KEY.method_name: KEY,
        }


- [crypto.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/crypto.py)
    - BasicWallet에서 암호화에 필요한 함수들 제공
    - 클래스 없이 함수만을 정의
    - 멤버 함수
        - create_keypair : 서명을 위한 키 쌍을 생성한다.
        - create_ed25519_keypair
        - seed_to_did
        - rotate_did_keypair_start
        - rotate_did_keypair_apply
        - create_local_did
        - create_public_did
        - get_public_did

- [did_method.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/wallet/did_method.py
)

    DIDMethod.class

    - did method 정의
    - 멤버 변수
        - name: str
        - key_types: List[KeyType]
        - rotation: bool 
        - holder_defined_did: HolderDefinedDid (Enum)

    DIDMethods.class

    - 지원되는 키 유형으로 DID 메서드를 지정하는 DID 메서드 클래스
    - 멤버 변수
        - _registry: Dict[str, DIDMethod] {SOV.method_name: SOV, KEY.method_name: KEY}


## crypto.py

---
## 시작순서

‘aries_cloudagent’에서 코드 상 순서 기재

main.py (run) -> command.__init__ (run_command) -> command.start (execute) -> core.conductor.py (setup, start)

self.root_profile, self.setup_public_did = await wallet_config(context)

context 값을 입력 받아 프로필 생성!

config/default_context.py

주요 암호화 및 복호화 부분 : [https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/askar/didcomm/v1.py](https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/askar/didcomm/v1.py)