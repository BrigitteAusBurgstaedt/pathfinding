using System;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class Dijkstra : PathFindAlgorithm
    {
        public Dijkstra(Tilemap tilemap) : base(tilemap) { }

        protected override bool SearchPath(Spot start, Spot end)
        {
            throw new NotImplementedException();
        }
    }
}