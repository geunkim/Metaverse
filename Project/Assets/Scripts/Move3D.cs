using UnityEngine;

public class Move3D : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도
    public float sensitivity = 2.0f; // 마우스 감도

    private Rigidbody rb;
    private Camera cam;
    private float moveFB, moveLR;
    private float rotX, rotY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 이동 입력 받기
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        // 회전 입력 받기
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY += Input.GetAxis("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        // 회전 적용
        transform.localRotation = Quaternion.Euler(0f, rotX, 0f);
        cam.transform.localRotation = Quaternion.Euler(-rotY, 0f, 0f);

        // 이동 적용
        Vector3 movement = transform.forward * moveFB + transform.right * moveLR;
        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }
}
