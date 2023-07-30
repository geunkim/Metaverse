using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class HttpClient : MonoBehaviour
{
    string test_url = "http://220.68.5.139:9000/genesis";
    string acapy_url = "http://220.68.5.139:8001/connections";
    string acapy_did_url = "http://220.68.5.139:8001/wallet/did";
    string acapy_connection_url = "http://220.68.5.139:8001/connections/create-invitation";
    public string data = null;

    string genesis_file = null;

    public IEnumerator HttpGet(string url, Action<UnityWebRequest> callback) {

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

    public void Get(string url, Action<string> onSuccess, Action<string> onError)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        SendRequest(request, onSuccess, onError);
    }

    public void Post(string url, string body, Action<string> onSuccess, Action<string> onError)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, body);
        SendRequest(request, onSuccess, onError);
    }

    private void SendRequest(UnityWebRequest request, Action<string> onSuccess, Action<string> onError)
    {
        var operation = request.SendWebRequest();
        operation.completed += (op) =>
        {
            if (request.isHttpError || request.isNetworkError)
            {
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            request.Dispose();
        };
    }

    public string CreateGenesisFile(string path)
    {
        string test_url = "http://220.68.5.139:9000/genesis";
        string genesis_file = null;
        Get(test_url, (response) =>
        {
            Debug.Log("GET 요청 성공: " + response);
            StreamWriter sw;
            if(false == File.Exists(path))
            {
                Debug.Log("Genesis File Create");
                sw = new StreamWriter(path);
                sw.WriteLine(response);
                sw.Close();
            }
        }, (error) =>
        {
            Debug.LogError("GET 요청 실패: " + error);
        });

        return genesis_file;
    }
}
