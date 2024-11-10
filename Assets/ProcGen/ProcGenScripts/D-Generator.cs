using Assets.ProcGen.ProcGenScripts;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public int minRoomSize = 6;
    public int maxRoomSize = 12;
    public int maxSplits = 4;
    public int minCorridorLength = 1;
    public int maxCorridorLength = 5;
    public GameObject tilePrefab;
    public GameObject playerPrefab; // Reference to the player prefab
    public GameObject goalPrefab; // Reference to the goal item prefab

    private BSPNode root;
    private PG_Tile[,] tiles;
    private GameObject playerInstance;

    void Start()
    {
        GenerateDungeon();
        PlaceExitDoor(); // Ensure the exit door tile is declared before placing walls
        PlaceWalls();
        PlacePlayer();
        PlaceGoalItem();
    }

    void GenerateDungeon()
    {
        root = new BSPNode(new RectInt(0, 0, width, height));
        tiles = new PG_Tile[width, height];
        Split(root, maxSplits);
        CreateRooms(root);
        ConnectRooms(root);
    }

    void Split(BSPNode node, int splits)
    {
        if (splits <= 0 || node.Rect.width <= minRoomSize * 2 || node.Rect.height <= minRoomSize * 2)
            return;

        bool splitHorizontally = Random.value > 0.5f;
        if (node.Rect.width > node.Rect.height && node.Rect.width / node.Rect.height >= 1.25f)
            splitHorizontally = false;
        else if (node.Rect.height > node.Rect.width && node.Rect.height / node.Rect.width >= 1.25f)
            splitHorizontally = true;

        int max = (splitHorizontally ? node.Rect.height : node.Rect.width) - minRoomSize;
        if (max <= minRoomSize)
            return;

        int split = Random.Range(minRoomSize, max);

        if (splitHorizontally)
        {
            node.Left = new BSPNode(new RectInt(node.Rect.x, node.Rect.y, node.Rect.width, split));
            node.Right = new BSPNode(new RectInt(node.Rect.x, node.Rect.y + split, node.Rect.width, node.Rect.height - split));
        }
        else
        {
            node.Left = new BSPNode(new RectInt(node.Rect.x, node.Rect.y, split, node.Rect.height));
            node.Right = new BSPNode(new RectInt(node.Rect.x + split, node.Rect.y, node.Rect.width - split, node.Rect.height));
        }

        Split(node.Left, splits - 1);
        Split(node.Right, splits - 1);
    }

    void CreateRooms(BSPNode node)
    {
        if (node.Left == null && node.Right == null)
        {
            int roomWidth = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, node.Rect.width));
            int roomHeight = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, node.Rect.height));
            int roomX = Random.Range(0, node.Rect.width - roomWidth);
            int roomY = Random.Range(0, node.Rect.height - roomHeight);
            node.Room = new RectInt(node.Rect.x + roomX, node.Rect.y + roomY, roomWidth, roomHeight);

            // Instantiate tiles for the room
            for (int x = node.Room.x; x < node.Room.x + node.Room.width; x++)
            {
                for (int y = node.Room.y; y < node.Room.y + node.Room.height; y++)
                {
                    var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity).GetComponent<PG_Tile>();
                    tiles[x, y] = tile;
                }
            }
        }
        else
        {
            if (node.Left != null)
                CreateRooms(node.Left);
            if (node.Right != null)
                CreateRooms(node.Right);
        }
    }

    void ConnectRooms(BSPNode node)
    {
        if (node.Left != null && node.Right != null)
        {
            ConnectRooms(node.Left);
            ConnectRooms(node.Right);

            RectInt leftRoom = GetRoom(node.Left);
            RectInt rightRoom = GetRoom(node.Right);

            Vector2Int leftCenter = new Vector2Int(leftRoom.x + leftRoom.width / 2, leftRoom.y + leftRoom.height / 2);
            Vector2Int rightCenter = new Vector2Int(rightRoom.x + rightRoom.width / 2, rightRoom.y + rightRoom.height / 2);

            CreateCorridor(leftCenter, rightCenter);
        }
    }

    RectInt GetRoom(BSPNode node)
    {
        if (node.Room.width != 0 && node.Room.height != 0)
            return node.Room;
        else
        {
            RectInt leftRoom = node.Left != null ? GetRoom(node.Left) : new RectInt();
            RectInt rightRoom = node.Right != null ? GetRoom(node.Right) : new RectInt();
            return leftRoom.width != 0 && leftRoom.height != 0 ? leftRoom : rightRoom;
        }
    }

    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int current = start;

        while (current != end)
        {
            if (Random.value > 0.5f)
            {
                int length = Random.Range(minCorridorLength, maxCorridorLength);
                for (int i = 0; i < length && current.x != end.x; i++)
                {
                    current.x += (end.x > current.x) ? 1 : -1;
                    CreateTile(current);
                }
            }
            else
            {
                int length = Random.Range(minCorridorLength, maxCorridorLength);
                for (int i = 0; i < length && current.y != end.y; i++)
                {
                    current.y += (end.y > current.y) ? 1 : -1;
                    CreateTile(current);
                }
            }
        }
    }

    void CreateTile(Vector2Int position)
    {
        if (tiles[position.x, position.y] == null)
        {
            var tile = Instantiate(tilePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity).GetComponent<PG_Tile>();
            tiles[position.x, position.y] = tile;
        }
    }

    void PlaceWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] != null)
                {
                    tiles[x, y].PlaceWalls(tiles, x, y, width, height);
                }
            }
        }
    }

    void PlacePlayer()
    {
        if (playerPrefab != null)
        {
            RectInt firstRoom = GetRoom(root);
            Vector3 playerPosition = new Vector3(firstRoom.x + firstRoom.width / 2, 1, firstRoom.y + firstRoom.height / 2);
            playerInstance = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }
    }

    void PlaceGoalItem()
    {
        if (goalPrefab != null)
        {
            List<RectInt> rooms = GetAllRooms(root);
            RectInt randomRoom = rooms[Random.Range(0, rooms.Count)];
            Vector3 goalPosition = new Vector3(randomRoom.x + randomRoom.width / 2, 1, randomRoom.y + randomRoom.height / 2);
            Instantiate(goalPrefab, goalPosition, Quaternion.identity);
        }
    }

    void PlaceExitDoor()
    {
        RectInt firstRoom = GetRoom(root);
        Vector3 exitPosition = Vector3.zero;
        PG_Tile exitTile = null;

        // Check the top edge
        for (int x = firstRoom.x; x < firstRoom.x + firstRoom.width; x++)
        {
            if (tiles[x, firstRoom.y + firstRoom.height - 1] != null)
            {
                exitTile = tiles[x, firstRoom.y + firstRoom.height - 1];
                exitPosition = new Vector3(x, 1, firstRoom.y + firstRoom.height - 1);
                break;
            }
        }

        // Check the bottom edge
        if (exitTile == null)
        {
            for (int x = firstRoom.x; x < firstRoom.x + firstRoom.width; x++)
            {
                if (tiles[x, firstRoom.y] != null)
                {
                    exitTile = tiles[x, firstRoom.y];
                    exitPosition = new Vector3(x, 1, firstRoom.y);
                    break;
                }
            }
        }

        // Check the left edge
        if (exitTile == null)
        {
            for (int y = firstRoom.y; y < firstRoom.y + firstRoom.height; y++)
            {
                if (tiles[firstRoom.x, y] != null)
                {
                    exitTile = tiles[firstRoom.x, y];
                    exitPosition = new Vector3(firstRoom.x, 1, y);
                    break;
                }
            }
        }

        // Check the right edge
        if (exitTile == null)
        {
            for (int y = firstRoom.y; y < firstRoom.y + firstRoom.height; y++)
            {
                if (tiles[firstRoom.x + firstRoom.width - 1, y] != null)
                {
                    exitTile = tiles[firstRoom.x + firstRoom.width - 1, y];
                    exitPosition = new Vector3(firstRoom.x + firstRoom.width - 1, 1, y);
                    break;
                }
            }
        }

        if (exitTile != null)
        {
            exitTile.SetAsExitDoorTile();
            Debug.Log("Exit door placed at: " + exitPosition);
        }
        else
        {
            Debug.LogError("No suitable exit tile found in the first room.");
        }
    }

    List<RectInt> GetAllRooms(BSPNode node)
    {
        List<RectInt> rooms = new List<RectInt>();
        if (node.Room.width != 0 && node.Room.height != 0)
        {
            rooms.Add(node.Room);
        }
        if (node.Left != null)
        {
            rooms.AddRange(GetAllRooms(node.Left));
        }
        if (node.Right != null)
        {
            rooms.AddRange(GetAllRooms(node.Right));
        }
        return rooms;
    }
}