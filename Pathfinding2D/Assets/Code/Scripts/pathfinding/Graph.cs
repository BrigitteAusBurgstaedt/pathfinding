using System.Text;
using UnityEngine;


namespace pathfinding
{
    /// <summary>
    /// Ein Graph enth�lt die Knoten (Spots)
    /// Basis ist ein zweidimensionales Spot Array welches die Karte mit allen m�glichen Pfaden widerspiegelt.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Gibt die Verschiebung zw. x-Koordinate und dem Index der 0. Dimension, notwendig da Indizes nicht negativ werden k�nnen, Koordinaten hingegen schon
        /// </summary>
        private readonly int _xOffset;

        /// <summary>
        /// Gibt die Verschiebung zw. y-Koordinate und dem Index der 1. Dimension, notwendig da Indizes nicht negativ werden k�nnen, Koordinaten hingegen schon
        /// </summary>
        private readonly int _yOffset;

        /// <summary>
        /// Grundstruktur des Graphen
        /// </summary>
        public Spot[,] spots {  get; private set; }

        /// <summary>
        /// Erstellt den Graphen und f�gt den Wurzelknoten ein.
        /// </summary>
        /// <param name="lengthX">Breite der Karte bzw. des Graphen</param>
        /// <param name="lengthY">H�he der Karte bzw. des Graphen</param>
        /// <param name="rootSpot">Wurzel Knoten der an der Stelle [0,0] eingef�gt wird und f�r die Bestimmung des Offsets verwendet wird</param>
        public Graph(int lengthX, int lengthY, Spot rootSpot)
        {
            spots = new Spot[lengthX, lengthY];
            _xOffset = rootSpot.X;
            _yOffset = rootSpot.Y;
            AddSpot(rootSpot);
        }

        /// <summary>
        /// Einen Knoten zu dem Graphen hinzuf�gen. Es wird gepr�ft ob der Spot in den Graphen passt, jedoch nicht ob schon ein Spot an der Stelle vorhanden ist.
        /// </summary>
        /// <param name="spot">Knoten welcher hinzugef�gt werden soll</param>
        public void AddSpot(Spot spot)
        {
            if (IsInGraph(spot))
                spots[spot.X - _xOffset, spot.Y - _yOffset] = spot;
            else
                Debug.Log("Spot out of Range of the Graph." +
                    " Index: " + (spot.X-_xOffset) + ", " + (spot.Y-_yOffset) + 
                    " Bounds: " + spots.GetUpperBound(0) + ", " + spots.GetUpperBound(1));
        }

        /// <summary>
        /// Einen Knoten aus dem Graphen auslesen. Es wird gepr�ft ob die Stelle im Graphen vorhanden ist.
        /// </summary>
        /// <param name="x">x-Koordinate</param>
        /// <param name="y">y-Koordinate</param>
        /// <returns>Den Knoten, oder null falls nicht vorhanden.</returns>
        public Spot GetSpot(int x, int y)
        {
            if (IsInGraph(x,y))
                return spots[x - _xOffset, y - _yOffset];

            Debug.Log("Spot ausserhalb des Graphen");
            return null;
        }

        /// <summary>
        /// Pr�ft ob ein Knoten innerhalb Graphen liegt (f�r AddSpot())
        /// </summary>
        /// <param name="spot">Knoten der gepr�ft werden soll</param>
        /// <returns>true wenn der Knoten sich im Graphen befindet, false sonst</returns>
        private bool IsInGraph(Spot spot)
        {
            return IsInGraph(spot.X, spot.Y);
        }

        /// <summary>
        /// Pr�ft ob ein Punkt innerhalb Graphen liegt (f�r GetSpot())
        /// </summary>
        /// <param name="x">x-Koordinate des Punkts</param>
        /// <param name="y">y-Koordinate des Punkts</param>
        /// <returns>true wenn der Punkt sich im Graphen befindet, false sonst</returns>
        private bool IsInGraph(int x, int y)
        {
            return (x - _xOffset >= 0 
                && x - _xOffset < spots.GetLength(0) 
                && y - _yOffset >= 0 
                && y - _yOffset < spots.GetLength(1));
        }

        /// <summary>
        /// F�r alle Knoten alle Nachbarn hinzuf�gen. Es sollten erst alle Knoten vorhanden sein.
        /// </summary>
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
        /// F�r einen Spot alle Nachbarn hinzuf�gen. Der erste Nachbarknoten ist rechts vom Ausgangsknoten. Die folgenden Nachbarn werden mit dem Uhrzeigersinn hinzugef�gt:
        ///       5   6
        ///     4   K   1
        ///       3   2
        /// </summary>
        /// <param name="spot">Spot dessen Nachbarn hinzugef�gt werden sollen</param>
        private void AddNeighbors(Spot spot)
        {
            int indexForX = spot.X - _xOffset;
            int indexForY = spot.Y - _yOffset;
            bool isOdd = (indexForY % 2 == 1);

            if (indexForX < spots.GetUpperBound(0))                         // UpperBound == Length - 1
                spot.Neighbors.Add(spots[indexForX + 1, indexForY]);        // 1. rechts 
            if (indexForY > 0 && isOdd)
                spot.Neighbors.Add(spots[indexForX, indexForY - 1]);        // 2. unten rechts (f�r ungerade)
            else if (indexForY > 0 && indexForX < spots.GetUpperBound(0))
                spot.Neighbors.Add(spots[indexForX + 1, indexForY - 1]);    // 2. unten rechts (f�r gerade)
            if (indexForY > 0 && !isOdd)
                spot.Neighbors.Add(spots[indexForX, indexForY - 1]);        // 3. unten links (f�r gerade)
            else if (indexForY > 0 && indexForX > 0)
                spot.Neighbors.Add(spots[indexForX - 1, indexForY - 1]);    // 3. unten links (f�r ungerade)
            if (indexForX > 0)
                spot.Neighbors.Add(spots[indexForX - 1, indexForY]);        // 4. links 
            if (indexForY < spots.GetUpperBound(1) && !isOdd)
                spot.Neighbors.Add(spots[indexForX, indexForY + 1]);        // 5. oben links (f�r gerade)
            else if (indexForY < spots.GetUpperBound(1) && indexForX > 0)
                spot.Neighbors.Add(spots[indexForX - 1, indexForY + 1]);    // 5. oben links (f�r ungerade)
            if (indexForY < spots.GetUpperBound(1) && isOdd)
                spot.Neighbors.Add(spots[indexForX, indexForY + 1]);        // 6. oben rechts (f�r ungerade)
            else if (indexForY < spots.GetUpperBound(1) && indexForX < spots.GetUpperBound(0))
                spot.Neighbors.Add(spots[indexForX + 1, indexForY + 1]);    // 6. oben rechts (f�r gerade)

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