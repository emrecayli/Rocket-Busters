using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 2f;
    public float rollSpeed = 50f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        MoveShip();
        RotateShip();
    }

    void MoveShip()
    {
        float moveForward = Input.GetAxis("Vertical");
        float moveStrafe = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.forward * moveForward + transform.right * moveStrafe;
        rb.velocity = moveDirection * moveSpeed;
    }

    void RotateShip()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float roll = 0f;
        if (Input.GetKey(KeyCode.Q)) roll = 1f;
        if (Input.GetKey(KeyCode.E)) roll = -1f;

        Vector3 rotation = new Vector3(-mouseY, mouseX, roll) * rotationSpeed;
        rb.angularVelocity = rotation * Mathf.Deg2Rad * rollSpeed;
    }
}