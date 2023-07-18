using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Hyperledger.Indy.WalletApi;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.PoolApi;

public class LoginSystem : MonoBehaviour
{
    string defult_wallet_path;

    // Start is called before the first frame update
    void Start()
    {
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
        string wallet_name = "test_wallet" + Random.Range(0, 1000).ToString();
        //string wallet_config = "{\"id\":\"" + wallet_name + "\"}";
        string wallet_config = "{\"id\":\"" + wallet_name + "\", \"storage_config\": {\"path\": \"" 
            + defult_wallet_path + "\"}}";
        string wallet_credentials = "{\"key\":\"wallet_key\"}";

        Debug.Log("wallet_config :" + wallet_config);

        Wallet.CreateWalletAsync(wallet_config, wallet_credentials).Wait();
    }
}
