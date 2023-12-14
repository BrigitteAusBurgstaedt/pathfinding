using System.Text;
using UnityEngine;


namespace pathfinding
{
    /// <summary>
    /// Ein Graph enthält die Knoten (Spots)
    /// Basis ist ein zweidimensionales Spot Array welches die Karte mit allen möglichen Pfaden widerspiegelt.
    /// </summary>
    public class Graph
    {
        private readonly int _xOffset;   // Gibt die Verschiebung zw. x-Koordinate und dem Index der 0. Dimension
        private readonly int _yOffset;   // Gibt die Verschiebung zw. y-Koordinate und dem Index der 1. Dimension
        // notwendig da Indizes nicht negativ werden können, Koordinaten hingegen schon.
        public Spot[,] spots;

        public Graph(int lengthX, int lengthY, Spot rootSpot)
        {
            spots = new Spot[lengthX, lengthY];
            _xOffset = rootSpot.X;
            _yOffset = rootSpot.Y;
            AddSpot(rootSpot);
        }

        /// <summary>
        /// Einen Spot zu dem Graphen hinzufügen. Es wird geprüft ob der Spot in den Graphen passt, jedoch nicht ob schon ein Spot an der Stelle vorhanden ist.
        /// </summary>
        /// <param name="spot"></param>
        public void AddSpot(Spot spot)
        {
            if (IsInGraph(spot))
                spots[spot.X - _xOffset, spot.Y - _yOffset] = spot;
            else
                Debug.Log("Spot out of Range of the Graph." +
                    " Index: " + (spot.X-_xOffset) + ", " + (spot.Y-_yOffset) + 
                    " Bounds: " + spots.GetUpperBound(0) + ", " + spots.GetUpperBound(1));
        }

        public Spot GetSpot(int x, int y)
        {
            if (IsInGraph(x,y))
                return spots[x - _xOffset, y - _yOffset];

            Debug.Log("Spot ausserhalb des Graphen");
            return null;
        }

        private bool IsInGraph(Spot spot)
        {
            return IsInGraph(spot.X, spot.Y);
        }

        private bool IsInGraph(int x, int y)
        {
            return (x - _xOffset >= 0 
                && x - _xOffset < spots.GetLength(0) 
                && y - _yOffset >= 0 
                && y - _yOffset < spots.GetLength(1));
        }

        public void AddNeighborsForAllSpots()
        {
            for (int i = 0; i < spots.GetLength(0); i++)
            {
                for (int j = 0; j < spots.GetLength(1); j++)
                {
                    AddNeighbors(spots[i, j]);
                }
            }
        }

        /// <summary>
        /// Für einen Spot alle Nachbarn hinzufügen.
        /// </summary>
        /// <param name="spot">Spot dessen Nachbarn hinzugefügt werden sollen</param>
        private void AddNeighbors(Spot spot)
        {
            int indexForX = spot.X - _xOffset;
            int indexForY = spot.Y - _yOffset;
            bool isOdd = (indexForY % 2 == 1);

            if (indexForX < spots.GetUpperBound(0))                     // UpperBound = Length - 1
                spot.Neighbors.Add(spots[indexForX + 1, indexForY]);    // rechts 
            if (indexForX > 0)
                spot.Neighbors.Add(spots[indexForX - 1, indexForY]);    // links 
            if (indexForY < spots.GetUpperBound(1))
                spot.Neighbors.Add(spots[indexForX, indexForY + 1]);    // oben links (für ungerade); oben rechts (sonst)
            if (indexForY > 0)
                spot.Neighbors.Add(spots[indexForX, indexForY - 1]);    // unten links (für ungerade); unten rechts (sonst)

            if (isOdd)
            {
                if (indexForY < spots.GetUpperBound(1) && indexForX > 0)
                    spot.Neighbors.Add(spots[indexForX - 1, indexForY + 1]); // oben links 
                if (indexForY > 0 && indexForX > 0)
                    spot.Neighbors.Add(spots[indexForX - 1, indexForY - 1]); // unten links 
            }
            else
            {
                if (indexForY < spots.GetUpperBound(1) && indexForX < spots.GetUpperBound(0))
                    spot.Neighbors.Add(spots[indexForX + 1, indexForY + 1]); // oben rechts 
                if (indexForY > 0 && indexForX < spots.GetUpperBound(0))
                    spot.Neighbors.Add(spots[indexForX + 1, indexForY - 1]); // unten rechts 
            }

            // spot.PrintNeighbors(); Debugging
        }

        /// <summary>
        /// Debug Methode die alle Knoten im Graphen ausgibt
        /// </summary>
        public void PrintSpots()
        {
            StringBuilder sb = new StringBuilder("Spots:\n");

            for (int i = 0; i < spots.GetLength(0); i++)
            {
                for (int j = 0; j < spots.GetLength(1); j++)
                {
                    sb.Append("\t(i: " + i + ", j: " + j + ", x: " + spots[i, j].X + ", y: " + spots[i, j].Y + ")");
                }
                sb.Append('\n');
            }

            Debug.Log(sb.ToString());
        }
    }

}