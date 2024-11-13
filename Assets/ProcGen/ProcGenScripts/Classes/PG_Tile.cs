using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PG_Tile : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject[] wallPrefabs;
    public GameObject[] propPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] corridorTrapPrefabs;
    public GameObject[] trapRoomTrapPrefabs;
    public float propPlacementChance = 0.2f;
    public float enemyPlacementChance = 0.1f;
    public float trapPlacementChance = 0.1f;
    public int wallCount { get; private set; }
    public bool isRoom = true;
    public enum roomType { Treasure, Lair, Trap, Normal };
    private bool isExitDoorTile = false;
    public float maxNavMeshDistance = 1.0f;
    private Transform[] enemySpawnPoints;
    public bool isTrapRoom = false;
    public bool isTreasureRoom = false;
    public bool isLairRoom = false;
    public bool isTeleportRoom = false;
    public bool isBossRoom = false;

    private IWallPlacer wallPlacer;
    private IPropPlacer propPlacer;
    private IEnemyPlacer enemyPlacer;
    private ITrapPlacer trapPlacer;

    void Start()
    {
        Instantiate(floorPrefab, transform.position, Quaternion.identity, transform);
        propPlacer.PlaceProps();
        GenerateSpawnPoints();
        enemyPlacer.PlaceEnemies();
    }

    public void SetAsExitDoorTile()
    {
        isExitDoorTile = true;
        Debug.Log("Exit door tile set at position: " + transform.position);
    }

    public void PlaceWalls(PG_Tile[,] tiles, int x, int y, int width, int height)
    {
        wallPlacer.PlaceWalls(tiles, x, y, width, height);
    }

    private void GenerateSpawnPoints()
    {
        List<Transform> spawnPoints = new List<Transform>();

        for (int i = 0; i < 1; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0, Random.Range(-0.4f, 0.4f));
            GameObject spawnPointObject = new GameObject("SpawnPoint");
            spawnPointObject.transform.position = randomPosition;
            spawnPointObject.transform.parent = transform;
            spawnPoints.Add(spawnPointObject.transform);
        }

        enemySpawnPoints = spawnPoints.ToArray();
    }

    bool FindValidNavMeshPosition(Vector3 sourcePosition, out Vector3 result)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(sourcePosition, out hit, maxNavMeshDistance, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = sourcePosition;
        return false;
    }
}