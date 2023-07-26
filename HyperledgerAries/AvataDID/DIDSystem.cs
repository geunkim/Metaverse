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

using Newtonsoft.Json.Linq;

public class DidSystem : MonoBehaviour
{
    public static DidSystem Instance;
    DidResolver didResolver;

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

    void Awake()
    {
        GetInstance();
    }

    void Start()
    {
        didResolver = this.gameObject.GetComponent<DidResolver>();

        genesis_file_path = Application.dataPath + "/genesis.txn";        
        Debug.Log("genesis_file_path: " + genesis_file_path);

        defult_wallet_path = Application.dataPath + "/wallet";
        Debug.Log("defult_wallet_path: " + defult_wallet_path);
        
        HttpClient httpClient = HttpClient.GetInstance();
        string genesis_file_ = httpClient.CreateGenesisFile(genesis_file_path);
        Debug.Log("genesis_file_: " + genesis_file_);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Create Wallet Start");
            CreateWallet();

            Debug.Log("Search Wallet Start");
            SearchWallet();

            Debug.Log("Open Wallet Start");
            OpenWallet();

            Debug.Log("Create Did Start");
            CreateDidInWallet();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Pool Start");
            ConnectionPool();

            Debug.Log("Get Nym Transaction Start");
            string nym_result = GetNymTransaction(result.Did);
            Debug.Log("nym_result: " + nym_result);

            Debug.Log("Get Attrib Transaction Start");
            string attrib_result = GetAttribTransaction(result.Did, "endpoint");
            Debug.Log("attrib_result: " + attrib_result);

            JObject jObject = JObject.Parse(attrib_result);
            string data = jObject.GetValue("result").Value<JObject>().GetValue("data").Value<string>();
            string endpoint = JObject.Parse(data).GetValue("endpoint").Value<JObject>().GetValue("endpoint").Value<string>();
            Debug.Log("endpoint: " + endpoint);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Clean Wallet Start");
            CleanWallet();

            Debug.Log("Clean Pool Start");
            CleanPool();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (didResolver.Equals(null))
            {
                Debug.Log("didResolver is null");
                return;
            }
            string resolve_data = didResolver.resolve(result.Did);
            Debug.Log("resolve_data: " + resolve_data);
        }

    }

    static public DidSystem GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType(typeof(DidSystem)) as DidSystem;
        }

        if (Instance == null)
        {
            GameObject obj = new GameObject("DidSystem");
            Instance = obj.AddComponent<DidSystem>();
        }

        return Instance;
    }

    void SearchWallet()
    {
        Debug.Log("Search Wallet");
        string[] wallet_list = Directory.GetDirectories(defult_wallet_path);
        foreach (string wallet in wallet_list)
        {
            Debug.Log(wallet);
        }
    }

    void CreateWallet()
    {
        Debug.Log("Indy Create Wallet");
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

    void CleanWallet()
    {
        try
        {
            Debug.Log("Indy Close Wallet");
            wallet.CloseAsync().Wait();
            Wallet.DeleteWalletAsync(wallet_config, wallet_credentials).Wait();
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
            Debug.Log("DID: " + result.Did);
            Debug.Log("Verkey: " + result.VerKey);
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

        pool_name = "pool" + UnityEngine.Random.Range(0, 1000).ToString();
        pool_config = "{\"genesis_txn\":\"" + genesis_file_path + "\"}";
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

    void CleanPool()
    {
        try
        {
            Debug.Log("Indy Close Pool Ledger");
            pool.CloseAsync().Wait();
            Pool.DeletePoolLedgerConfigAsync(pool_name).Wait();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public string GetNymTransaction(string target_did)
    {
        string submitter_did = result.Did;

        string nym_request = Ledger.BuildGetNymRequestAsync(submitter_did, target_did).Result;
        Debug.Log("nym_request: " + nym_request);

        string nym_response = Ledger.SubmitRequestAsync(pool, nym_request).Result;
        Debug.Log("nym_response: " + nym_response);

        return nym_response;
    }

    public string GetAttribTransaction(string target_did, string attrib)
    {
        string submitter_did = result.Did;

        string attrib_request = Ledger.BuildGetAttribRequestAsync(submitter_did, target_did, attrib, null, null).Result;
        Debug.Log("attrib_request: " + attrib_request);

        string attrib_response = Ledger.SubmitRequestAsync(pool, attrib_request).Result;
        Debug.Log("attrib_response: " + attrib_response);

        return attrib_response;
    }
}
