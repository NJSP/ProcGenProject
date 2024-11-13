using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Fields for prefabs and configuration values
    public GameObject[] wallPrefabs;
    public GameObject[] propPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] corridorTrapPrefabs;
    public GameObject[] trapRoomTrapPrefabs;
    public float propPlacementChance = 0.2f;
    public float enemyPlacementChance = 0.1f;
    public float trapPlacementChance = 0.1f;

    void Start()
    {
        // Initialize the dependencies with the provided values
        IWallPlacer wallPlacer = new WallPlacer(wallPrefabs);
        IPropPlacer propPlacer = new PropPlacer(propPrefabs, propPlacementChance);
        IEnemyPlacer enemyPlacer = new EnemyPlacer(enemyPrefabs, enemyPlacementChance);
        ITrapPlacer trapPlacer = new TrapPlacer(corridorTrapPrefabs, trapRoomTrapPrefabs, trapPlacementChance);

        IDungeonLayoutGenerator layoutGenerator = new DungeonLayoutGenerator();
        ITilePlacer tilePlacer = new TilePlacer(wallPlacer, propPlacer, enemyPlacer, trapPlacer);

        DungeonGenerator dungeonGenerator = new DungeonGenerator(layoutGenerator, tilePlacer);
        dungeonGenerator.GenerateDungeon(10, 10);
    }
}