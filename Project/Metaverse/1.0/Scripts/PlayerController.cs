using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도
    public float sensitivity = 2.0f; // 마우스 감도

    private Rigidbody rb;
    private Camera cam;
    private Animator animator;
    private float moveFB, moveLR;
    private float rotX, rotY;

    PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();

        playerController = gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        // 이동 입력 받기
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        // 이동 적용
        Vector3 movement = transform.forward * moveFB + transform.right * moveLR;
        rb.MovePosition(transform.position + movement * Time.deltaTime);

        // 이동 애니메이션 적용
        if (movement.magnitude > 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        // 회전 입력 받기
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY += Input.GetAxis("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        // 회전 적용
        transform.localRotation = Quaternion.Euler(0f, rotX, 0f);
        cam.transform.localRotation = Quaternion.Euler(-rotY, 0f, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Switch"))
        {
            animator.SetBool("Walk", false);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            
            Cursor.visible = true; // 마우스 커서를 보이게 함
            Cursor.lockState = CursorLockMode.None; // 마우스 커서가 고정되지 않도록 함
            playerController.enabled = false;
        }
    }
}
