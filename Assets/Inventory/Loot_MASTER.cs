using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public float weight;
    public int value;
    public GameObject itemPrefab; // For visual representation or dropping
}
