from unittest.mock import Mock

from aries_cloudagent.protocols.connections.v1_0.manager import ConnectionManager
from aries_cloudagent.protocols.coordinate_mediation.v1_0.route_manager import RouteManager

from aries_cloudagent.connections.base_manager import BaseConnectionManager
from aries_cloudagent.core.profile import Profile, ProfileManagerProvider, ProfileSession
from aries_cloudagent.config.injection_context import InjectionContext
from aries_cloudagent.wallet.did_info import DIDInfo

import json
import logging
import asyncio
import sys, os

def HelloWorld():
    print("Hello")

class TestProfile(Profile):
    def session(self, context: InjectionContext = None) -> ProfileSession:
        """Start a new interactive session with no transaction support requested."""

    def transaction(self, context: InjectionContext = None) -> ProfileSession:
        """
        Start a new interactive session with commit and rollback support.

        If the current backend does not support transactions, then commit
        and rollback operations of the session will not have any effect.
        """

async def aries_test():

    profile = InMemoryProfile.test_profile(
            {
                "default_endpoint": "http://aries.ca/endpoint",
                "default_label": "This guy",
                "additional_endpoints": ["http://aries.ca/another-endpoint"],
                "debug.auto_accept_invites": True,
                "debug.auto_accept_requests": True,
            },
            bind={
                BaseResponder: responder,
                BaseCache: InMemoryCache(),
                OobMessageProcessor: self.oob_mock,
                RouteManager: route_manager,
                DIDMethods: DIDMethods(),
            },
        )

    profile = TestProfile()
    connection = ConnectionManager(profile)

    didinfo = DIDInfo()

    dd_in = {
            "@context": "https://w3id.org/did/v1",
            "id": "did:sov:LjgpST2rjsoxYegQDRm7EL",
            "publicKey": [
                {
                    "id": "3",
                    "type": "RsaVerificationKey2018",
                    "controller": "did:sov:LjgpST2rjsoxYegQDRm7EL",
                    "publicKeyPem": "-----BEGIN PUBLIC X...",
                },
                {
                    "id": "4",
                    "type": "RsaVerificationKey2018",
                    "controller": "did:sov:LjgpST2rjsoxYegQDRm7EL",
                    "publicKeyPem": "-----BEGIN PUBLIC 9...",
                },
                {
                    "id": "6",
                    "type": "RsaVerificationKey2018",
                    "controller": "did:sov:LjgpST2rjsoxYegQDRm7EL",
                    "publicKeyPem": "-----BEGIN PUBLIC A...",
                },
            ],
            "authentication": [
                {
                    "type": "RsaSignatureAuthentication2018",
                    "publicKey": "did:sov:LjgpST2rjsoxYegQDRm7EL#4",
                }
            ],
            "service": [
                {
                    "id": "0",
                    "type": "Agency",
                    "serviceEndpoint": "did:sov:Q4zqM7aXqm7gDQkUVLng9h",
                }
            ],
        }

    base.create_did_document()

    print("End Aries Test")
    pass

os.add_dll_directory("D:\libindy_1.16.0\lib")
HelloWorld()
#runtime_config()
asyncio.run(aries_test())
print("End Main")