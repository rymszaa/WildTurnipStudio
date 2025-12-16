using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovment : MonoBehaviour
{
    [SerializeField] GameObject playerBody;
    [SerializeField] float walkSpeed = 10f;

    public float mouseSensitivity = 100f;

    Vector2 moveInput;
    Rigidbody myRigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myRigidbody = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
void FixedUpdate()
    {
        //mouse move
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        playerBody.transform.Rotate(Vector3.up * mouseX);


        //move
        Vector3 move =
            myRigidbody.transform.forward * moveInput.y +
            myRigidbody.transform.right * moveInput.x;

        myRigidbody.MovePosition(
            myRigidbody.position + move * walkSpeed * Time.fixedDeltaTime
        );
    }


    void OnMove(InputValue value)
    {
        moveInput=value.Get<Vector2>();
    }
}
