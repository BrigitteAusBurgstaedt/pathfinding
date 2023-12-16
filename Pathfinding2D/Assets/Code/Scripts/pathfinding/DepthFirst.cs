using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Unity.Collections;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class DepthFirst : PathFindAlgorithm
    {

        public DepthFirst(Tilemap tilemap) : base(tilemap) { }

        protected override bool SearchPath(Spot start, Spot end)
        {
            if (start.Visited == 0) // Aller erster Startknoten
                start.Visited = 1;

            if (start.Equals(end))  // Abbruchbedingung
            {
                return true;
            }

            for (int i = 0; i < start.Neighbors.Count; i++)
            {
                if (start.Neighbors[i].IsWalkable && start.Neighbors[i].Visited == 0)
                {
                    start.Neighbors[i].Visited = start.Visited + 1;
                    if (SearchPath(start.Neighbors[i], end))    // Rekursivschritt
                    {
                        start.Neighbors[i].Previous = start;
                        return true;
                    }
                        
                }
            }

            return false;

        }
    }
}