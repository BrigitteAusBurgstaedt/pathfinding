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

            List<Spot> iteration = new() {start};   // nur für die Visualisierung; 0. Iteration
            Iterations.Add(iteration);  // nur für die Visualisierung

            if (start.Equals(end)) // Schon da
                return true;

            while (waitList.Any()) 
            {
                iteration = new();   // nur für die Visualisierung

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

                        iteration.Add(s);   // nur für die Visualisierung

                        if (s.Equals(end))
                        {
                            Iterations.Add(iteration);  // nur für die Visualisierung; letzte Iteration
                            return true;
                        } 
                    }
                }

                Iterations.Add(iteration);  // nur für die Visualisierung
            }

            return false;
        }
    }
}