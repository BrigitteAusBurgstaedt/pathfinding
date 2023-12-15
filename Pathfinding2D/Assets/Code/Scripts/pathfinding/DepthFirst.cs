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
                    if (!start.Neighbors[i].Visited)
                    {
                        if (start.Neighbors[i].IsWalkable)
                        {
                            SearchPath(start.Neighbors[i], end);
                        }
                    }
                }

            return false;

        }
    }
}