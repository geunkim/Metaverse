## 로그인 시스템을 이용한 DID 발급

### 1. 메타버스 내의 상호작용을 통해 DID를 발급을 테스트한다. 이를 위해 메타버스 내에서 버튼을 클릭 시 사용자에게 DID가 발급되도록 한다
DIDButton.cs를 통해 버튼을 클릭 시 DID 발급 기능이 구현된 파이썬 코드를 실행하도록 한다.
```C#
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickExample : MonoBehaviour
{
    public Button generateDIDButton;   // 이벤트에 활용할 버튼

    private bool didGenerated = false; // DID가 발급되어 있는지의 여부

    private void Start()
    {
        generateDIDButton.onClick.AddListener(GenerateDID); // 버튼 클릭 이벤트를 DID 생성 함수와 연결
    }

    // DID 생성 함수
    private void GenerateDID()
    {
        if (!didGenerated)
        {
            PythonTest.IndyFromPython(); // 파이썬 코드 실행
            didGenerated = true;
        }
    }
}
```
#

### 2. 발급받은 DID를 유니티에 저장하여 로그인 시스템에 활용

- 이를 위해서는 DID를 생성하는 파이썬 코드를 수정하여 유니티로 DID 값을 보낼 수 있어야 함
- 유니티의 DID 정보를 관리하는 클래스의 메서드를 호출하여 유니티 내 변수에 저장
- 다음은 DID를 생성하고 유니티로 값을 전달하는 기능을 하는 pythontest.py이다
```Python
import sys
import os
import asyncio
import json
from pathlib import Path

from indy import pool, wallet, did
from indy.error import ErrorCode, IndyError

import UnityEngine

os.add_dll_directory("C:\indy1_9\lib")

pool_name = 'pool'
issuer_wallet_config = json.dumps({"id": "issuer_wallet"})
issuer_wallet_credentials = json.dumps({"key": "issuer_wallet_key"})
genesis_file_path = Path.home().joinpath("pool_transactions_genesis")


async def proof_negotiation():
    try:
        # 3. Creating Issuer wallet and opening it to get the handle.
        UnityEngine.Debug.Log('\n3. Creating Issuer wallet and opening it to get the handle.\n')
        try:
            await wallet.create_wallet(issuer_wallet_config, issuer_wallet_credentials)
        except IndyError as ex:
            if ex.error_code == ErrorCode.WalletAlreadyExistsError:
                pass

        issuer_wallet_handle = await wallet.open_wallet(issuer_wallet_config, issuer_wallet_credentials)

        # 4. Generating and storing steward DID and verkey
        UnityEngine.Debug.Log('\n4. Generating and storing steward DID and verkey\n')
        steward_seed = '000000000000000000000000Steward1'
        did_json = json.dumps({'seed': steward_seed})
        steward_did, steward_verkey = await did.create_and_store_my_did(issuer_wallet_handle, did_json)

        # 22. Closing the wallet handle
        UnityEngine.Debug.Log('\n22. Closing the wallet handle\n')
        await wallet.close_wallet(issuer_wallet_handle)

        # 23. Deleting created wallet_handles
        UnityEngine.Debug.Log('\n23. Deleting created wallet_handles\n')
        await wallet.delete_wallet(issuer_wallet_config, issuer_wallet_credentials)

        # Return the generated DID to Unity
        return steward_did

    except IndyError as e:
        UnityEngine.Debug.Log('Error occurred: %s' % e)


def main():
    # os.add_dll_directory("D:\libindy_1.16.0\lib")
    UnityEngine.Debug.Log("START!!!")
    loop = asyncio.get_event_loop()
    result = loop.run_until_complete(proof_negotiation())
    loop.close()

    # Send the generated DID back to Unity
    did_json = json.loads(result)
    user_did = did_json.get('did', '')
    UnityEngine.Debug.Log(f'Generated DID: {user_did}')

    # Pass the DID value to Unity
    unity_handler = UnityHandler()
    unity_handler.SendDID(user_did)


class UnityHandler:
    def SendDID(self, did):
        # Call Unity function to receive the DID value
        UnityEngine.Debug.Log(f'Sending DID to Unity: {did}')
        UnityEngine.GameObject.FindObjectOfType[PythonTest]().SetDID(did)


py_ver = int(f"{sys.version_info.major}{sys.version_info.minor}")
if py_ver > 37 and sys.platform.startswith('win'):
    asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())

main()
```
- UnityHandler 클래스 내의 SendDID() 메서드를 통해 DID 값을 Unity로 전달. 
- UnityHandler 클래스에서 Unity로 DID 값을 전달하기 위해 PythonTest.cs 스크립트의 SetDID() 메서드를 호출하여 DID 값을 전달.
- SetDID() 메서드는 Unity에서 정의된 스크립트에 따라 Unity에서 처리됨. 
- Unity에서 SetDID() 메서드를 적절히 구현하여 DID 값을 받고 필요한 작업을 수행.

