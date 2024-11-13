using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace Assets.ProcGen.ProcGenScripts
{
    internal class PG_Tile : MonoBehaviour
    {
        public GameObject floorPrefab;
        public GameObject[] wallPrefabs; // Array of wall prefabs, where index 0 is a regular wall and index 1 is a door
        public GameObject[] propPrefabs; // Array of prop prefabs
        public GameObject[] enemyPrefabs; // Array of enemy prefabs
        public GameObject[] corridorTrapPrefabs; // Array of corridor trap prefabs
        public GameObject[] trapRoomTrapPrefabs; // Array of trap room trap pref
        public float propPlacementChance = 0.2f; // Default chance to place a prop (20%)
        public float enemyPlacementChance = 0.1f; // Default chance to place an enemy (10%)
        public float trapPlacementChance = 0.1f; // Default chance to place a trap
        public int wallCount { get; private set; } // Public variable to show the number of walls implemented in the tile
        public bool isRoom = true; // Flag to indicate if the tile is a room or a hallway
        public enum roomType { Treasure, Lair, Trap, Normal };
        private const float wallOffset = 0.5f; // Adjust this value based on your wall's dimensions
        private const float verticalOffset = -2.0f; // Adjust this value to move the walls down
        private bool isExitDoorTile = false; // Flag to indicate if this tile is the exit door tile
        public float maxNavMeshDistance = 1.0f; // Maximum distance to search for a valid NavMesh position
        private Transform[] enemySpawnPoints;
        public bool isTrapRoom = false; // Flag to indicate if the room is a trap room
        public bool isTreasureRoom = false; // Flag to indicate if the room is a treasure room
        public bool isLairRoom = false; // Flag to indicate if the room is a lair room
        public bool isTeleportRoom = false;
        public bool isBossRoom = false;

        void Start()
        {
            Instantiate(floorPrefab, transform.position, Quaternion.identity, transform);
            PlaceProps();
            GenerateSpawnPoints();
            PlaceEnemies();
        }

        public void SetAsExitDoorTile()
        {
            isExitDoorTile = true;
            Debug.Log("Exit door tile set at position: " + transform.position);
        }

        public void PlaceWalls(PG_Tile[,] tiles, int x, int y, int width, int height)
        {
            bool hasWallTop = y + 1 >= height || tiles[x, y + 1] == null;
            bool hasWallBottom = y - 1 < 0 || tiles[x, y - 1] == null;
            bool hasWallLeft = x - 1 < 0 || tiles[x - 1, y] == null;
            bool hasWallRight = x + 1 >= width || tiles[x + 1, y] == null;

            bool usedExitDoor = false; // Flag to check if exit door is used
            wallCount = 0; // Reset wall count

            if (hasWallTop)
            {
                int wallIndex = isExitDoorTile && !usedExitDoor ? 1 : 0;
                float offset = wallIndex == 1 ? 0 : verticalOffset; // No vertical offset for the exit door
                Instantiate(wallPrefabs[wallIndex], transform.position + Vector3.forward * wallOffset + Vector3.up * offset, Quaternion.Euler(0, 270, 0), transform);
                Debug.Log("Wall placed at top with prefab index: " + wallIndex);
                if (wallIndex == 1) usedExitDoor = true;
                wallCount++;
            }
            if (hasWallBottom)
            {
                int wallIndex = isExitDoorTile && !usedExitDoor ? 1 : 0;
                float offset = wallIndex == 1 ? 0 : verticalOffset; // No vertical offset for the exit door
                Instantiate(wallPrefabs[wallIndex], transform.position - Vector3.forward * wallOffset + Vector3.up * offset, Quaternion.Euler(0, 90, 0), transform);
                Debug.Log("Wall placed at bottom with prefab index: " + wallIndex);
                if (wallIndex == 1) usedExitDoor = true;
                wallCount++;
            }
            if (hasWallLeft)
            {
                int wallIndex = isExitDoorTile && !usedExitDoor ? 1 : 0;
                float offset = wallIndex == 1 ? 0 : verticalOffset; // No vertical offset for the exit door
                Instantiate(wallPrefabs[wallIndex], transform.position - Vector3.right * wallOffset + Vector3.up * offset, Quaternion.Euler(0, 180, 0), transform);
                Debug.Log("Wall placed at left with prefab index: " + wallIndex);
                if (wallIndex == 1) usedExitDoor = true;
                wallCount++;
            }
            if (hasWallRight)
            {
                int wallIndex = isExitDoorTile && !usedExitDoor ? 1 : 0;
                float offset = wallIndex == 1 ? 0 : verticalOffset; // No vertical offset for the exit door
                Instantiate(wallPrefabs[wallIndex], transform.position + Vector3.right * wallOffset + Vector3.up * offset, Quaternion.Euler(0, 0, 0), transform);
                Debug.Log("Wall placed at right with prefab index: " + wallIndex);
                if (wallIndex == 1) usedExitDoor = true;
                wallCount++;
            }
        }

        private void PlaceProps()
        {
            // Example rule: Place a prop if the tile is a room, not an exit door tile, and based on the prop placement chance
            if (isRoom && !isExitDoorTile && propPrefabs.Length > 0 && Random.value < propPlacementChance && wallCount == 1)
            {
                int propIndex = Random.Range(0, propPrefabs.Length);
                Vector3 propPosition = transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0, Random.Range(-0.4f, 0.4f));
                Instantiate(propPrefabs[propIndex], propPosition, Quaternion.identity, transform);
                Debug.Log("Prop placed at position: " + propPosition);
            }
        }

        private void GenerateSpawnPoints()
        {
            List<Transform> spawnPoints = new List<Transform>();

            // Example: Generate 3 random spawn points within the tile
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

        void PlaceEnemies()
        {
            foreach (Transform spawnPoint in enemySpawnPoints)
            {
                if (Random.value < enemyPlacementChance) // Check if an enemy should be placed based on the placement chance
                {
                    Vector3 validPosition;
                    if (FindValidNavMeshPosition(spawnPoint.position, out validPosition))
                    {
                        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                        Instantiate(enemyPrefabs[enemyIndex], validPosition, Quaternion.identity, transform);
                    }
                    else
                    {
                        Debug.LogWarning("Failed to find a valid NavMesh position for enemy at " + spawnPoint.position);
                    }
                }
            }
        }

        private void PlaceTraps()
        {
            if (!isExitDoorTile && Random.value < trapPlacementChance)
            {
                if (!isRoom && corridorTrapPrefabs.Length > 0) // Place corridor traps
                {
                    int trapIndex = Random.Range(0, corridorTrapPrefabs.Length);
                    Vector3 trapPosition = transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0, Random.Range(-0.4f, 0.4f));
                    Instantiate(corridorTrapPrefabs[trapIndex], trapPosition, Quaternion.identity, transform);
                    Debug.Log("Corridor trap placed at position: " + trapPosition);
                }
                else if (isRoom && isTrapRoom == true && trapRoomTrapPrefabs.Length > 0) // Place trap room traps
                {
                    int trapIndex = Random.Range(0, trapRoomTrapPrefabs.Length);
                    Vector3 trapPosition = transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0, Random.Range(-0.4f, 0.4f));
                    Instantiate(trapRoomTrapPrefabs[trapIndex], trapPosition, Quaternion.identity, transform);
                    Debug.Log("Trap room trap placed at position: " + trapPosition);
                }
            }
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
}