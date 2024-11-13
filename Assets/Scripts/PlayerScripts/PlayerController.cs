// 11/12/2024 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using System;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 0.5f;
    public float deceleration = 0.5f;

    private float currentSpeed;
    private Vector3 moveDirection;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Gather input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Adjust speed based on movement
        if (moveDirection.magnitude > 0)
        {
            // Accelerate
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        }
        else
        {
            // Decelerate
            currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, baseSpeed);
        }

        // Apply movement
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);
    }
}
