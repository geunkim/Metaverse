from multiprocessing import context
from unittest.mock import MagicMock, Mock

from aries_cloudagent.protocols.connections.v1_0.manager import ConnectionManager, ConnectionManagerError
from aries_cloudagent.protocols.connections.v1_0.messages.connection_invitation import ConnectionInvitation
from aries_cloudagent.protocols.connections.v1_0.messages.connection_request import ConnectionRequest
from aries_cloudagent.protocols.connections.v1_0.messages.connection_response import ConnectionResponse
from aries_cloudagent.protocols.connections.v1_0.models.connection_detail import ConnectionDetail

from aries_cloudagent.protocols.coordinate_mediation.v1_0.route_manager import RouteManager, CoordinateMediationV1RouteManager

from aries_cloudagent.connections.base_manager import BaseConnectionManager

from aries_cloudagent.core.profile import Profile, ProfileManagerProvider, ProfileSession
from aries_cloudagent.core.in_memory import InMemoryProfile, InMemoryProfileSession, InMemoryProfileManager

from aries_cloudagent.config.injection_context import InjectionContext

from aries_cloudagent.wallet.did_info import DIDInfo
from aries_cloudagent.wallet.did_method import DIDMethods
from aries_cloudagent.wallet.base import BaseWallet

from aries_cloudagent.indy.sdk.profile import IndySdkProfile
from aries_cloudagent.indy.sdk.wallet_setup import IndyWalletConfig, IndyOpenWallet
from aries_cloudagent.ledger.indy import IndySdkLedgerPool

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

    profile = InMemoryProfile.test_profile({},bind={})

    route_manager = CoordinateMediationV1RouteManager()

    context = profile.context
    context.injector.bind_instance(
            RouteManager, route_manager
        )
    context.injector.bind_instance(
            DIDMethods, DIDMethods()
        )

    connection = ConnectionManager(profile)

    connect_record, connect_invite = await connection.create_invitation()

    print(connect_invite)
    print(connect_record)
    print(context.__repr__)
    print(profile.__repr__)

    print("End Aries Test")
    pass

os.add_dll_directory("D:\libindy_1.16.0\lib")
HelloWorld()
#runtime_config()

loop = asyncio.get_event_loop()
loop.run_until_complete(aries_test())
loop.close
print("End Main")