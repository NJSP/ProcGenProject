public interface IDungeonLayoutGenerator
{
    PG_Tile[,] GenerateLayout(int width, int height);
}