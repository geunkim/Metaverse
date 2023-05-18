using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject uiPanel;
    public PlayerController moveScript;

    void Start()
    {
        moveScript = GetComponent<PlayerController>(); // Move3D 스크립트를 가져옴
        Button exitButton = uiPanel.GetComponentInChildren<Button>();
        exitButton.onClick.AddListener(ExitPanel);
    }

    void ExitPanel()
    {
        
    }
}
