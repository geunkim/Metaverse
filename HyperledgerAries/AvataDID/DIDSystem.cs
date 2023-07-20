using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

using Hyperledger.Indy.WalletApi;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.PoolApi;
using Hyperledger.Indy.LedgerApi;
using Hyperledger.Indy.CryptoApi;
using Hyperledger.Indy.AnonCredsApi;

public class DIDSystem : MonoBehaviour
{
    string defult_wallet_path;
    string wallet_config;
    string wallet_name;
    string wallet_credentials;

    Wallet wallet;
    Pool pool;

    string genesis_file_path = null;

    CreateAndStoreMyDidResult result;

    string did_seed = "issuer00000000000000000000000000";

    // Start is called before the first frame update
    void Start()
    {
        genesis_file_path = Application.dataPath + "/genesis.txn";        
        defult_wallet_path = Application.streamingAssetsPath + "/wallet";
        CreateWallet();
        SearchWallet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SearchWallet()
    {
        string[] wallet_list = Directory.GetDirectories(defult_wallet_path);
        foreach (string wallet in wallet_list)
        {
            Debug.Log(wallet);
        }
    }

    void CreateWallet()
    {
        wallet_name = "test_wallet" + UnityEngine.Random.Range(0, 1000).ToString();
        //string wallet_config = "{\"id\":\"" + wallet_name + "\"}";
        wallet_config = "{\"id\":\"" + wallet_name + "\", \"storage_config\": {\"path\": \"" 
            + defult_wallet_path + "\"}}";
        wallet_credentials = "{\"key\":\"wallet_key\"}";

        Debug.Log("wallet_config :" + wallet_config);

        Wallet.CreateWalletAsync(wallet_config, wallet_credentials).Wait();
    }

    void OpenWallet()
    {
        try
        {
            wallet = Wallet.OpenWalletAsync(wallet_config, wallet_credentials).Result;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    void CloseWallet()
    {
        try
        {
            wallet.CloseAsync().Wait();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    void CreateDidInWallet()
    {
        string did_json = "{\"seed\":\"" + did_seed + "\"}";

        try
        {
            result = Did.CreateAndStoreMyDidAsync(wallet, did_json).Result;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    void ConnectionPool()
    {
        if(false == File.Exists(genesis_file_path))
        {
            Debug.Log("Genesis File is Null");
            return;
        }

        string pool_name = "pool";
        string pool_config = "{\"genesis_txn\":\"" + genesis_file_path + "\"}";
        Debug.Log("Pool Config: " + pool_config);

        try
        {
            Debug.Log("Indy Create Pool Ledger Config");
            Pool.CreatePoolLedgerConfigAsync(pool_name, pool_config).Wait();

            Debug.Log("Indy Open Pool Ledger");
            pool = Pool.OpenPoolLedgerAsync(pool_name, pool_config).Result;
            
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    void GetNymTransaction(string target_did)
    {
        string submitter_did = result.Did;

        string nym_request = Ledger.BuildGetNymRequestAsync(submitter_did, target_did).Result;
        Debug.Log("nym_request: " + nym_request);

        string nym_response = Ledger.SubmitRequestAsync(pool, nym_request).Result;
        Debug.Log("nym_response: " + nym_response);
    }
}
