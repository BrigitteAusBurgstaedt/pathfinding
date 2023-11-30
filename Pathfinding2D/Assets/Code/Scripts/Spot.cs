using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Spot
{
    public int X; // x-Koordinate
    public int Y; // y-Koordinate
    public int F;
    public int G;
    public int H;
    public bool IsWalkable = false; // z-Koordinate
    public List<Spot> Neighboors;
    public Spot previous = null;

    public Spot(int x, int y, bool isWalkable)
    {
        X = x;
        Y = y;
        F = 0;
        G = 0;
        H = 0;
        Neighboors = new List<Spot>();
        IsWalkable = isWalkable;
    }
    public void AddNeighboors(Spot[,] grid, int x, int y)
    {
        bool isOdd = y % 2 == 1;

        if (x < grid.GetUpperBound(0))
            Neighboors.Add(grid[x + 1, y]); // rechts 
        if (x > 0)
            Neighboors.Add(grid[x - 1, y]); // links 
        if (y < grid.GetUpperBound(1))
            Neighboors.Add(grid[x, y + 1]); // oben links (für ungerade); oben rechts (sonst)
        if (y > 0)
            Neighboors.Add(grid[x, y - 1]); // unten links (für ungerade); unten rechts (sonst)

        if (isOdd)
        {
            if (y < grid.GetUpperBound(1) && x > 0)
                Neighboors.Add(grid[x - 1, y + 1]); // oben links 
            if (y > 0 && x > 0)
                Neighboors.Add(grid[x - 1, y - 1]); // unten links 
        }
        else
        {
            if (y < grid.GetUpperBound(1) && x < grid.GetUpperBound(0))
                Neighboors.Add(grid[x + 1, y + 1]); // oben rechts 
            if (y > 0 && x < grid.GetUpperBound(0))
                Neighboors.Add(grid[x + 1, y - 1]); // unten rechts 
        }
    }

    /// <summary>
    /// Diese Methode wandelt die Tilemap in Spots um für die Suchalgorithmen.
    /// </summary>
    /// <param name="tilemap"></param>
    /// <returns> Ein zweidimensionales Array mit aus Knotenpunkten. </returns>
    public static Spot[,] TilemapToSpots(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;
        Spot[,] spots = new Spot[tilemap.cellBounds.size.x, tilemap.cellBounds.size.y];

        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                spots[i, j] = new Spot(x, y, tilemap.HasTile(new Vector3Int(x, y, 0)));
            }
        }

        return spots;
    }


}
