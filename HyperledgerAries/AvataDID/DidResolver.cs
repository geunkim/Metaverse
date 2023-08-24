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


public class DidResolver
{
    public static string resolve(DidSystem didSystem, string targetDid)
    {
        Debug.Log("DID Resolver resolve");

        string nymResult = didSystem.GetNymTransaction(targetDid);
        Debug.Log("nymResult: " + nymResult);
        JObject nymJobject = JObject.Parse(nymResult);
        string nymData = nymJobject.GetValue("result").Value<JObject>().GetValue("data").Value<string>();
        Debug.Log("nymData: " + nymData);        

        if (nymData.Equals("null")) 
        {
            Debug.Log("nym transaction is null");
            return null;
        }

        string attribResult = didSystem.GetAttribTransaction(targetDid, "endpoint");

        JObject attribJobject = JObject.Parse(attribResult);
        string attribData = attribJobject.GetValue("result").Value<JObject>().GetValue("data").Value<string>();
        Debug.Log("attribData: " + attribData);

        if (attribData.Equals("null")) 
        {
            Debug.Log("attrib transaction is null");
            return null;
        }

        // endpoint
        string endpoint = JObject.Parse(attribData).GetValue("endpoint").ToString();
        Debug.Log("endpoint: " + endpoint);

        // verkey
        string verkey = JObject.Parse(nymData).GetValue("verkey").Value<string>();
        Debug.Log("verkey: " + verkey);

        // context
        List<string> context_list = new();
        context_list.Add("https://www.w3.org/ns/did/v1");
        context_list.Add("https://w3id.org/security/suites/ed25519-2018/v1");
        context_list.Add("https://w3id.org/security/suites/x25519-2019/v1");

        JArray context = new(context_list);
        Debug.Log("context: " + context.ToString());

        // id
        string id = "https://localhost:8080/" + targetDid;
        Debug.Log("id: " + id);

        // verificationMethod
        JArray verificationMethod = new();

        JObject verificationMethodJobject = new();
        verificationMethodJobject.Add("id", targetDid + "#key-1");
        verificationMethodJobject.Add("type", "Ed25519VerificationKey2018");
        verificationMethodJobject.Add("controller", targetDid);
        verificationMethodJobject.Add("publicKeyBase58", verkey);

        verificationMethod.Add(verificationMethodJobject);
        Debug.Log("verificationMethod: " + verificationMethod.ToString());

        // authentication
        JArray authentication = new();
        authentication.Add(targetDid + "#key-1");
        Debug.Log("authentication: " + authentication.ToString());

        // assertionMethod
        JArray assertionMethod = new();
        assertionMethod.Add(targetDid + "#key-1");
        Debug.Log("assertionMethod: " + assertionMethod.ToString());

        // service
        JArray service = new();

        JObject serviceJobject = JObject.Parse(endpoint);
        Debug.Log("serviceJobject: " + serviceJobject.ToString());

        foreach (JProperty property in serviceJobject.Properties())
        {
            JObject serviceJlist = new();
            if (property.Name.Equals("endpoint"))
            {
                serviceJlist.Add("type", property.Name);
                serviceJlist.Add("serviceEndpoint", property.Value);
            }/*
            else
            {
                Debug.Log("property.Name: " + property.Name);
                Debug.Log("property.Value: " + property.Value);
                serviceJlist.Add(property.Name, property.Value);
            }
            */
            service.Add(serviceJlist);
        }
        Debug.Log("service: " + service.ToString());

        // didDocument
        JObject didDocument = new();
        didDocument.Add("@context", context);
        didDocument.Add("id", id);
        didDocument.Add("verificationMethod", verificationMethod);
        didDocument.Add("authentication", authentication);
        didDocument.Add("assertionMethod", assertionMethod);
        didDocument.Add("service", service);
        Debug.Log("didDocument: " + didDocument.ToString());

        return didDocument.ToString();
    }
}
