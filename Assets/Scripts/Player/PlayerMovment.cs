using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 10f;

    [Header("Mouse Sensitivity")]
    [SerializeField] float horizontalSensitivity = 250f;
    [SerializeField] float verticalSensitivity = 200f;

    [Header("Camera")]
    [SerializeField] Transform cameraPivot;

    Vector2 moveInput;
    float mouseXAccum;
    float mouseYAccum;
    float pitch;

    Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Accumulate mouse input every frame
        mouseXAccum += Input.GetAxis("Mouse X") * horizontalSensitivity;
        mouseYAccum += Input.GetAxis("Mouse Y") * verticalSensitivity;
    }

    void FixedUpdate()
    {
        // --- Yaw (body rotation) ---
        float yawStep = mouseXAccum * Time.fixedDeltaTime;
        mouseXAccum = 0f;

        rb.MoveRotation(
            rb.rotation * Quaternion.Euler(0f, yawStep, 0f)
        );

        // --- Pitch (camera only) ---
        float pitchStep = mouseYAccum * Time.fixedDeltaTime;
        mouseYAccum = 0f;

        pitch -= pitchStep;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // --- Movement ---
        Vector3 move =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        Vector3 velocity = move * walkSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}