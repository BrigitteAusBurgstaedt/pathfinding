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
        public Spot[,] Spots { get; set; }

        protected PathFindAlgorithm(Tilemap tilemap)
        {
            Spots = Utils.TilemapToSpots(tilemap);
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