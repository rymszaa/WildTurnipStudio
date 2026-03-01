using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 25f;
    public float airControlPercent = 0.1f; // 10% kontroli w powietrzu
    public float groundFriction = 8f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector2 moveInput;

    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // NEW INPUT SYSTEM (Send Messages)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (IsGrounded())
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        HandleHorizontalMovement();
        ApplyGravity();

        Vector3 finalVelocity = horizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(finalVelocity * Time.deltaTime);
    }

    void HandleHorizontalMovement()
    {
        Vector3 inputDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        inputDirection.Normalize();

        bool grounded = IsGrounded();

        float controlMultiplier = grounded ? 1f : airControlPercent;
        Vector3 targetVelocity = inputDirection * moveSpeed;

        // Przyspieszanie (inercja)
        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetVelocity,
            acceleration * controlMultiplier * Time.deltaTime
        );

        // Tarcie tylko na ziemi
        if (grounded && moveInput.magnitude < 0.1f)
        {
            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                Vector3.zero,
                groundFriction * Time.deltaTime
            );
        }
    }

    void ApplyGravity()
    {
        if (IsGrounded() && verticalVelocity <= 0f)
        {
            verticalVelocity = -2f; // lekki docisk do ziemi
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    bool IsGrounded()
    {
        // Lepsze wykrywanie niż samo controller.isGrounded
        return controller.isGrounded && verticalVelocity <= 0f;
    }
}