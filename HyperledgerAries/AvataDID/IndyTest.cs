using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;


public class IndyTest : MonoBehaviour
{
    [DllImport("indy")]
    public static extern void indy_create_wallet(char[] config, char[] credentials);

    [DllImport("indy")]
    public static extern int indy_open_wallet(char[] config, char[] credentials);

    [DllImport("indy")]
    public static extern int indy_create_and_store_my_did(int wallet_handle, char[] did_json);

    [DllImport("indy")]
    public static extern int indy_close_wallet(int wallet_handle);

    [DllImport("indy")]
    public static extern int indy_delete_wallet(char[] config, char[] credentials);

    [DllImport("hello_world")]
    public static extern void hello_world();

    [DllImport("hello_world")]
    public static extern int add_rust(int a, int b);

    
    public string wallet_config;
    public string wallet_credentials = "{\"key\":\"wallet_key\"}";

    public int wallet = 0;

    // Start is called before the first frame update
    void Start()
    {
        wallet_config = "{\"id\":\"wallet\", \"storage_type\": {\"path\": \"" + Application.dataPath + "\"}}";

        Debug.Log("Hello World");
        hello_world();

        Debug.Log("Add");
        int num = add_rust(1, 2);
        Debug.Log("1 + 2 = " + num.ToString());

        
        try
        {
            Debug.Log("Indy Create Wallet");
            indy_create_wallet(wallet_config.ToCharArray(), wallet_credentials.ToCharArray());

        }        
        catch (Exception e)
        {
            Debug.Log(e.ToString());

            Debug.Log("Indy Delete Wallet");
            indy_delete_wallet(wallet_config.ToCharArray(), wallet_credentials.ToCharArray());
        }


        Debug.Log("Indy Open Wallet");
        int wallet = indy_open_wallet(wallet_config.ToCharArray(), wallet_credentials.ToCharArray());
        Debug.Log("Wallet Handle: " + wallet.ToString());

        
        Debug.Log("Indy Create DID");
        int did = indy_create_and_store_my_did(wallet, "{}".ToCharArray());
        Debug.Log("DID: " + did.ToString());
        
        
        Debug.Log("Indy Close Wallet");
        indy_close_wallet(wallet);
        

        Debug.Log("Indy Delete Wallet");
        indy_delete_wallet(wallet_config.ToCharArray(), wallet_credentials.ToCharArray());


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
