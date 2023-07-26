using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hyperledger.Indy.WalletApi;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.PoolApi;
using Hyperledger.Indy.LedgerApi;
using Hyperledger.Indy.CryptoApi;
using Hyperledger.Indy.AnonCredsApi;

public class DidUser : MonoBehaviour
{

    string defult_wallet_path;
    string wallet_config;
    string wallet_name;
    string wallet_credentials;

    string pool_name;
    string pool_config;

    Wallet wallet;
    Pool pool;

    string genesis_file_path = null;

    CreateAndStoreMyDidResult result;

    string did_seed = "issuer00000000000000000000000000";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
