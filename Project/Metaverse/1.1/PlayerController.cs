using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // �̵� �ӵ�
    public float sensitivity = 2.0f; // ���콺 ����

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
        // �̵� �Է� �ޱ�
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        // �̵� ����
        Vector3 movement = transform.forward * moveFB + transform.right * moveLR;
        rb.MovePosition(transform.position + movement * Time.deltaTime);

        // �̵� �ִϸ��̼� ����
        if (movement.magnitude > 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        // ȸ�� �Է� �ޱ�
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY += Input.GetAxis("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        // ȸ�� ����
        transform.localRotation = Quaternion.Euler(0f, rotX, 0f);
        if (cam != null)
        {
            cam.transform.localRotation = Quaternion.Euler(-rotY, 0f, 0f);
        }
        else
        {
            Debug.Log("Camera not found");
        }
    }

    // ī�޶� ���� �Լ�
    public void SetCamera(Camera camera)
    {
        cam = camera;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Switch"))
        {
            animator.SetBool("Walk", false);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            
            Cursor.visible = true; // ���콺 Ŀ���� ���̰� ��
            Cursor.lockState = CursorLockMode.None; // ���콺 Ŀ���� �������� �ʵ��� ��
            playerController.enabled = false;
        }
    }
}
