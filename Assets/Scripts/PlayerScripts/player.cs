using Assets.ProcGen.ProcGenScripts;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private InventorySystem inventory;
    private HealthSystem healthSystem;

    public float speed = 5f;
    public float jumpHeight = 2f;
    private Vector3 moveDirection;

    void Start()
    {
        // Get a reference to the CharacterController and InventorySystem components
        characterController = GetComponent<CharacterController>();
        inventory = GetComponent<InventorySystem>();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Gather input and pass it to movement logic
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        moveDirection = move * speed;

        // Example: Jump logic
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        // Apply gravity
        moveDirection.y += Physics.gravity.y * Time.deltaTime;

        // Pass movement to CharacterController
        characterController.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.I))
        {
            // Open inventory
            inventory.OpenInventory();
        }
    }

    public void Interact()
    {
        // Player-specific logic for interacting with items
        
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public bool CollectItem(InventoryItem item)
    {
        if (inventory.currentWeight + item.weight <= inventory.maxCapacity)
        {
            inventory.AddItem(item);
            return true;
        }
        return false;
    }
}
