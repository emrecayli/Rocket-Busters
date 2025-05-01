using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 20f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float dashDistance = 10f;
    public float dashCooldown = 3f;
    public float cameraBobSpeed = 5f;
    public float cameraBobAmount = 0.1f;
    public Image dashCooldownImage;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;
    private bool isDashing = false;
    private float lastDashTime;
    private Vector3 originalCameraPosition;
    private float bobTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalCameraPosition = playerCamera.transform.localPosition;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        bool isMoving = Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(horizontalInput) > 0.1f;
        bool isMovingBackward = verticalInput < 0f;

        float speed = isRunning ? runSpeed : walkSpeed;
        if (isMovingBackward)
        {
            speed = walkSpeed * 0.8f; // Geri hareket daha yavaş
        }

        float curSpeedX = canMove ? speed * verticalInput : 0;
        float curSpeedY = canMove ? speed * horizontalInput : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = -1f;
            }
        }
        else
        {
            moveDirection.y = movementDirectionY - (gravity * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }
        
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        UpdateDashCooldownUI();
        characterController.Move(moveDirection * Time.deltaTime);
        ApplyCameraBob(isMoving, isRunning);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        Vector3 dashDirection = transform.forward * dashDistance;
        float dashDuration = 0.2f;
        float dashSpeed = dashDistance / dashDuration;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }

    void ApplyCameraBob(bool isMoving, bool isRunning)
    {
        if (isMoving)
        {
            bobTimer += Time.deltaTime * cameraBobSpeed * (isRunning ? 1.5f : 1f);
            float bobOffset = Mathf.Sin(bobTimer) * cameraBobAmount * (isRunning ? 1.5f : 1f);
            playerCamera.transform.localPosition = originalCameraPosition + new Vector3(0, bobOffset, 0);
        }
        else
        {
            bobTimer = 0f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalCameraPosition, Time.deltaTime * cameraBobSpeed);
        }
    }

    void UpdateDashCooldownUI()
    {
        if (dashCooldownImage != null)
        {
            float cooldownRemaining = Mathf.Clamp(dashCooldown - (Time.time - lastDashTime), 0, dashCooldown);
            float alpha = cooldownRemaining / dashCooldown;
            dashCooldownImage.color = new Color(1f, 1f, 1f, 1f - alpha);
        }
    }
}
