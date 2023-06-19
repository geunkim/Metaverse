from aries_cloudagent.protocols.didcomm_prefix import DIDCommPrefix

from aries_cloudagent.protocols.connections.v1_0.manager import ConnectionManager, ConnectionManagerError
from aries_cloudagent.protocols.connections.v1_0.messages.connection_invitation import ConnectionInvitation
from aries_cloudagent.protocols.connections.v1_0.messages.connection_request import ConnectionRequest
from aries_cloudagent.protocols.connections.v1_0.messages.connection_response import ConnectionResponse
from aries_cloudagent.protocols.connections.v1_0.models.connection_detail import ConnectionDetail, ConnectionDetailSchema, DIDDocWrapper

from aries_cloudagent.protocols.didexchange.v1_0.messages.request import DIDXRequest

from aries_cloudagent.protocols.coordinate_mediation.v1_0.route_manager import RouteManager, CoordinateMediationV1RouteManager

from aries_cloudagent.connections.base_manager import BaseConnectionManager

from aries_cloudagent.core.profile import Profile, ProfileManagerProvider, ProfileSession
from aries_cloudagent.core.in_memory import InMemoryProfile, InMemoryProfileSession, InMemoryProfileManager
from aries_cloudagent.core.oob_processor import OobMessageProcessor

from aries_cloudagent.config.injection_context import InjectionContext

from aries_cloudagent.transport.inbound.receipt import MessageReceipt
from aries_cloudagent.transport.inbound.message import InboundMessage

from aries_cloudagent.wallet.did_info import DIDInfo
from aries_cloudagent.wallet.did_method import DIDMethods
from aries_cloudagent.wallet.base import BaseWallet
from aries_cloudagent.wallet.in_memory import InMemoryWallet

from aries_cloudagent.indy.sdk.profile import IndySdkProfile
from aries_cloudagent.indy.sdk.wallet_setup import IndyWalletConfig, IndyOpenWallet

from aries_cloudagent.ledger.indy import IndySdkLedgerPool

from aries_cloudagent.storage.base import BaseStorage
from aries_cloudagent.storage.in_memory import InMemoryStorage

import json
import logging
import asyncio
import sys, os

# 실행 되는지 확인을 위한 함수
def HelloWorld():
    print("Hello")

