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
            start.Visited = true;

            if (start.Equals(end))
            {
                return true;
            }

            for (int i = 0; i < start.Neighbors.Count; i++)
            {
                Spot neighbor = start.Neighbors[i];
                if (!neighbor.Visited && neighbor.IsWalkable)
                {
                    if (SearchPath(start.Neighbors[i], end))
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