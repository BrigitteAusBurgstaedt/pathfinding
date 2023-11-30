using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Abstrakte Klasse für alle Pfadsuchalgorithmen im Projekt.
/// </summary>
public abstract class Suchalgorithmus
{
    public Spot[,] Spots;

    protected Suchalgorithmus(Spot[,] spots)
    {
        Spots = spots;
    }

    protected Suchalgorithmus(Tilemap tilemap)
    {
        Spots = Spot.TilemapToSpots(tilemap);
    }

    public void UpdateSpots(Tilemap tilemap)
    {
        Spots = Spot.TilemapToSpots(tilemap);
    }

    protected bool IsValidPath(Spot start, Spot end)
    {
        if (end == null)
            return false;
        if (start == null)
            return false;
        if (!end.IsWalkable)
            return false;
        return true;
    }

    public abstract List<Spot> CreatePath(Vector2Int start, Vector2Int end, int maxLength);
}