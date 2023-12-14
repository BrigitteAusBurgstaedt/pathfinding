using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class BreadthFirst : PathFindAlgorithm
    {

        public BreadthFirst(Tilemap tilemap) : base(tilemap) { }

        protected override bool SearchPath(Spot start, Spot end)
        {
            List<Spot> waitList = new List<Spot>(); // Enthält die Liste der Knoten die Noch Bearbeitet werden müssen
            start.Visited = true;
            waitList.Add(start);

            while (waitList.Any()) 
            {
                Spot firstSpotInWaitList = waitList[0];
                waitList.RemoveAt(0);

                if (firstSpotInWaitList.Equals(end))
                {
                    return true;
                }

                foreach (Spot s in firstSpotInWaitList.Neighbors)
                {
                    if (!s.Visited)
                    {
                        s.Visited = true;
                        if (s.IsWalkable)
                        {
                            s.Previous = firstSpotInWaitList;
                            waitList.Add(s);
                        }
                    }
                }
            }

            return false;
        }
    }
}