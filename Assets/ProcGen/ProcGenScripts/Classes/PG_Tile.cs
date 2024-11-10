using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.ProcGen.ProcGenScripts
{
    internal class PG_Tile : MonoBehaviour
    {
        public GameObject floorPrefab;
        public GameObject[] wallPrefabs; // Array of wall prefabs, where index 0 is a regular wall and index 1 is a door
        public GameObject[] propPrefabs; // Array of prop prefabs
        public GameObject[] enemyPrefabs; // Array of enemy prefabs
        public float propPlacementChance = 0.2f; // Default chance to place a prop (20%)
        public float enemyPlacementChance = 0.1f; // Default chance to place an enemy (10%)
        public int wallCount { get; private set; } // Public variable to show the number of walls implemented in the tile
        public bool isRoom = true; // Flag to indicate if the tile is a room or a hallway
        private const float wallOffset = 0.5f; // Adjust this value based on your wall's dimensions
        private const float verticalOffset = -2.0f; // Adjust this value to move the walls down
        private bool isExitDoorTile = false; // Flag to indicate if this tile is the exit door tile

        void Start()
        {
            Instantiate(floorPrefab, transform.position, Quaternion.identity, transform);
            PlaceProps();
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

        private void PlaceEnemies()
        {
            // Example rule: Place an enemy if the tile is a room, not an exit door tile, and based on the enemy placement chance
            if (isRoom && !isExitDoorTile && enemyPrefabs.Length > 0 && Random.value < enemyPlacementChance)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                Vector3 enemyPosition = transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0, Random.Range(-0.4f, 0.4f));
                Instantiate(enemyPrefabs[enemyIndex], enemyPosition, Quaternion.identity, transform);
                Debug.Log("Enemy placed at position: " + enemyPosition);
            }
        }
    }
}