using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    /// <summary>
    /// Abstrakte Klasse für alle Pfadsuchalgorithmen im Projekt.
    /// </summary>
    public abstract class PathFindAlgorithm
    {
        /// <summary>
        /// Abgeleitete Knotenpunkte des Graphen.
        /// </summary>
        public Graph graph;

        protected PathFindAlgorithm(Tilemap tilemap)
        {
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;
            Spot rootSpot = new Spot(bounds.xMin, bounds.yMin, tilemap.HasTile(new Vector3Int(bounds.xMin, bounds.yMin, 0)));

            graph = new Graph(bounds.size.x + 1, bounds.size.y + 1, rootSpot); // Size + 1 da Length gebraucht wird

            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin;  y <= bounds.yMax; y++)
                {
                    if (x == bounds.xMin && y == bounds.yMin) continue;
                    graph.AddSpot(new Spot(x, y, tilemap.HasTile(new Vector3Int(x, y, 0))));
                }
            }

            graph.AddNeighborsForAllSpots();
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

        public abstract List<Spot> CreatePath(Spot start, Spot end, int maxLength);
    }
}