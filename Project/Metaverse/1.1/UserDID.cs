using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Scripting.Python;
using UnityEngine;
using UnityEngine.UI;

public class UserDID : MonoBehaviour
{
    private string userDID;          // ����� DID
    private bool didGenerated = false; // DID�� �߱޵Ǿ� �ִ����� ����

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateDID();
        }
    }

    // DID ���� �Լ�
    public void GenerateDID()
    {
        if (!didGenerated)
        {
            IndyFromPython(); // ���̽� �ڵ� ����
            didGenerated = true;
        }
    }

    static void IndyFromPython()
    {
        // ���̽� �ڵ� ����
        Debug.Log("Python code executed");
        //PythonRunner.RunFile($"{Application.dataPath}/Scripts/did.py");

    }

    // DID�� �޾ƿ��� �Լ�, ���̽� �ڵ忡�� ȣ��ȴ�
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
