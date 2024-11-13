using UnityEngine;

public class PropPlacer : MonoBehaviour, IPropPlacer
{
    private GameObject[] propPrefabs;
    private float propPlacementChance;
    private bool isRoom;
    private bool isExitDoorTile;
    private int wallCount;

    public PropPlacer(GameObject[] propPrefabs, float propPlacementChance, bool isRoom, bool isExitDoorTile, int wallCount)
    {
        this.propPrefabs = propPrefabs;
        this.propPlacementChance = propPlacementChance;
        this.isRoom = isRoom;
        this.isExitDoorTile = isExitDoorTile;
        this.wallCount = wallCount;
    }

    public void PlaceProps()
    {
        if (propPrefabs == null || propPrefabs.Length == 0)
        {
            Debug.LogWarning("No prop prefabs provided.");
            return;
        }

        if (isRoom && !isExitDoorTile && wallCount < 2)
        {
            if (Random.value <= propPlacementChance)
            {
                int randomIndex = Random.Range(0, propPrefabs.Length);
                GameObject propToPlace = propPrefabs[randomIndex];
                Vector3 position = transform.position; // Assuming this script is attached to a GameObject
                Quaternion rotation = Quaternion.identity; // Default rotation

                GameObject.Instantiate(propToPlace, position, rotation);
            }
        }
    }
}
