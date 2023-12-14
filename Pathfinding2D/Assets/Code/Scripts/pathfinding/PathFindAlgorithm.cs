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
            LoadGraph(tilemap);
        }

        /// <summary>
        /// Graph neu laden.
        /// </summary>
        /// <param name="tilemap"></param>
        public void LoadGraph (Tilemap tilemap)
        {
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;
            Spot rootSpot = new Spot(bounds.xMin, bounds.yMin, tilemap.HasTile(new Vector3Int(bounds.xMin, bounds.yMin, 0)));

            graph = new Graph(bounds.size.x + 1, bounds.size.y + 1, rootSpot); // Size + 1 da Length gebraucht wird

            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
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

        public List<Spot> CreatePath(Vector2Int start, Vector2Int end)
        {
            return CreatePath(graph.GetSpot(start.x, start.y), graph.GetSpot(end.x, end.y));
        }

        /// <summary>
        /// Erzeugt dem Path über den jeweiligen Vorgänger und gibt diesen als Liste zurück. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Spot> CreatePath(Spot start, Spot end) 
        {
            if (!IsValidPath(start, end))
            {
                Debug.Log("Kein valider Pfad!");
                return new List<Spot>();
            }

            if (!SearchPath(start, end))
            {
                return new List<Spot>();
            }

            List<Spot> Path = new List<Spot>();
            var temp = end;
            Path.Add(temp);
            while (temp.Previous != null)
            {
                Path.Add(temp.Previous);
                temp = temp.Previous;
            }
            return Path;

        }

        /// <summary>
        /// Der jeweilige Suchalgorithmus.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected abstract bool SearchPath(Spot start, Spot end);
    }
}