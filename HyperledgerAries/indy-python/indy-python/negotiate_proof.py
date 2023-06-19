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
import aiohttp

from pathlib import Path
from indy import pool, ledger, wallet, did, anoncreds
from indy.error import ErrorCode, IndyError

'''
indy-python 테스트를 위한 코드
'''

# libindy 공유 라이브러리를 가져오기 위한 코드
# libindy 공유 라이브러리 파일의 경로 작성
os.add_dll_directory("D:\libindy_1.16.0\lib")

# 로컬에서 pool 정보 식별을 위한 이름 변수
# pool config 값의 이름이 되며 이는 "C:\Users\사용자이름\.indy_client\pool" 위치에 저장
# 본 코드는 config 파일 생성 후 삭제까지 하므로 확인을 위해선 'delete_pool_ledger_config' 함수를 실행하지 말것
# 또한 config 파일 이름은 겹칠 경우 에러를 발생하므로 기존 config 파일 조회 시 'open_pool_ledger'함수만 실행
pool_name = 'Issuer'

# 지갑 이름을 위한 변수
# 이미 같은 이름의 지갑이 있을 경우 'open_wallet' 함수만 실행
issuer_wallet_config = json.dumps({"id": "issuer_wallet"})

# 지갑 비번 변수
issuer_wallet_credentials = json.dumps({"key": "issuer_wallet_key"})

# genesis 파일 조회를 위한 url 변수
# 현재 von-network를 통한 http get 요청으로 genesis 정보를 가져온다.
genesis_file_url = "http://220.68.5.139:9000/genesis"

# genesis 파일 저장 및 사용 경로 변수
# 현재 genesis 정보는 'genesis_file_url'을 통해 정보를 가져온 뒤 해당 변수 위치에 저장하여 참조한다.
# Path.cwd()는 현재 프로그램이 실행 중인 디렉토리 경로
genesis_file_path = Path.cwd().joinpath('pool_transactions_genesis')

# log 확인 함수
def print_log(value_color="", value_noncolor=""):
    """set the colors for text."""
    HEADER = '\033[92m'
    ENDC = '\033[0m'
    print(HEADER + value_color + ENDC + str(value_noncolor))

# HTTP GET 요청을 통해 genesis 정보를 가져오는 함수
async def get_genesis_file_from_url(genesis_url):
    async with aiohttp.ClientSession() as session:
        async with session.request("GET",genesis_url) as resp:
                print(resp.status)
                print(await resp.text())
                return await resp.text()

# 가져온 genesis 정보를 파일로 저장하는 함수
async def save_pool_genesis_txn_file(path):
    data = await get_genesis_file_from_url(genesis_file_url)

    path.parent.mkdir(parents=True, exist_ok=True)

    with open(str(path), "w+") as f:
        f.writelines(data)

# 경로의 genesis 파일을 확인하는 함수
# 경로에 genesis 파일이 없는 경우 새로 생성한다.
async def check_genesis_file_from_path(path):
    if(not path.exists()):
        await save_pool_genesis_txn_file(path)

    return path

# 풀 연결 테스트 함수
async def pool_connect_test():
    try:
        print("=== check_genesis_file_from_path ===")
        
        path = await check_genesis_file_from_path(genesis_file_path)

        print("genesis_file_path")
        print(path)

        pool_config = json.dumps({'genesis_txn': str(path)})

        print("=== create_pool_ledger_config ===")
        try:
            await pool.create_pool_ledger_config(config_name=pool_name, config=pool_config)
        except IndyError as ex:
            if ex.error_code == ErrorCode.PoolLedgerConfigAlreadyExistsError:
                pass

        print(pool_config)

        print("=== open_pool_ledger ===")

        pool_handle = await pool.open_pool_ledger(config_name=pool_name, config=None)

        print("=== close_pool_ledger ===")

        await pool.close_pool_ledger(pool_handle)

        print("=== delete_pool_ledger_config ===")

        await pool.delete_pool_ledger_config(pool_name)

    except IndyError as e:
        print('Error occurred: %s' % e)

