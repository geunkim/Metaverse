using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using Python.Runtime;

public class PythonTest : MonoBehaviour
{
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        string dir = __DIR__();
        UnityEngine.Debug.Log("dir : " + dir);
        GameObject.FindObjectOfType<Text>().text = dir;
        
        UnityEngine.Debug.Log("text is " + text);
        Text text_;

        text.SetActive(true);
        text_ = text.GetComponent<Text>();
        text_.text = "test !!";
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            PrintHelloWorldFromPython();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            IndyFromPython();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            PythonTest2();
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
        PythonRunner.RunFile($"{Application.dataPath}/Scripts/pythonTest.py");
    }

    static void PythonTest2()
    {
        UnityEngine.Debug.Log($"{Application.dataPath}");
        PythonRunner.RunFile($"{Application.dataPath}/Scripts/pythonTest2.py", "Hello");
    }

    private static string __DIR__([System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
    {
        return Path.GetDirectoryName(fileName);
    }

}
