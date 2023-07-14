using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

using Hyperledger.Indy.WalletApi;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.PoolApi;


public class IndyTest : MonoBehaviour
{
    [DllImport("indy")]
    public static extern void indy_create_wallet(char[] config, char[] credentials);

    [DllImport("indy")]
    public static extern int indy_open_wallet(char[] config, char[] credentials);

    [DllImport("indy")]
    public static extern char[] indy_create_and_store_my_did(int wallet_handle, char[] did_json);

    [DllImport("indy")]
    public static extern char[] indy_list_my_dids_with_meta(int wallet_handle);

    [DllImport("indy")]
    public static extern void indy_close_wallet(int wallet_handle);

    [DllImport("indy")]
    public static extern void indy_delete_wallet(char[] config, char[] credentials);

    [DllImport("indy")]
    public static extern void indy_create_pool_ledger_config(char[] config_name, char[] config);

    [DllImport("indy")]
    public static extern int indy_open_pool_ledger(char[] config_name, char[] config);

    [DllImport("hello_world")]
    public static extern void hello_world();

    [DllImport("hello_world")]
    public static extern int add_rust(int a, int b);

    
    string wallet_config;
    string wallet_credentials = "{\"key\":\"wallet_key\"}";

    public Text text;

    int wallet = 0;

    string genesis_file_path = null;
    string test_url = "http://220.68.5.139:9000/genesis";

    // Start is called before the first frame update
    void Start()
    {
        /*
        wallet_config = "{\"id\":\"wallet\", \"storage_type\": {\"path\": \"" + Application.dataPath + 
        "/.indy_client/wallet\"}}";
        */
        wallet_config = "{\"id\":\"wallet_unity\"}";
        Debug.Log(wallet_config);

        genesis_file_path = Application.dataPath + "/genesis.txn";
        HttpClient httpClient = HttpClient.GetInstance();
        string genesis_file_ = httpClient.CreateGenesisFile(genesis_file_path);
        Debug.Log("genesis_file_: " + genesis_file_);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Wallet Test Start");
            IndyWalletTestFun();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Pool Connection Test Start");
            IndyPoolTestFun();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Wallet API Test Start");
            IndyWalletApiTestFun();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Pool API Test Start");
            IndyPoolApiTestFun();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }

    public void IndyWalletTestFun()
    {
        Debug.Log("Hello World");
        hello_world();

        Debug.Log("Add");
        int num = add_rust(1, 2);
        Debug.Log("1 + 2 = " + num.ToString());
        
        int wallet;
        char[] did;
        char[] did_list;

        
        try
        {
            Debug.Log("Indy Create Wallet");
            indy_create_wallet(wallet_config.ToCharArray(), wallet_credentials.ToCharArray());

            Debug.Log("Indy Open Wallet");
            wallet = indy_open_wallet(wallet_config.ToCharArray(), wallet_credentials.ToCharArray());
            Debug.Log("Wallet Handle: " + wallet.ToString());

            Debug.Log("Wallet Handle: " + wallet.GetType());
            text.text = wallet.ToString();

            /*
            try
            {
                
                
                Debug.Log("Indy List DID");
                did_list = indy_list_my_dids_with_meta(wallet);
                Debug.Log("DID List: " + did_list.ToString());

                
                Debug.Log("Indy Create DID");
                did = indy_create_and_store_my_did(wallet, "{}".ToCharArray());
                Debug.Log("DID: " + did.ToString());
                
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            finally
            {
                //Debug.Log("Indy Close Wallet");
                //indy_close_wallet(wallet);
            }
            */

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            //Debug.Log("Indy Delete Wallet");
            //indy_delete_wallet(wallet_config.ToCharArray(), wallet_credentials.ToCharArray());
        }
    }

    public void IndyPoolTestFun()
    {
        string pool_name = "pool";
        string genesis_file_path = Application.dataPath + "/genesis.txn";

        StreamWriter sw;
        if(false == File.Exists(genesis_file_path))
        {
            Debug.Log("Genesis File Create");
            sw = new StreamWriter(genesis_file_path);
            //sw.WriteLine(genesis_file);
        }

        string pool_config = "{\"genesis_txn\":\"" + genesis_file_path + "\"}";
        Debug.Log("Pool Config: " + pool_config);

        
        Debug.Log("Indy Create Pool Ledger Config");
        indy_create_pool_ledger_config(pool_name.ToCharArray(), pool_config.ToCharArray());

        /*
        Debug.Log("Indy Open Pool Ledger");
        int pool = indy_open_pool_ledger(pool_name.ToCharArray(), pool_config.ToCharArray());
        Debug.Log("Pool Handle: " + pool.ToString());
        */
    }

    public void IndyWalletApiTestFun()
    {
        string wallet_name = "wallet";
        string wallet_config = "{\"id\":\"" + wallet_name + "\", \"storage_type\": {\"path\": \"" 
        + Application.dataPath + "/indy/wallet\"}}";
        string wallet_credentials = "{\"key\":\"wallet_key\"}";

        Wallet wallet_handle = null;
        CreateAndStoreMyDidResult did = null;
        string did_list = null;

        try
        {
            Debug.Log("Indy Create Wallet");
            Wallet.CreateWalletAsync(wallet_config, wallet_credentials).Wait();

            Debug.Log("Indy Open Wallet");
            wallet_handle = Wallet.OpenWalletAsync(wallet_config, wallet_credentials).Result;
            Debug.Log("Wallet Handle: " + wallet_handle.ToString());

            Debug.Log("Indy Create DID");
            string did_json = "{\"seed\":\"test0000000000000000000000000000\"}";
            did = Did.CreateAndStoreMyDidAsync(wallet_handle, did_json).Result;
            Debug.Log("DID: " + did);

            Debug.Log("Indy List DID");
            did_list = Did.ListMyDidsWithMetaAsync(wallet_handle).Result;
            Debug.Log("DID List: " + did_list);

            text.text = did_list;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            Debug.Log("Indy Close Wallet");
            wallet_handle.CloseAsync().Wait();

            //Debug.Log("Indy Delete Wallet");
            //Wallet.DeleteWalletAsync(wallet_config, wallet_credentials).Wait();
        }
    }

    public void IndyPoolApiTestFun()
    {
        if(false == File.Exists(genesis_file_path))
        {
            Debug.Log("Genesis File is Null");
            return;
        }

        string pool_name = "pool";
        string pool_config = "{\"genesis_txn\":\"" + genesis_file_path + "\"}";
        Debug.Log("Pool Config: " + pool_config);

        Pool pool_handle = null;

        try
        {
            Debug.Log("Indy Create Pool Ledger Config");
            Pool.CreatePoolLedgerConfigAsync(pool_name, pool_config).Wait();

            Debug.Log("Indy Open Pool Ledger");
            pool_handle = Pool.OpenPoolLedgerAsync(pool_name, pool_config).Result;
            Debug.Log("Pool Handle: " + pool_handle.ToString());

            text.text = pool_handle.ToString();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            Debug.Log("Indy Close Pool Ledger");
            pool_handle.CloseAsync().Wait();
            Debug.Log("Indy Delete Pool Ledger Config");
            Pool.DeletePoolLedgerConfigAsync(pool_name).Wait();
        }
    }
}
