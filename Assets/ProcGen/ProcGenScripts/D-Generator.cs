public class DungeonGenerator
{
    private readonly IDungeonLayoutGenerator layoutGenerator;
    private readonly ITilePlacer tilePlacer;

    public DungeonGenerator(IDungeonLayoutGenerator layoutGenerator, ITilePlacer tilePlacer)
    {
        this.layoutGenerator = layoutGenerator;
        this.tilePlacer = tilePlacer;
    }

    public void GenerateDungeon(int width, int height)
    {
        PG_Tile[,] layout = layoutGenerator.GenerateLayout(width, height);
        tilePlacer.PlaceTiles(layout);
    }
}