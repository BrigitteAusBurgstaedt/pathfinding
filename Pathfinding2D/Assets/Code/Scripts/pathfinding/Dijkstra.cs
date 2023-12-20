using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class Dijkstra : PathFindAlgorithm
    {
        public Dijkstra(Tilemap tilemap) : base(tilemap) { }

        protected override bool SearchPath(Spot start, Spot end)
        {
            List<Spot> waitList = new List<Spot>(); // Enthält die Liste der Knoten die noch Bearbeitet werden müssen
            for (int i = 0; i < Graph.Spots.GetLength(0); i++)
            {
                for (int j = 0; j < Graph.Spots.GetLength(1); j++)
                {
                    if (Graph.Spots[i, j].IsWalkable)
                        waitList.Add(Graph.Spots[i, j]);
                }
            }
            start.Visited = 1;
            Initialize(start);
            Iterations.Add(waitList); // nur für die Visualisierung

            while (waitList.Any())
            {
                int indexOfNearest = GetIndexOfNearest(waitList);
                Spot nearest = waitList[indexOfNearest];
                waitList.RemoveAt(indexOfNearest);

                if (nearest.Equals(end))
                {
                    return true;
                }

                List<Spot> iteration = new List<Spot>(); // nur für die Visualisierung

                foreach (Spot s in nearest.Neighbors)
                {

                    if (s.IsWalkable && waitList.Contains(s)) // Vgl Visited
                    {
                        int tmp = nearest.Distance + s.Cost; 
                        iteration.Add(s); // nur für die Visualisierung

                        if (tmp < s.Distance)
                        {
                            s.Distance = tmp;
                            s.Previous = nearest;
                        }
                    }
                }

                Iterations.Add(iteration);
            }

            return false;
        }

        private void Initialize(Spot start)
        {
            start.Distance = 0;
        }

        private int GetIndexOfNearest(List<Spot> waitList)
        {
            int indexOfNearest = -1;
            int minDistance = int.MaxValue;
            for (int i = 0; i < waitList.Count; i++)
            {
                if (waitList[i].Distance <= minDistance)
                {
                    minDistance = waitList[i].Distance;
                    indexOfNearest = i;
                }
            }

            return indexOfNearest;
        }
    }
}