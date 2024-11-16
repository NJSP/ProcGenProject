using UnityEngine;

public class SmoothSwingingAxeTrap : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingAngle = 45f; // Maximum angle the axe swings (degrees)
    public float swingSpeed = 1f; // Speed of the swinging motion (cycles per second)
    public float dampingFactor = 0.98f; // Multiplier to reduce swing amplitude over time (optional)

    private float currentAngle = 0f; // Current angle of the swing
    private float time; // Internal time tracker
    private float randomOffset; // Random initial phase offset
    private bool isSwinging = true;

    void Start()
    {
        // Initialize the random phase offset
        randomOffset = Random.Range(0f, Mathf.PI * 2); // Randomize the starting phase

        // Initialize time
        time = randomOffset;
    }

    void Update()
    {
        if (isSwinging)
        {
            // Increment time based on swing speed
            time += Time.deltaTime * swingSpeed;

            // Calculate the new angle using a sinusoidal function with the random offset
            float angle = Mathf.Sin(time) * swingAngle;

            // Apply damping if needed
            swingAngle *= dampingFactor;

            // Rotate the trap to the calculated angle
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Reset to initial swing amplitude if it gets too low
            if (swingAngle < 5f) swingAngle = 45f; 

        }
    }
}
