using myspace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AvatarData
{
    public GameObject AvatarObject { get; set; }
    public string AvatarID { get; set; }
    // �߰����� �ƹ�Ÿ ���� �ʵ��...

    public AvatarData(GameObject avatarObject, string avatarID)
    {
        AvatarObject = avatarObject;
        AvatarID = avatarID;
    }
}

public class Game_Manager : MonoBehaviour
{
    private UserDID userDID; // UserDID ��ü ����
    private List<AvatarData> avatarList; // �ƹ�Ÿ ����Ʈ
    private static Game_Manager instance;
    private string did;          // ����� DID
    private string avatarID;       // �ƹ�Ÿ �ĺ���
    private Vector3 avatarPos;

    //public Button button;   // �α��� ��ư
    public InputField inputField;   // �ƹ�Ÿ �ĺ��� �Է� �ʵ�
    public GameObject avatarPrefab;   // �ƹ�Ÿ ��ü ����
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
        // ���� ���� �� UserDID ��ü ã�Ƽ� ����
        userDID = FindObjectOfType<UserDID>();
        //button.onClick.AddListener(OnLoginButtonClick);
        inputField.onEndEdit.AddListener(OnEndEdit);
        avatarList = new List<AvatarData>();
    }

    // �α��� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnLoginButtonClick()
    {
        // UserDID�� GetDID() ȣ���Ͽ� ����ڿ��� DID �ο�
        //userDID.GenerateDID();
        //did = userDID.GetDID();
        did = "asdasd";
        Debug.Log($"User DID: {did}");

        // load scene "Outside"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Outside");
    }

    // �ƹ�Ÿ ID �Է� �� ����
    private void OnEndEdit(string value)
    {
        // �Էµ� ��(value)�� ������ ����
        avatarID = value;
        Debug.Log("Avatar ID: " + avatarID);

        // load scene "Outside"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Outside");
    }

    // �� ��ȯ �Ŀ� ȣ��Ǵ� �̺�Ʈ
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // �� ��ȯ ���� ȣ��Ǵ� �̺�Ʈ
    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ���� ��ȯ�� �Ŀ� ȣ��Ǵ� �ݹ� �Լ�
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "Outside")
        {
            // �ƹ�Ÿ �ĺ��� ����, ���� ĭ �Է����� ��ü
            avatarPos = new Vector3(230.07f, 18.0f, 165.0f);
            CreateAvatar(avatarID, avatarPos);
        }

        if (scene.name == "Inside")
        {
            // �ƹ�Ÿ ����
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
                    // �ƹ�Ÿ ��ü�� �ı��� ��쿡 ���� ó��
                    GameObject avatarObject = Instantiate(avatarPrefab, avatarPos, Quaternion.identity);
                    avatarData.AvatarObject = avatarObject;
                    avatarList.Add(avatarData);
                }
            }
        }


    }

    // �ƹ�Ÿ ���� �Լ�
    public void CreateAvatar(string avatarName, Vector3 pos)
    {
        // �ƹ�Ÿ ����, �ƹ�Ÿ�� ������� DID ����
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
