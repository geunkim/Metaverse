using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class HttpClient : MonoBehaviour
{

    string test_url = "http://220.68.5.139:9000/genesis";
    string acapy_url = "http://220.68.5.139:8001/connections";
    string acapy_did_url = "http://220.68.5.139:8001/wallet/did";
    string acapy_connection_url = "http://220.68.5.139:8001/connections/create-invitation";
    string data = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("HTTP TEST CHECK");
            StartCoroutine(HttpGet(test_url, (www) =>
            {
                data = www.downloadHandler.text;
            }));
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("ACA-PY TEST CHECK");
            StartCoroutine(HttpGet(acapy_did_url, (www) =>
            {
                data = www.downloadHandler.text;
            }));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("ACA-PY TEST CHECK POST");

            string json = "{}";

            StartCoroutine(HttpPost(acapy_connection_url, json, (www) =>
            {
                data = www.downloadHandler.text;
            }));
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("data Check");
            Debug.Log("data : " + data);
        }
    }

    IEnumerator HttpGet(string url, Action<UnityWebRequest> callback) {

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Complete");
            }
            www.Dispose();
        }
    }

    IEnumerator HttpPost(string url, string json, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            //www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Complete");
                callback(www);
            }
            www.Dispose();
        }
    }
}
