using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class DepthFirst : PathFindAlgorithm
    {
        // private int currentVisited = 0; für später

        public DepthFirst(Tilemap tilemap) : base(tilemap) { }

        public override Object GetVisualNextStep(Tilemap tilemap, out Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        protected override bool SearchPath(Spot start, Spot end)
        {
            if (start.Visited == 0) // Aller erster Startknoten
            {
                start.Visited = 1;
                Steps.Add(start); // nur für Visuals
            }

            if (start.Equals(end))  // Abbruchbedingung
            {
                return true;
            }

            for (int i = 0; i < start.Neighbors.Count; i++)
            {
                if (start.Neighbors[i].IsWalkable && start.Neighbors[i].Visited == 0)
                {
                    start.Neighbors[i].Visited = start.Visited + 1;
                    start.Neighbors[i].Previous = start;
                    Steps.Add(start.Neighbors[i]); // nur für Visuals

                    if (SearchPath(start.Neighbors[i], end))    // Rekursivschritt
                    {
                        return true;
                    }
                        
                }
            }

            return false;

        }
    }
}