#

다음으로 파이썬 코드를 실행하고 생성된 DID를 받아오고 활용하는 유니티 스크립트 pythontest.cs
```C#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PythonTest : MonoBehaviour
{
    private string userDID;          // 사용자 DID
    private bool isLoggedIn = false; // 사용자 로그인 여부
    public Button generateDIDButton; // 이벤트에 활용할 버튼

    private void Start()
    {
        generateDIDButton.onClick.AddListener(GenerateDID); // 버튼 클릭 이벤트를 DID 생성 함수와 연결
    }

    // DID를 생성하는 파이썬 파일을 실행
    static void IndyFromPython()
    {
        UnityEngine.Debug.Log($"{Application.dataPath}");
        PythonRunner.RunFile($"{Application.dataPath}/_PythonScript/pythonTest.py");
    }

    // 만약 기존에 DID가 없다면 DID 생성 함수 실행
    public void GenerateDID()
    {
        if (string.IsNullOrEmpty(userDID))
        {
            IndyFromPython();
        }
    }

    // DID를 받아오는 함수, 파이썬 코드에서 호출된다
    public void SetDID(string did)
    {
        userDID = did;
        UnityEngine.Debug.Log($"Received DID from Python: {userDID}");

        if (!isLoggedIn)
        {
            // 로그인 절차
            if (LoginProcess(userDID))
            {
                isLoggedIn = true;
                UnityEngine.Debug.Log("User logged in successfully!"); // 로그인 성공
            }
            else
            {
                UnityEngine.Debug.Log("Failed to log in user!");
            }
        }
    }

    // 로그인 절차를 수행하는 함수
    private bool LoginProcess(string did)
    {
        // 로그인 절차를 수행하는 코드를 작성.
        // 사용자 인증을 진행하고 인증 결과를 반환하는 등의 작업을 수행.
        // 성공적으로 로그인되었을 경우 true를 반환하고, 그렇지 않을 경우 false를 반환.
        if (did == userDID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
```
#
### 작동 구조
- Click Button에서 회원가입시 필요한 사용자의 ID와 비밀번호 입력이 필요
- 이 ID와 비밀번호를 Key로 가지는 DID 값 생성 필요

![DID drawio (1)](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/1895da2c-fa61-4e3e-94cd-94288877c33b)



### 0612 진행상황
![image](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/0c693763-9c2c-460c-9d9a-26074c019710)
- UserDID 클래스의 SetID() 메서드를 호출하는 과정에서 UserDID 클래스를 찾을 수 없다는 에러 발생 -> 해결 필요
- 하나의 사용자가 여러 아바타를 가질 수 있는 상황을 가정
- 세 개의 클래스 생성
    - UserDID : 파이썬 코드를 통해 DID 값을 생성하고 변수에 저장. 이후 다른 클래스에서 UserDID의 GetDID()를 통해 DID 값을 얻을 수 있음
    - Game_Manager : 메타버스 내에 전역적으로 존재하는 클래스. 아바타 생성 및 아바타 관리를 담당.
    - Avatar : 아바타 ID와 사용자의 DID를 가지고 있음. Game_Manager로부터 얻은 DID 값을 통해 여러 신원 증명 활동을 가능.

