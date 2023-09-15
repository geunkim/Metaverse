using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // ����ٴ� ���
    public float distance = 5.0f; // ī�޶�� ����� �Ÿ�
    public float height = 2.0f; // ī�޶�� ����� ����
    public float sensitivity = 2.0f; // ���콺 ����

    private float rotX, rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // ���콺 �Է� �ޱ�
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY += Input.GetAxis("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        // ī�޶� ȸ��
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);
        }

        // ī�޶� ��ġ
        Vector3 offset = new Vector3(0f, height, -distance);
        transform.position = target.position + transform.rotation * offset;
    }
}
