using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient : MonoBehaviour
{
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
            StartCoroutine(HttpTest());
        }
    }

    IEnumerator HttpTest() {
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Get("http://220.68.5.139:9000/genesis"))
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
        }
    }
}
