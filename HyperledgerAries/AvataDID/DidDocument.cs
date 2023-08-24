using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DidDocument
{
    public Root root;

    public string ToString()
    {
        string result = "";

        result += "context: " + root.context[0] + "\n";
        result += "id: " + root.id + "\n";
        result += "verificationMethod: " + root.verificationMethod[0].id + "\n";
        result += "authentication: " + root.authentication[0] + "\n";
        result += "assertionMethod: " + root.assertionMethod[0] + "\n";
        result += "service: " + root.service[0].type + "\n";
        result += "serviceEndpoint: " + root.service[0].serviceEndpoint + "\n";

        return result;
    }
}

[System.Serializable]
public class Root
{
    public List<string> context { get; set; }
    public string id { get; set; }
    public List<VerificationMethod> verificationMethod { get; set; }
    public List<string> authentication { get; set; }
    public List<string> assertionMethod { get; set; }
    public List<Service> service { get; set; }
}

public class Service
{
    public string type { get; set; }
    public string serviceEndpoint { get; set; }
}

public class VerificationMethod
{
    public string id { get; set; }
    public string type { get; set; }
    public string controller { get; set; }
    public string publicKeyBase58 { get; set; }
}