UserDID.cs
```C#
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
```

#
```C#
Game_Manager.cs
using myspace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    private UserDID userDID; // UserDID 객체 참조
    private List<Avatar> avatarList; // 아바타 리스트
    private static Game_Manager instance;
    private string did;          // 사용자 DID

    public GameObject cameraPrefab; // 카메라 객체 참조
    public Button button;   // 로그인 버튼
    public GameObject avatarPrefab;   // 아바타 객체 참조
    public static Game_Manager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        // 게임 시작 시 UserDID 객체 찾아서 참조
        userDID = FindObjectOfType<UserDID>();
        button.onClick.AddListener(OnLoginButtonClick);
        avatarList = new List<Avatar>();
    }

    // 로그인 버튼 클릭 시 호출되는 함수
    public void OnLoginButtonClick()
    {
        // UserDID의 GetDID() 호출하여 사용자에게 DID 부여
        userDID.GenerateDID();
        did = userDID.GetDID();
        Debug.Log($"User DID: {did}");

        // load scene "inside"
        UnityEngine.SceneManagement.SceneManager.LoadScene("inside");
        CreateAvatar("asd");
    }

    // 아바타 생성 함수
    public void CreateAvatar(string avatarName)
    {
        // 아바타 생성
        GameObject avatarObject = Instantiate(avatarPrefab, new Vector3(3.37f, 0.03f, 3.67f), Quaternion.identity);
        Avatar avatar = avatarObject.GetComponent<Avatar>();

        // 아바타에 사용자의 DID 저장
        avatar.InitializeAvatar(avatarName, did);
        avatarList.Add(avatar);

        // 카메라 생성
        GameObject cameraObject = Instantiate(cameraPrefab, avatarObject.transform);
        Camera avatarCamera = cameraObject.GetComponent<Camera>();

        // PlayerController에 카메라 전달
        PlayerController playerController = avatarObject.GetComponent<PlayerController>();
        playerController.SetCamera(avatarCamera);

    }
}
```
#
Avatar.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private string avatarID;       // 아바타 식별자
    private string userDID;        // 사용자 DID

    // 아바타 초기화 함수
    public void InitializeAvatar(string avatarID, string userDID)
    {
        this.avatarID = avatarID;
        this.userDID = userDID;
    }

    // 아바타 동작 함수
    public void PerformAction()
    {
        // 아바타의 행위를 수행
        // 예: 신원 증명이 필요한 행위를 수행할 때 UserDID를 사용하여 신원을 증명
        if (string.IsNullOrEmpty(userDID))
        {
            Debug.LogError("UserDID is not set for the avatar!");
            return;
        }

        // 아바타의 행위를 수행하는 동안 UserDID를 사용하여 신원 증명
        Debug.Log($"Performing action with UserDID: {userDID}");
        // ... 행위 수행 코드 ...
    }
}
```

![image](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/ae92a325-1022-4836-812f-ac89fca51795)
- 게임 실행 시 처음 화면. 버튼을 클릭하면 씬이 전환되며 아바타를 생성
- 아바타 ID 문자열을 입력하면 해당 문자열을 ID로 갖는 아바타 생성 기능 추가 예정


![image](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/6db3c497-a89c-4270-a795-0194d4032214)
- 아바타와 카메라를 생성. 카메라는 아바타를 따라다님
- 아바타는 사용자 DID를 가지고 있음


- 설계 도형
![avata drawio](https://github.com/Hongyoosung/Metaverse-1/assets/101240036/f418da51-fd21-4d91-950b-dd57e589fcf3)




























