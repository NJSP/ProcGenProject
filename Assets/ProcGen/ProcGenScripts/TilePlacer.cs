public class TilePlacer : ITilePlacer
{
    private readonly IWallPlacer wallPlacer;
    private readonly IPropPlacer propPlacer;
    private readonly IEnemyPlacer enemyPlacer;
    private readonly ITrapPlacer trapPlacer;

    public TilePlacer(IWallPlacer wallPlacer, IPropPlacer propPlacer, IEnemyPlacer enemyPlacer, ITrapPlacer trapPlacer)
    {
        this.wallPlacer = wallPlacer;
        this.propPlacer = propPlacer;
        this.enemyPlacer = enemyPlacer;
        this.trapPlacer = trapPlacer;
    }

    public void PlaceTiles(PG_Tile[,] layout)
    {
        int rows = layout.GetLength(0);
        int cols = layout.GetLength(1);

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                PG_Tile tile = layout[x, y];

                switch (tile.roomType)
                {
                    case PG_Tile.roomType.Treasure:
                        propPlacer.PlaceProps();
                        break;
                    case PG_Tile.roomType.Lair:
                        enemyPlacer.PlaceEnemies();
                        break;
                    case PG_Tile.roomType.Trap:
                        trapPlacer.PlaceTraps();
                        break;
                    case PG_Tile.roomType.Normal:
                        // Place normal tile logic if any
                        break;
                }

                if (tile.wallCount > 0)
                {
                    wallPlacer.PlaceWalls(layout, x, y, 1, 1);
                }
            }
        }
    }
}