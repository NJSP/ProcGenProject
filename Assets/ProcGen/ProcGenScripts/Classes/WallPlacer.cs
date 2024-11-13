using UnityEngine;

public class WallPlacer : MonoBehaviour, IWallPlacer
{
    private GameObject[] wallPrefabs;
    private bool isExitDoorTile;
    private const float wallOffset = 0.5f;
    private const float verticalOffset = -2.0f;

    public WallPlacer(GameObject[] wallPrefabs, bool isExitDoorTile)
    {
        this.wallPrefabs = wallPrefabs;
        this.isExitDoorTile = isExitDoorTile;
    }

    public void PlaceWalls(PG_Tile[,] tiles, int x, int y, int width, int height)
    {
        // Check and place walls on each side of the tile
        if (x > 0 && tiles[x - 1, y] == null) // Left
        {
            PlaceWall(wallPrefabs[0], new Vector3(x - wallOffset, verticalOffset, y));
        }
        if (x < width - 1 && tiles[x + 1, y] == null) // Right
        {
            PlaceWall(wallPrefabs[1], new Vector3(x + wallOffset, verticalOffset, y));
        }
        if (y > 0 && tiles[x, y - 1] == null) // Bottom
        {
            PlaceWall(wallPrefabs[2], new Vector3(x, verticalOffset, y - wallOffset));
        }
        if (y < height - 1 && tiles[x, y + 1] == null) // Top
        {
            PlaceWall(wallPrefabs[3], new Vector3(x, verticalOffset, y + wallOffset));
        }
    }

    private void PlaceWall(GameObject wallPrefab, Vector3 position)
    {
        GameObject wall = GameObject.Instantiate(wallPrefab, position, Quaternion.identity);
        wall.transform.parent = this.transform;
    }
}
