from indy import did, wallet, pool, crypto, libindy

import json
import logging
import asyncio
import sys, os

def runtime_config():
    libindy.set_runtime_config('{"crypto_thread_pool_size": 2}')

def HelloWorld():
    print("Hello")

async def indy_test():
    """
    pool_config = json.dumps()
    pool_name = "sandbox"
    await pool.create_pool_ledger_config(pool_name, pool_config)

    pool_handle = await pool.open_pool_ledger(pool_name, None)
    """
    my_wallet_config = '{"id":"my_wallet"}'
    print(my_wallet_config)
    credentials = '{"key":"8dvfYSt5d1taSd6yJdpjq4emkwsPDDLYxkNFysFD2cZY"}'
    print(credentials)
    await wallet.create_wallet(my_wallet_config, credentials)
    my_wallet_handle = await wallet.open_wallet(my_wallet_config, credentials)
    
    (my_did, my_verkey) = await did.create_and_store_my_did(my_wallet_handle, "{}")
    print(my_did, my_verkey)

    pass

os.add_dll_directory("D:\libindy_1.16.0\lib")
HelloWorld()
#runtime_config()
asyncio.run(indy_test())