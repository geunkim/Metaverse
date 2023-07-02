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

    public string GetAvatarID()
    {
        return avatarID;
    }
}
