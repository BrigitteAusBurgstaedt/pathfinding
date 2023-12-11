using pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Utils
{
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

        for (int i = 0; i < (bounds.size.x);i++)
        {
            for (int j = 0; j < (bounds.size.y);j++)
            {
                spots[i, j].AddNeighbors(spots, i, j);
            }
        }

        return spots;
    }

    public static Spot GetSpot(Spot[,] spots, Camera camera, Tilemap tilemap)
    {
        int xOffset = spots[0, 0].X;
        int yOffset = spots[0, 0].Y;

        Vector3Int gridPos = tilemap.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

        return spots[gridPos.x - xOffset, gridPos.y - yOffset];
    }

    public static Spot GetSpot(Spot[,] spots, Vector2Int gridPos)
    {
        int xOffset = spots[0, 0].X;
        int yOffset = spots[0, 0].Y;

        if(gridPos.x - xOffset >= 0 && gridPos.x - xOffset < spots.GetLength(0) && gridPos.y - yOffset >= 0 && gridPos.y - yOffset < spots.GetLength(1))
            return spots[gridPos.x - xOffset, gridPos.y - yOffset];

        return null;
    }
}
