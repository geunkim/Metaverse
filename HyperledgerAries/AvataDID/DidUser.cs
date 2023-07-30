using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hyperledger.Indy.WalletApi;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.PoolApi;
using Hyperledger.Indy.LedgerApi;
using Hyperledger.Indy.CryptoApi;
using Hyperledger.Indy.AnonCredsApi;
using System.Text;
using Newtonsoft.Json.Linq;

public class DidUser : MonoBehaviour
{
    DidSystem didSystem;

    Wallet wallet;

    string walletName = "wallet";
    string walletKey = "walletCredentials";

    Dictionary<string, Wallet> walletList;

    public CreateAndStoreMyDidResult didAndVerkey;

    public string didSeed = "issuer00000000000000000000000000";

    string walletConfig;
    string walletCredentials;

    void Start()
    {
        didSystem = DidSystem.GetInstance();

        WalletCreateAndOpen();
        CreateAndStoreDid();
        GameManager.GetInstance().didUserDictionary.Add(didAndVerkey.Did, this);
    }

    void WalletCreateAndOpen()
    {
        Debug.Log("DID User Create Wallet");
        string walletTestName = this.walletName + UnityEngine.Random.Range(0, 1000).ToString();
        walletConfig = "{\"id\":\"" + walletTestName + "\"}";
        //string walletConfig = "{\"id\":\"" + this.walletName + "\"}";
        //walletConfig = "{\"id\":\"" + walletName + "\", \"storageConfig\": {\"path\": \"" 
        //    + defultWalletPath + "\"}}";
        walletCredentials = "{\"key\":\"" + this.walletKey + "\"}";

        Wallet.CreateWalletAsync(walletConfig, walletCredentials).Wait();
        wallet = Wallet.OpenWalletAsync(walletConfig, walletCredentials).Result;
    }

    void CreateAndStoreDid()
    {
        Debug.Log("CreateAndStoreDid");
        string didConfig = "{\"seed\":\"" + didSeed + "\"}";
        didAndVerkey = Did.CreateAndStoreMyDidAsync(wallet, didConfig).Result;
        Debug.Log("didAndVerkey: " + didAndVerkey.Did + " " + didAndVerkey.VerKey);
    }

    public void CleanWallet()
    {
        if(wallet == null)
        {
            Debug.Log("this.wallet is null");
        }

        Debug.Log(didAndVerkey.Did + " - CleanWallet");
        wallet.CloseAsync().Wait();
        Wallet.DeleteWalletAsync(walletConfig, walletCredentials).Wait();
    }
    
    public string PackMessage(string message, string targetDid)
    {
        Debug.Log("PackMessage");
        byte[] packedMessage = Crypto.PackMessageAsync(wallet, didAndVerkey.VerKey, targetDid, 
            System.Text.Encoding.UTF8.GetBytes(message)).Result;
        Debug.Log("packedMessage: " + packedMessage.ToString());

        return Encoding.UTF8.GetString(packedMessage);
    }

    public string PackMessage(string message, CreateAndStoreMyDidResult targetDidResult)
    {
        Debug.Log("PackMessage");
        JArray targetVerkeyArray = new();
        targetVerkeyArray.Add(targetDidResult.VerKey);
        string targetVerkey = targetVerkeyArray.ToString();
        Debug.Log("targetVerkey: " + targetVerkey);

        byte[] packedMessage = Crypto.PackMessageAsync(wallet, targetVerkey, didAndVerkey.VerKey, 
            System.Text.Encoding.UTF8.GetBytes(message)).Result;
        Debug.Log("packedMessage: " + packedMessage.ToString());

        return Encoding.UTF8.GetString(packedMessage);
    }

    public string UnpackMessage(string message)
    {
        Debug.Log("UnpackMessage");
        byte[] unpackMessage = Crypto.UnpackMessageAsync(wallet, 
            System.Text.Encoding.UTF8.GetBytes(message)).Result;
        Debug.Log("unpackMessage: " + unpackMessage.ToString());

        return Encoding.UTF8.GetString(unpackMessage);
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter");
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("OnCollisionEnter with " + other.gameObject.name);
            DidUser targetDidUser = other.gameObject.GetComponent<DidUser>();
            if(targetDidUser != null)
            {
                Debug.Log("targetDidUser != null");
                string message = "Hello, " + targetDidUser.didAndVerkey.Did;
                string packedMessage = PackMessage(message, targetDidUser.didAndVerkey);
                Debug.Log("packedMessage: " + packedMessage);

                string unpackMessage = targetDidUser.UnpackMessage(packedMessage);
                Debug.Log("unpackMessage: " + unpackMessage);
            }
            else
            {
                Debug.Log("targetDidUser == null");
            }
        }
    }
}
