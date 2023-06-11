using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting.Python;
using UnityEngine;
using UnityEngine.UI;

public class UserDID : MonoBehaviour
{
    private string userDID;          // 사용자 DID
    private bool didGenerated = false; // DID가 발급되어 있는지의 여부

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateDID();
        }
    }

    // DID 생성 함수
    public void GenerateDID()
    {
        if (!didGenerated)
        {
            IndyFromPython(); // 파이썬 코드 실행
            didGenerated = true;
        }
    }

    static void IndyFromPython()
    {
        // 파이썬 코드 실행
        Debug.Log("Python code executed");
        PythonRunner.RunFile($"{Application.dataPath}/Scripts/did.py");

    }

    // DID를 받아오는 함수, 파이썬 코드에서 호출된다
    public void SetDID(string did)
    {
        userDID = did;
        UnityEngine.Debug.Log($"Received DID from Python: {userDID}");
    }

    public string GetDID()
    {
        return userDID;
    }

}
