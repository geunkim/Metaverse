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

public class DidResolver : MonoBehaviour
{
    public string resolve(string target_did)
    {
        Debug.Log("DID Resolver resolve");
        DidSystem didSystem = DidSystem.GetInstance();

        string nym_result = didSystem.GetNymTransaction(target_did);
        Debug.Log("nym_result: " + nym_result);
        JObject nym_jObject = JObject.Parse(nym_result);
        string nym_data = nym_jObject.GetValue("result").Value<JObject>().GetValue("data").Value<string>();
        Debug.Log("nym_data: " + nym_data);        

        if (nym_data.Equals("null")) 
        {
            Debug.Log("nym transaction is null");
            return null;
        }

        string attrib_result = didSystem.GetAttribTransaction(target_did, "endpoint");

        JObject attrib_jObject = JObject.Parse(attrib_result);
        string attrib_data = attrib_jObject.GetValue("result").Value<JObject>().GetValue("data").Value<string>();
        Debug.Log("attrib_data: " + attrib_data);

        if (attrib_data.Equals("null")) 
        {
            Debug.Log("attrib transaction is null");
            return null;
        }

        // endpoint
        string endpoint = JObject.Parse(attrib_data).GetValue("endpoint").ToString();
        Debug.Log("endpoint: " + endpoint);

        // verkey
        string verkey = JObject.Parse(nym_data).GetValue("verkey").Value<string>();
        Debug.Log("verkey: " + verkey);

        // context
        List<string> context_list = new();
        context_list.Add("https://www.w3.org/ns/did/v1");
        context_list.Add("https://w3id.org/security/suites/ed25519-2018/v1");
        context_list.Add("https://w3id.org/security/suites/x25519-2019/v1");

        JArray context = new(context_list);
        Debug.Log("context: " + context.ToString());

        // id
        string id = "https://localhost:8080/" + target_did;
        Debug.Log("id: " + id);

        // verificationMethod
        JArray verificationMethod = new();

        JObject verificationMethod_jObject = new();
        verificationMethod_jObject.Add("id", target_did + "#key-1");
        verificationMethod_jObject.Add("type", "Ed25519VerificationKey2018");
        verificationMethod_jObject.Add("controller", target_did);
        verificationMethod_jObject.Add("publicKeyBase58", verkey);

        verificationMethod.Add(verificationMethod_jObject);
        Debug.Log("verificationMethod: " + verificationMethod.ToString());

        // authentication
        JArray authentication = new();
        authentication.Add(target_did + "#key-1");
        Debug.Log("authentication: " + authentication.ToString());

        // assertionMethod
        JArray assertionMethod = new();
        assertionMethod.Add(target_did + "#key-1");
        Debug.Log("assertionMethod: " + assertionMethod.ToString());

        // service
        JArray service = new();

        JObject service_jObject = JObject.Parse(endpoint);
        Debug.Log("service_jObject: " + service_jObject.ToString());

        foreach (JProperty property in service_jObject.Properties())
        {
            JObject service_jList = new();
            if (property.Name.Equals("endpoint"))
            {
                service_jList.Add("type", property.Name);
                service_jList.Add("serviceEndpoint", property.Value);
            }
            else
            {
                service_jList.Add(property.Name, property.Value);
            }
            service.Add(service_jList);
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
