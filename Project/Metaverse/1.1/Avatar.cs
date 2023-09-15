using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private string avatarID;       // �ƹ�Ÿ �ĺ���
    private string userDID;        // ����� DID

    // �ƹ�Ÿ �ʱ�ȭ �Լ�
    public void InitializeAvatar(string avatarID, string userDID)
    {
        this.avatarID = avatarID;
        this.userDID = userDID;
    }

    // �ƹ�Ÿ ���� �Լ�
    public void PerformAction()
    {
        // �ƹ�Ÿ�� ������ ����
        // ��: �ſ� ������ �ʿ��� ������ ������ �� UserDID�� ����Ͽ� �ſ��� ����
        if (string.IsNullOrEmpty(userDID))
        {
            Debug.LogError("UserDID is not set for the avatar!");
            return;
        }

        // �ƹ�Ÿ�� ������ �����ϴ� ���� UserDID�� ����Ͽ� �ſ� ����
        Debug.Log($"Performing action with UserDID: {userDID}");
        // ... ���� ���� �ڵ� ...
    }

    public string GetAvatarID()
    {
        return avatarID;
    }
}
