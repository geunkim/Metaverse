using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 따라다닐 대상
    public float distance = 5.0f; // 카메라와 대상의 거리
    public float height = 2.0f; // 카메라와 대상의 높이
    public float sensitivity = 2.0f; // 마우스 감도

    private float rotX, rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // 마우스 입력 받기
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY += Input.GetAxis("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        // 카메라 회전
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);
        }

        // 카메라 위치
        Vector3 offset = new Vector3(0f, height, -distance);
        transform.position = target.position + transform.rotation * offset;
    }
}
