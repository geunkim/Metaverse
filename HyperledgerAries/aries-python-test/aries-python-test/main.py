from aries_cloudagent.protocols.connections.v1_0.manager import ConnectionManager
from aries_cloudagent.connections.base_manager import BaseConnectionManager
from aries_cloudagent.core import profile
from aries_cloudagent.wallet.did_info import DIDInfo

import json
import logging
import asyncio
import sys, os

def HelloWorld():
    print("Hello")

async def aries_test():

    base = BaseConnectionManager()
    connection = ConnectionManager()

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

    pass

#os.add_dll_directory("D:\libindy_1.16.0\lib")
HelloWorld()
#runtime_config()
asyncio.run(aries_test())