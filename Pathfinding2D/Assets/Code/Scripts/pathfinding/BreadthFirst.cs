using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class BreadthFirst : PathFindAlgorithm
    {

        public BreadthFirst(Tilemap tilemap) : base(tilemap) { }

        protected override bool SearchPath(Spot start, Spot end)
        {
            List<Spot> waitList = new();     // Enthält die Liste der Knoten die Noch Bearbeitet werden müssen
            int visitedNumber = 1;                      // Gibt an wie viele aktuell besucht wurden
            start.Visited = visitedNumber;
            waitList.Add(start);

            Steps.Add(start);  // nur für die Visualisierung

            if (start.Equals(end)) // Schon da
                return true;

            while (waitList.Any()) 
            {
                Spot firstSpotInWaitList = waitList[0];
                waitList.RemoveAt(0);

                foreach (Spot s in firstSpotInWaitList.Neighbors)
                {
                    
                    if (s.IsWalkable && s.Visited == 0)
                    {
                        visitedNumber++;
                        s.Visited = visitedNumber;
                        s.Previous = firstSpotInWaitList;
                        waitList.Add(s);

                        Steps.Add(s);   // nur für die Visualisierung

                        if (s.Equals(end))
                        {
                            return true;
                        } 
                    }
                }
            }

            return false;
        }
    }
}