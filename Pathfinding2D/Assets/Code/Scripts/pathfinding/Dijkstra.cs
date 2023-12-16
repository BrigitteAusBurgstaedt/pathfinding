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
            List<Spot> waitList = new List<Spot>(); // Enthält die Liste der Knoten die Noch Bearbeitet werden müssen
            for (int i = 0; i < graph.spots.GetLength(0); i++)
            {
                for (int j = 0; j < graph.spots.GetLength(1); j++)
                {
                    waitList.Add(graph.spots[i, j]);
                }
            }
            start.Visited = true;
            Initialize(start);

            while (waitList.Any())
            {
                int indexOfNearest = GetIndexOfNearest(waitList);
                Spot nearest = waitList[indexOfNearest];
                waitList.RemoveAt(indexOfNearest);

                if (nearest.Equals(end))
                {
                    return true;
                }

                foreach (Spot s in nearest.Neighbors)
                {

                    if (waitList.Contains(s)) // Vgl Visited
                    {
                        if (s.IsWalkable)
                        {
                            int tmp = nearest.Distance + s.Cost;
                            if (tmp < s.Distance)
                            {
                                s.Distance = tmp;
                                s.Previous = nearest;
                            }

                        }
                    }
                }
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