# python indy Test 함수
# VC, VP 과정이 모두 들어가 있다. 
async def proof_negotiation():
    try:
        # pool 버전 설정
        await pool.set_protocol_version(2)

        # genesis 파일 정보 가져오기
        path = await check_genesis_file_from_path(genesis_file_path)

        # 1.
        # genesis 파일을 통해 pool_config 파일을 만든다. 이때 pool_name을 통해 config 파일을 구분
        # config 파일은 기본 'C:\Users\사용자이름\.indy_client\pool' 경로에 저장되며 재사용 가능
        print_log('\n1. opening a new local pool ledger configuration that will be used '
                  'later when connecting to ledger.\n')
        pool_config = json.dumps({'genesis_txn': str(path)})
        try:
            await pool.create_pool_ledger_config(config_name=pool_name, config=pool_config)
        except IndyError as ex:
            if ex.error_code == ErrorCode.PoolLedgerConfigAlreadyExistsError:
                pass

        print_log(pool_config)

        # 2.
        # pool_config 파일을 통해 pool과 연결
        # pool에 이상이 있거나 genesis 파일 정보에 잘못된 정보가 있을 경우 해당 부분에서 오류가 발생한다.
        # 만약 오류 발생 시 pool이 제대로 돌아가고 있는지, genesis 정보에 틀린점이 없는지 확인 필요
        print_log('\n2. Open pool ledger and get the handle from libindy\n')
        pool_handle = await pool.open_pool_ledger(config_name=pool_name, config=None)

        # 3.
        # 지갑 이름, 비번을 통해 지갑을 만든다.
        # 기본 'C:\Users\사용자이름\.indy_client\wallet' 경로에 저장되며 재사용 가능
        print_log('\n3. Creating Issuer wallet and opening it to get the handle.\n')
        try:
            await wallet.create_wallet(issuer_wallet_config, issuer_wallet_credentials)
        except IndyError as ex:
            if ex.error_code == ErrorCode.WalletAlreadyExistsError:
                pass

        # 만들어진 지갑을 지갑 정보를 통해 조회한다. 
        issuer_wallet_handle = await wallet.open_wallet(issuer_wallet_config, issuer_wallet_credentials)

        # 4.
        # Steward 권한을 가진 DID를 생성한다.
        # Hyperledger Indy는 특정 권한이 있어야만 데이터 추가가 가능하므로 권한을 가진 DID가 필요하다.
        # 특정 DID 생성이 필요할 경우 seed 값을 통해 생성
        print_log('\n4. Generating and storing steward DID and verkey\n')
        steward_seed = '000000000000000000000000Steward1'
        did_json = json.dumps({'seed': steward_seed})
        steward_did, steward_verkey = await did.create_and_store_my_did(issuer_wallet_handle, did_json)
        print_log('Steward DID: ', steward_did)
        print_log('Steward Verkey: ', steward_verkey)

        # 5.
        # issuer가 사용할 did 생성
        # did_json과 같은 설정 값이 없을 경우 랜덤한 did 생성
        print_log('\n5. Generating and storing trust anchor DID and verkey\n')
        trust_anchor_did, trust_anchor_verkey = await did.create_and_store_my_did(issuer_wallet_handle, "{}")
        print_log('Trust anchor DID: ', trust_anchor_did)
        print_log('Trust anchor Verkey: ', trust_anchor_verkey)

        # 6.
        # 블록체인에 did를 저장하기 위한 transction 작성 (NYM 트랜잭션)
        # 권한을 가진 DID를 사용해 transction을 작성한다.
        print_log('\n6. Building NYM request to add Trust Anchor to the ledger\n')
        nym_transaction_request = await ledger.build_nym_request(submitter_did=steward_did,
                                                                 target_did=trust_anchor_did,
                                                                 ver_key=trust_anchor_verkey,
                                                                 alias=None,
                                                                 role='TRUST_ANCHOR')
        print_log('NYM transaction request: ')
        pprint.pprint(json.loads(nym_transaction_request))

        # 7.
        # 만들어진 transction을 전송
        print_log('\n7. Sending NYM request to the ledger\n')
        nym_transaction_response = await ledger.sign_and_submit_request(pool_handle=pool_handle,
                                                                        wallet_handle=issuer_wallet_handle,
                                                                        submitter_did=steward_did,
                                                                        request_json=nym_transaction_request)
        print_log('NYM transaction response: ')
        pprint.pprint(json.loads(nym_transaction_response))

        # 8.
        # Schema 생성
        # Issuer는 Schema를 기준으로 정보를 작성한다.
        print_log('\n8. Issuer create Credential Schema\n')
        schema = {
            'name': 'gvt',
            'version': '1.0',
            'attributes': '["age", "sex", "height", "name"]'
        }
        issuer_schema_id, issuer_schema_json = await anoncreds.issuer_create_schema(steward_did, 
                                                                                schema['name'],
                                                                                schema['version'],
                                                                                schema['attributes'])
        print_log('Schema: ')
        pprint.pprint(issuer_schema_json)

        # 9.
        # 블록체인에 Schema 저장을 위한 transction 작성 (Schema 트랜잭션)
        print_log('\n9. Build the SCHEMA request to add new schema to the ledger\n')
        schema_request = await ledger.build_schema_request(steward_did, issuer_schema_json)
        print_log('Schema request: ')
        pprint.pprint(json.loads(schema_request))

        # 10.
        # 만들어진 transction을 전송
        print_log('\n10. Sending the SCHEMA request to the ledger\n')
        schema_response = \
            await ledger.sign_and_submit_request(pool_handle,
                                                 issuer_wallet_handle,
                                                 steward_did,
                                                 schema_request)
        print_log('Schema response:')
        pprint.pprint(json.loads(schema_response))

        # 11.
        # Schema 기반의 Credential Definition 생성
        # Credential Definition는 VC 구조 정보로 어떤 속성 및 증명 정보를 지니는지를 정한다.
        # Issuer는 Credential Definition을 기반으로 값을 넣어 VC를 만든다.
        print_log('\n11. Creating and storing Credential Definition using anoncreds as Trust Anchor, for the given Schema\n')
        cred_def_tag = 'TAG1'
        cred_def_type = 'CL'
        cred_def_config = json.dumps({"support_revocation": False})

        (cred_def_id, cred_def_json) = \
            await anoncreds.issuer_create_and_store_credential_def(issuer_wallet_handle,
                                                                   trust_anchor_did,
                                                                   issuer_schema_json,
                                                                   cred_def_tag,
                                                                   cred_def_type,
                                                                   cred_def_config)
        
        print_log('Credential definition: ')
        pprint.pprint(json.loads(cred_def_json))

        # 12.
        # Holder 지갑 및 did 생성
        print_log('\n12. Creating Prover wallet and opening it to get the handle.\n')
        prover_did = 'VsKV7grR1BUE29mG2Fm2kX'
        prover_wallet_config = json.dumps({"id": "prover_wallet"})
        prover_wallet_credentials = json.dumps({"key": "prover_wallet_key"})

        try:
            await wallet.create_wallet(prover_wallet_config, prover_wallet_credentials)
        except IndyError as ex:
            if ex.error_code == ErrorCode.WalletAlreadyExistsError:
                pass

        prover_wallet_handle = await wallet.open_wallet(prover_wallet_config, prover_wallet_credentials)

        # 13.
        # Holder의 Master Secret 생성
        print_log('\n13. Prover is creating Link Secret\n')
        prover_link_secret_name = 'link_secret'
        link_secret_id = await anoncreds.prover_create_master_secret(prover_wallet_handle,
                                                                     prover_link_secret_name)

        # 14.
        # Issuer가 Holder 에게 전송하기 위한 Credential Offer 생성
        # Credential Definition 정보를 기반으로 생성
        # Credential Offer는 VC 발급 전 VC 모습을 미리 알리고자 보내는 내용으로 Indy의 경우 VC 발급을 위한 필수 과정이다.
        print_log('\n14. Issuer (Trust Anchor) is creating a Credential Offer for Prover\n')
        cred_offer_json = await anoncreds.issuer_create_credential_offer(issuer_wallet_handle,
                                                                         cred_def_id)
        print_log('Credential Offer: ')
        pprint.pprint(json.loads(cred_offer_json))

        # 15.
        # 받은 Credential Offer 기반의 Credential Request를 Holder가 생성
        # Credential Request는 Holder가 VC 발급을 위해 Issuer에게 VC를 요청하는 정보를 담는다.
        print_log('\n15. Prover creates Credential Request for the given credential offer\n')
        (cred_req_json, cred_req_metadata_json) = \
            await anoncreds.prover_create_credential_req(prover_wallet_handle,
                                                         prover_did,
                                                         cred_offer_json,
                                                         cred_def_json,
                                                         prover_link_secret_name)
        print_log('Credential Request: ')
        pprint.pprint(json.loads(cred_req_json))

        # 16.
        # Issuer가 받은 Credential Offer 기반의 Credential 생성 (VC 생성)
        print_log('\n16. Issuer (Trust Anchor) creates Credential for Credential Request\n')
        cred_values_json = json.dumps({
            "sex": {"raw": "male", "encoded": "5944657099558967239210949258394887428692050081607692519917050011144233"},
            "name": {"raw": "Alex", "encoded": "1139481716457488690172217916278103335"},
            "height": {"raw": "175", "encoded": "175"},
            "age": {"raw": "28", "encoded": "28"}
        })
        (cred_json, _, _) = \
            await anoncreds.issuer_create_credential(issuer_wallet_handle,
                                                     cred_offer_json,
                                                     cred_req_json,
                                                     cred_values_json, None, None)
        print_log('Credential: ')
        pprint.pprint(json.loads(cred_json))

        # 17.
        # Holder가 받은 Credential을 저장 (VC 저장)
        print_log('\n17. Prover processes and stores received Credential\n')
        await anoncreds.prover_store_credential(prover_wallet_handle, None,
                                                cred_req_metadata_json,
                                                cred_json,
                                                cred_def_json, None)

        # 18.
        # Verifier가 증명을 위한 Proof Request 생성
        # Proof Request는 Verifier가 Holder 증명을 위해 보내는 VP (증명을 위한 VC 변환 값) 요청이다.
        # Proof Request에는 증명을 위한 정보 및 증명에 사용할 VP의 Credential Definition 정보를 지닌다.
        # 해당 코드의 경우 Issuer가 Verifier 역할을 같이 진행
        print_log('\n18. Prover gets Credentials for Proof Request\n')
        proof_request = {
            'nonce': '123432421212',
            'name': 'proof_req_1',
            'version': '0.1',
            'requested_attributes': {
                'attr1_referent': {
                    'name': 'name',
                    "restrictions": {
                        "issuer_did": trust_anchor_did,
                        "schema_id": issuer_schema_id
                    }
                }
            },
            'requested_predicates': {
                'predicate1_referent': {
                    'name': 'age',
                    'p_type': '>=',
                    'p_value': 18,
                    "restrictions": {
                       "issuer_did": trust_anchor_did
                    }
                }
            }
        }
        print_log('Proof Request: ')
        pprint.pprint(proof_request)

        # 19. 
        # Holder는 받은 Proof Request 기반으로 VP 생성
        # VP는 이전에 받은 VC 기반으로 생성하며 Credential Definition 값을 통해 VC 조회, Schema 정보를 통해 필요한 정보를 조회한다.
        print_log('\n19. Prover gets Credentials for attr1_referent anf predicate1_referent\n')
        proof_req_json = json.dumps(proof_request)
        prover_cred_search_handle = \
            await anoncreds.prover_search_credentials_for_proof_req(prover_wallet_handle, proof_req_json, None)

        creds_for_attr1 = await anoncreds.prover_fetch_credentials_for_proof_req(prover_cred_search_handle,
                                                                                 'attr1_referent', 1)
        prover_cred_for_attr1 = json.loads(creds_for_attr1)[0]['cred_info']
        print_log('Prover credential for attr1_referent: ')
        pprint.pprint(prover_cred_for_attr1)

        creds_for_predicate1 = await anoncreds.prover_fetch_credentials_for_proof_req(prover_cred_search_handle,
                                                                                      'predicate1_referent', 1)
        prover_cred_for_predicate1 = json.loads(creds_for_predicate1)[0]['cred_info']
        print_log('Prover credential for predicate1_referent: ')
        pprint.pprint(prover_cred_for_predicate1)

        await anoncreds.prover_close_credentials_search_for_proof_req(prover_cred_search_handle)
        
        # 20.
        # Holder는 받은 Proof Request 기반의 Proof 생성
        # Proof는 Holder가 Proof Request 기반으로 만든 VP를 Verifier에게 전달하기 위해 만든다.
        print_log('\n20. Prover creates Proof for Proof Request\n')
        prover_requested_creds = json.dumps({
            'self_attested_attributes': {},
            'requested_attributes': {
                'attr1_referent': {
                    'cred_id': prover_cred_for_attr1['referent'],
                    'revealed': True
                }
            },
            'requested_predicates': {
                'predicate1_referent': {
                    'cred_id': prover_cred_for_predicate1['referent']
                }
            }
        })
        print_log('Requested Credentials for Proving: ')
        pprint.pprint(json.loads(prover_requested_creds))

        prover_schema_id = json.loads(cred_offer_json)['schema_id']
        schemas_json = json.dumps({prover_schema_id: json.loads(issuer_schema_json)})
        cred_defs_json = json.dumps({cred_def_id: json.loads(cred_def_json)})
        proof_json = await anoncreds.prover_create_proof(prover_wallet_handle,
                                                         proof_req_json,
                                                         prover_requested_creds,
                                                         link_secret_id,
                                                         schemas_json,
                                                         cred_defs_json,
                                                         "{}")
        proof = json.loads(proof_json)
        print_log('\nproof : ')
        pprint.pprint(proof)
        assert 'Alex' == proof['requested_proof']['revealed_attrs']['attr1_referent']["raw"]

        # 21.
        # Verifier는 받은 Proof의 VP를 검증한다.
        # 해당 검증은 본인이 받은 VP가 문제가 없는지 확인한다.
        # 받은 개인정보를 판단하는 것은 Verifier의 Aplication에서 진행한다.
        print_log('\n21. Verifier is verifying proof from Prover\n')
        assert await anoncreds.verifier_verify_proof(proof_req_json,
                                                             proof_json,
                                                             schemas_json,
                                                             cred_defs_json,
                                                             "{}", "{}")

        # 22.
        # handles 제거
        print_log('\n22. Closing both wallet_handles and pool\n')
        await wallet.close_wallet(issuer_wallet_handle)
        await wallet.close_wallet(prover_wallet_handle)
        await pool.close_pool_ledger(pool_handle)

        # 23.
        # 지갑 삭제
        # 지갑 재사용 및 파일 확인을 위해선 해당 부분을 실행해선 안된다.
        print_log('\n23. Deleting created wallet_handles\n')
        await wallet.delete_wallet(issuer_wallet_config, issuer_wallet_credentials)
        await wallet.delete_wallet(prover_wallet_config, prover_wallet_credentials)

        # 24.
        # 풀 삭제
        # 풀 재사용 및 파일 확인을 위해선 해당 부분을 실행해선 안된다.
        print_log('\n24. Deleting pool ledger config\n')
        await pool.delete_pool_ledger_config(pool_name)

    except IndyError as e:
        print('Error occurred: %s' % e)

def main():

    # python의 비동기 함수 사용을 위해 사용
    loop = asyncio.get_event_loop()

    # run_until_complete : 해당 비동기 함수가 끝날 때 까지 기다린다.
    loop.run_until_complete(proof_negotiation())

    #loop.run_until_complete(pool_connect_test())
    loop.close()

if __name__ == '__main__':
    main()

