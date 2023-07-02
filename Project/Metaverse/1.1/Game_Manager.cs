using myspace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AvatarData
{
    public GameObject AvatarObject { get; set; }
    public string AvatarID { get; set; }
    // 추가적인 아바타 정보 필드들...

    public AvatarData(GameObject avatarObject, string avatarID)
    {
        AvatarObject = avatarObject;
        AvatarID = avatarID;
    }
}

public class Game_Manager : MonoBehaviour
{
    private UserDID userDID; // UserDID 객체 참조
    private List<AvatarData> avatarList; // 아바타 리스트
    private static Game_Manager instance;
    private string did;          // 사용자 DID
    private string avatarID;       // 아바타 식별자
    private Vector3 avatarPos;

    //public Button button;   // 로그인 버튼
    public InputField inputField;   // 아바타 식별자 입력 필드
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
        //button.onClick.AddListener(OnLoginButtonClick);
        inputField.onEndEdit.AddListener(OnEndEdit);
        avatarList = new List<AvatarData>();
    }

    // 로그인 버튼 클릭 시 호출되는 함수
    public void OnLoginButtonClick()
    {
        // UserDID의 GetDID() 호출하여 사용자에게 DID 부여
        //userDID.GenerateDID();
        //did = userDID.GetDID();
        did = "asdasd";
        Debug.Log($"User DID: {did}");

        // load scene "Outside"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Outside");
    }

    // 아바타 ID 입력 및 저장
    private void OnEndEdit(string value)
    {
        // 입력된 값(value)을 변수에 저장
        avatarID = value;
        Debug.Log("Avatar ID: " + avatarID);

        // load scene "Outside"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Outside");
    }

    // 씬 전환 후에 호출되는 이벤트
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 씬 전환 전에 호출되는 이벤트
    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 전환된 후에 호출되는 콜백 함수
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "Outside")
        {
            // 아바타 식별자 생성, 이후 칸 입력으로 대체
            avatarPos = new Vector3(230.07f, 18.0f, 165.0f);
            CreateAvatar(avatarID, avatarPos);
        }

        if (scene.name == "Inside")
        {
            // 아바타 생성
            avatarPos = new Vector3(3.37f, 0.03f, 3.67f);
            AvatarData avatarData = GetAvatar(avatarID);

            if (avatarData == null)
            {
                GameObject avatarObject = Instantiate(avatarPrefab, avatarPos, Quaternion.identity);
                avatarData = new AvatarData(avatarObject, avatarID);
                avatarList.Add(avatarData);
            }
            else
            {
                if (avatarData.AvatarObject != null)
                {
                    avatarData.AvatarObject.transform.position = avatarPos;
                    avatarData.AvatarObject.SetActive(true);
                }
                else
                {
                    // 아바타 객체가 파괴된 경우에 대한 처리
                    GameObject avatarObject = Instantiate(avatarPrefab, avatarPos, Quaternion.identity);
                    avatarData.AvatarObject = avatarObject;
                    avatarList.Add(avatarData);
                }
            }
        }


    }

    // 아바타 생성 함수
    public void CreateAvatar(string avatarName, Vector3 pos)
    {
        // 아바타 생성, 아바타에 사용자의 DID 저장
        GameObject avatarObject = Instantiate(avatarPrefab, pos, Quaternion.identity);
        Debug.Log($"Avatar created at {pos}");

        Avatar avatar = avatarObject.GetComponent<Avatar>();
        avatar.InitializeAvatar(avatarName, did);

        AvatarData avatarData = new AvatarData(avatarObject, avatarName);
        avatarList.Add(avatarData);

    }

    public AvatarData GetAvatar(string avatarName)
    {
        foreach (AvatarData avatarData in avatarList)
        {
            if (avatarName == avatarData.AvatarID)
            {
                return avatarData;
            }
        }
        return null;
    }
}
