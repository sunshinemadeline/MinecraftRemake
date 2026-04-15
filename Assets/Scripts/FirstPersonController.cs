using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    //Character Movement
   public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 0.9f;
    public float gravity = -200f; 

    //Mouse Movement
    public Transform cameraPivot;
    public float Sensitivity = 50f;
    public float minPitch = -50f;
    public float maxPitch = 50f;

    //Ground stuff for checking if player is on ground
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask = ~0;

    private CharacterController controller;
    private float pitch = 0f;
    private float verticalVelocity = 0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseLook();
        PlayerMovement();
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * Sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Sensitivity;

        //yaw: left/right
        transform.Rotate(Vector3.up * mouseX);

        //pitch: up/down
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void PlayerMovement()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.forward * vertical + transform.right * horizontal;
        move = move.normalized;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 horizontalMove = move * currentSpeed;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMove = horizontalMove;
        finalMove.y = verticalVelocity;

        controller.Move(finalMove * Time.deltaTime);
    }

    bool IsOnGround()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float rayLength = (controller.height * 0.5f) + groundCheckDistance;

        return Physics.Raycast(rayOrigin, Vector3.down, rayLength, groundMask);
    }
}
