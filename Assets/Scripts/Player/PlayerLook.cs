using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivityX = 200f;//pion
    public float sensitivityY = 200f;//poziom

    [Header("References")]
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 0f;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        // obrót kamery(w pionie)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // obrót postaci(w poziomie)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}