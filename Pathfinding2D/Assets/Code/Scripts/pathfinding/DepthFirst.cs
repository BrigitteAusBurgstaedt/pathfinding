using System;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class DepthFirst : PathFindAlgorithm
    {

        public DepthFirst(Tilemap tilemap) : base(tilemap) { }

        protected override bool SearchPath(Spot start, Spot end)
        {
            throw new NotImplementedException();
        }
    }
}