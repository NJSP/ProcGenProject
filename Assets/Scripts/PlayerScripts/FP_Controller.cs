using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP_Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float baseSprintSpeed = 10f;
    public float maxSprintSpeed = 15f;
    public float acceleration = 0.5f;
    public float deceleration = 0.5f;
    public float crouchSpeed = 2.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 5f;

    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;
    private float xRotation = 0f;
    private float currentSpeed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
        HandleCrouch();
        HandleInteraction();
        HandleMouseLook();

        if (Input.GetKeyDown(KeyCode.P))
        {
            LogPlayerPosition();
        }
    }

    void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float speed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? baseSprintSpeed : walkSpeed);
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (move.magnitude > 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionLayer))
            {
                Debug.Log("Interacted with: " + hit.collider.name);
                // Add interaction logic here
            }
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    void LogPlayerPosition()
    {
        Vector3 playerPosition = transform.position;
        Debug.Log("Player position: " + playerPosition);
    }
}