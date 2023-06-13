using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using UnityEngine;

public class HttpServer : MonoBehaviour
{

    private HttpListener _listener;
    private Thread _thread;

    public string url = "localhost";
    public int port = 8080;

    // Start is called before the first frame update
    void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://" + url + ":" + port + "/");
        _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        _listener.Start();

        _thread = new Thread(startListener);
        _thread.Start();
        Debug.Log("HTTP SERVER START!!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void startListener() 
    {
        while (_listener.IsListening) 
        {
        
        }
    }
}
