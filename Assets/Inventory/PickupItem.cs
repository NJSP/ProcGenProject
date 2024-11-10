using Assets.ProcGen.ProcGenScripts;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItem itemData; // Reference to the item's data (ScriptableObject or data class)
    public float pickupRadius = 1f;
    public AudioClip[] pickupSounds; // Array of AudioClips
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component missing on " + gameObject.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                bool pickedUp = player.CollectItem(itemData);
                if (pickedUp)
                {
                    Debug.Log("Picked up item: " + itemData.itemName);
                    if (pickupSounds == null || pickupSounds.Length == 0)
                    {
                        Debug.LogError("Pickup sounds are not assigned.");
                    }
                    if (audioSource == null)
                    {
                        Debug.LogError("AudioSource is not assigned.");
                    }
                    if (pickupSounds != null && pickupSounds.Length > 0 && audioSource != null)
                    {
                        AudioClip randomClip = pickupSounds[Random.Range(0, pickupSounds.Length)];
                        Debug.Log("Playing pickup sound: " + randomClip.name);
                        audioSource.PlayOneShot(randomClip);
                        Debug.Log("AudioSource is playing: " + audioSource.isPlaying);
                        Destroy(gameObject, randomClip.length); // Delay destruction until the clip finishes playing
                    }
                    else
                    {
                        Destroy(gameObject); // Remove item from the world after pickup
                    }
                }
            }
        }
    }
}