using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HttpClient httpClient;

    public Dictionary<string, DidUser> didUserDictionary;

    public DidSystem didSystem;

    // Start is called before the first frame update

    void Awake()
    {
        GetInstance();
        didSystem = this.GetComponent<DidSystem>();
        httpClient = this.GetComponent<HttpClient>();
        didUserDictionary = new Dictionary<string, DidUser>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");

            foreach (KeyValuePair<string, DidUser> pair in didUserDictionary)
            {
                pair.Value.CleanWallet();
            }

            Application.Quit();
        }
    }

    public static GameManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<GameManager>();
            if (Instance == null)
            {
                GameObject container = new("GameManager");
                Instance = container.AddComponent<GameManager>();
            }
        }
        return Instance;
    }

    public DidSystem GetDidSystem()
    {
        return didSystem;
    }
}
