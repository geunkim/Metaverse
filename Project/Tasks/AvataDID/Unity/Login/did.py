"""
Example demonstrating Proof Verification.

First Issuer creates Claim Definition for existing Schema.
After that, it issues a Claim to Prover (as in issue_credential.py example)

Once Prover has successfully stored its Claim, it uses Proof Request that he
received, to get Claims which satisfy the Proof Request from his wallet.
Prover uses the output to create Proof, using its Master Secret.
After that, Proof is verified against the Proof Request
"""
import sys, os
import asyncio
import json
import pprint
from pathlib import Path

from indy import pool, ledger, wallet, did, anoncreds
from indy.error import ErrorCode, IndyError

import UnityEngine

os.add_dll_directory("D:\libindy_1.6.0\lib")

pool_name = 'pool'
issuer_wallet_config = json.dumps({"id": "issuer_wallet"})
issuer_wallet_credentials = json.dumps({"key": "issuer_wallet_key"})
genesis_file_path = Path.home().joinpath("pool_transactions_genesis")

async def proof_negotiation():
    try:

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

        # 22.
        UnityEngine.Debug.Log('\n22. Closing both wallet_handles and pool\n')
        await wallet.close_wallet(issuer_wallet_handle)

        # 23.
        UnityEngine.Debug.Log('\n23. Deleting created wallet_handles\n')
        await wallet.delete_wallet(issuer_wallet_config, issuer_wallet_credentials)

        # Send the generated DID back to Unity

        UnityEngine.Debug.Log(f'Generated DID: {did_json}')

        # Pass the DID value to Unity
        unity_handler = UnityHandler()
        unity_handler.SendDID(did_json)

    except IndyError as e:
        UnityEngine.Debug.Log('Error occurred: %s' % e)

def main():
    # os.add_dll_directory("D:\libindy_1.16.0\lib")
    UnityEngine.Debug.Log("START!!!")
    loop = asyncio.get_event_loop()
    loop.run_until_complete(proof_negotiation())
    loop.close()
    

class UnityHandler:
    def SendDID(self, did):
        # Call Unity function to receive the DID value
        UnityEngine.Debug.Log(f'Sending DID to Unity: {did}')
        UnityEngine.GameObject.FindObjectOfType<UserDID>().SetDID(did)

py_ver = int(f"{sys.version_info.major}{sys.version_info.minor}")
if py_ver > 37 and sys.platform.startswith('win'):
    asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())

main()
