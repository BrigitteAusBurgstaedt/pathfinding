using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace pathfinding
{
    public class Astar : PathFindAlgorithm
    {

        public Astar(Tilemap tilemap) : base(tilemap) { }

        public override List<Spot> CreatePath(Spot start, Spot end, int maxLength)
        {
            if (!IsValidPath(start, end))
            {
                Debug.Log("Kein valider Pfad!");
                return new List<Spot>();
            }
          

            List<Spot> OpenSet = new List<Spot>();
            List<Spot> ClosedSet = new List<Spot>();

            OpenSet.Add(start);

            while (OpenSet.Count > 0)
            {
                //Find shortest step distance in the direction of your goal within the open set
                int winner = 0;
                for (int i = 0; i < OpenSet.Count; i++)
                    if (OpenSet[i].F < OpenSet[winner].F)
                        winner = i;
                    else if (OpenSet[i].F == OpenSet[winner].F && OpenSet[i].H < OpenSet[winner].H) //tie breaking for faster routing
                            winner = i;

                var current = OpenSet[winner];

                //Found the path, creates and returns the path
                if (OpenSet[winner] == end)
                {
                    List<Spot> Path = new List<Spot>();
                    var temp = current;
                    Path.Add(temp);
                    while (temp.Previous != null)
                    {
                        Path.Add(temp.Previous);
                        temp = temp.Previous;
                    }
                    if (maxLength - (Path.Count - 1) < 0)
                    {
                        Path.RemoveRange(0, (Path.Count - 1) - maxLength);
                    }
                    return Path;
                }

                OpenSet.Remove(current);
                ClosedSet.Add(current);


                //Finds the next closest step on the grid
                var neighboors = current.Neighbors;
                for (int i = 0; i < neighboors.Count; i++)//look through our current spots neighboors (current spot is the shortest F distance in openSet
                {
                    var n = neighboors[i];
                    if (!ClosedSet.Contains(n) && n.IsWalkable)//Checks to make sure the neighboor of our current tile is not within closed set, and has a height of less than 1
                    {
                        var tempG = current.G + 1;//gets a temp comparison integer for seeing if a route is shorter than our current path

                        bool newPath = false;
                        if (OpenSet.Contains(n)) //Checks if the neighboor we are checking is within the openset
                        {
                            if (tempG < n.G)//The distance to the end goal from this neighboor is shorter so we need a new path
                            {
                                n.G = tempG;
                                newPath = true;
                            }
                        }
                        else//if its not in openSet or closed set, then it IS a new path and we should add it too openset
                        {
                            n.G = tempG;
                            newPath = true;
                            OpenSet.Add(n);
                        }
                        if (newPath)//if it is a newPath caclulate the H and F and set current to the neighboors previous
                        {
                            n.H = Heuristic(n, end);
                            n.Previous = current;
                        }
                    }
                }

            }

            Debug.Log("Keinen Pfad gefunden!");
            return new List<Spot>();
        }

        private int Heuristic(Spot a, Spot b)
        {
            // manhattan (schlechte heuristik)
            // var dx = Math.Abs(a.X - b.X);
            // var dy = Math.Abs(a.Y - b.Y);
            // return 1 * (dx + dy);

            var dx = Math.Abs(a.X - b.X);
            var dy = Math.Abs(a.Y - b.Y);
            return 1 * Math.Max(dx, dy); // nicht perfekt funktioniert aber
        }
    }
}