async def aries_test():

    test_seed = "testseed000000000000000000000001"
    test_did = "55GkHamhTU1ZbTbV2ab9DE"
    test_verkey = "3Dn1SJNPaCXcvvJvSbsFWP2xaCjMom3can8CQNhWrTRx"
    test_endpoint = "http://localhost"

    test_target_did = "GbuDUYXaUZRfHD2jeDuQuP"
    test_target_verkey = "9WCgWKUaAJj3VWxxtzvvMQN3AoFxoBtBDo9ntwJnVVCC"


    '''
    context = InjectionContext()
    context.injector.bind_instance(
            IndySdkLedgerPool, IndySdkLedgerPool("name")
        )

    wallet = await IndyWalletConfig(
            {
                "auto_recreate": True,
                "auto_remove": True,
                "key": await IndySdkWallet.generate_wallet_key(),
                "key_derivation_method": "RAW",
                "name": "test-wallet",
            }
        ).create_wallet()
    '''

    # 테스트를 위한 Profile
    profile1 = InMemoryProfile.test_profile(
        {
            "default_endpoint": "http://aries.ca/endpoint"
        }
        ,bind={})
    
    profile2 = InMemoryProfile.test_profile(
        {
            "default_endpoint": "http://aries.ca/endpoint"
        }
        ,bind={})

    # Connection 사용을 위해선 연결 정보를 기록하는 RouteManager가 필요
    route_manager = CoordinateMediationV1RouteManager()

    # Profile에 RouteManager 등록
    context1 = profile1.context
    context1.injector.bind_instance(
            RouteManager, route_manager
        )
    
    context2 = profile2.context
    context2.injector.bind_instance(
            RouteManager, route_manager
        )

    # Connection 사용을 위한 Manager 생성
    connection1 = ConnectionManager(profile1)
    connection2 = ConnectionManager(profile2)

    print("create_invitation")
    # Connection 프로토콜의 초대장 생성 실행
    # 결과: connect_record(ConnRecord 클래스), connect_invite(ConnectionInvitation 클래스)
    connect_record, connect_invite = await connection1.create_invitation()

    print("invite :", connect_invite)
    print("connect_record :", connect_record)
    print(context1.__repr__)
    print(profile1.__repr__)

    print("receive_invitation")
    # Connection 프로토콜의 초대장 받기 실행
    # 결과: connect_record(ConnRecord 클래스)
    # 사용시 End_point 값을 요구
    invitee_record = await connection2.receive_invitation(connect_invite)
    print(invitee_record)

    context1.injector.bind_instance(
            DIDMethods, DIDMethods()
        )
    context2.injector.bind_instance(
            DIDMethods, DIDMethods()
        )

    print("create_request")
    # Connection 프로토콜의 request 실행
    # 결과: connect_record(ConnRecord 클래스)
    # 사용시 DIDMethods 요구
    request = await connection2.create_request(invitee_record)
    print(request)

    receipt = MessageReceipt(recipient_verkey=connect_record.invitation_key)

    inboundMsg = InboundMessage("",receipt)
    oob = OobMessageProcessor(inboundMsg)

    context1.injector.bind_instance(
            OobMessageProcessor, oob
        )
    context2.injector.bind_instance(
            OobMessageProcessor, oob
        )

    print("receive_request")
    # Connection 프로토콜의 receive_request 실행
    # 결과: connect_record(ConnRecord 클래스)
    # 사용시 OobMessageProcessor, InboundMessage 요구
    request2 = await connection1.receive_request(request, receipt)
    print(request2)

    context1.injector.bind_instance(
            BaseStorage, InMemoryStorage(profile1)
        )
    
    async with profile1.session() as session:
        storage: BaseStorage = session.inject(BaseStorage)
        result = await storage.find_record(
            "connection_request", {"connection_id": request2.connection_id}
        )
        print("result : ", result)

        print("test request")
        test_request = await request2.retrieve_request(session)

    print("create_response")
    # Connection 프로토콜의 create_response 실행
    # 결과: response(ConnectionResponse 클래스)
    # 사용시 BaseStorage 요구
    response = await connection1.create_response(request2)
    print(response)

    # 20230612 : 문제 발생
    # create_response에서 'ConnectionRequest schema validation failed' 이라는 문제 발생
    # 또한 다음과 같은 에러 발생 'marshmallow.exceptions.ValidationError: {'label': ['Missing data for required field.']}'
    # https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/protocols/connections/v1_0/manager.py#L415
    # 위 링크는 'create_response' 함수 링크이며 함수 내의 'retrieve_request' 부분에서 에러 발생
    # retrieve_request 함수는 ConnRecord에서 사용 (https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/connections/models/conn_record.py#L469)
    # retrieve_request 함수의 마지막 부분인 return에서 deserialize 부분에서 에러 발생
    # deserialize는 BaseModel에서 사용 (https://github.com/hyperledger/aries-cloudagent-python/blob/main/aries_cloudagent/messaging/models/base.py#L171)
    # deserialize 함수의 마지막 부분인 cast 부분에서 에러 발생
    # cast는 python 패키지 중 하나인 typing 패키지에서 시용
    # InboundMessage("",receipt) -- 여기서 payload 값이 문제일 수 있음, 또한 입력 값의 문제일 수 있다.

    print("End Aries Test")
    pass

os.add_dll_directory("D:\libindy_1.16.0\lib")
HelloWorld()
#runtime_config()

loop = asyncio.get_event_loop()
loop.run_until_complete(aries_test())
loop.close
print("End Main")