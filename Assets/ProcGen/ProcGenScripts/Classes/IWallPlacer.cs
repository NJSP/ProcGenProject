using Assets.ProcGen.ProcGenScripts;

public interface IWallPlacer
{
    void PlaceWalls(PG_Tile[,] tiles, int x, int y, int width, int height);
  
}
