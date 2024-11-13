using UnityEngine;

public class EnemyPlacer : IEnemyPlacer
{
    private GameObject[] enemyPrefabs;
    private float enemyPlacementChance;
    private Transform[] enemySpawnPoints;

    public EnemyPlacer(GameObject[] enemyPrefabs, float enemyPlacementChance, Transform[] enemySpawnPoints)
    {
        this.enemyPrefabs = enemyPrefabs;
        this.enemyPlacementChance = enemyPlacementChance;
        this.enemySpawnPoints = enemySpawnPoints;
    }

    public void PlaceEnemies()
    {
        foreach (var spawnPoint in enemySpawnPoints)
        {
            if (Random.value <= enemyPlacementChance)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject.Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}