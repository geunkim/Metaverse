using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;

public class PythonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            PrintHelloWorldFromPython();
            UnityEngine.Debug.Log("W pressed");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            IndyFromPython();
            UnityEngine.Debug.Log("K pressed");
        }
    }

    static void PrintHelloWorldFromPython()
    {
        PythonRunner.RunString(@"
                import UnityEngine;
                UnityEngine.Debug.Log('hello world')
                ");
    }

    static void IndyFromPython()
    {
        UnityEngine.Debug.Log($"{Application.dataPath}");
        // 파이썬 코드 경로
        PythonRunner.RunFile($"{Application.dataPath}/did.py");
    }
}