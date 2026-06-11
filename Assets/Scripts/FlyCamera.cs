using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float boostMultiplier = 2f;
    public float mouseSensitivity = 2f;
    public float smoothTime = 0.05f;

    float yaw;
    float pitch;

    Vector3 velocity;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        HandleCursor();

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            HandleMouseLook();
            HandleMovement();
        }
    }

    void HandleCursor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * mouseSensitivity * 100f * Time.deltaTime;
        pitch -= mouseY * mouseSensitivity * 100f * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void HandleMovement()
    {
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= boostMultiplier;

        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        Vector3 move = transform.TransformDirection(input);

        if (Input.GetKey(KeyCode.E)) move += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) move += Vector3.down;

        Vector3 targetPosition = transform.position + move * speed * Time.deltaTime;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}