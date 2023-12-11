using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace pathfinding
{
    /// <summary>
    /// Ein Graph enth�lt die Knoten (Spots)
    /// Basis ist ein zweidimensionales Spot Array welches die Karte mit allen m�glichen Pfaden widerspiegelt.
    /// </summary>
    public class Graph
    {
        private int _xOffset;   // Gibt die Verschiebung zw. x-Koordinate und dem Index der 0. Dimension
        private int _yOffset;   // Gibt die Verschiebung zw. y-Koordinate und dem Index der 1. Dimension
        // notwendig da Indizes nicht negativ werden k�nnen, Koordinaten hingegen schon.
        private Spot[,] spots;

        public Graph(int sizeX, int sizeY, Spot rootSpot)
        {
            spots = new Spot[sizeX, sizeY];
            _xOffset = rootSpot.X;
            _yOffset = rootSpot.Y;
            AddSpot(rootSpot);
        }

        /// <summary>
        /// Einen Spot zu dem Graphen hinzuf�gen.
        /// </summary>
        /// <param name="spot"></param>
        public void AddSpot(Spot spot)
        {
            if (spot.X - _xOffset >= 0 && spot.X - _xOffset <= spots.GetUpperBound(0) && spot.Y - _yOffset >= 0 && spot.Y - _xOffset <= spots.GetUpperBound(1))
                spots[spot.X - _xOffset, spot.Y - _yOffset] = spot;
            else
                Debug.Log("Spot out of Range of the Graph");
        }

        public void AddNeighborsForAllSpots()
        {

            for (int i = 0; i <= spots.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= spots.GetUpperBound(1); j++)
                {
                    AddNeighbors(spots[i, j]);
                }
            }
        }

        /// <summary>
        /// F�r einen Spot alle Nachbarn hinzuf�gen
        /// </summary>
        /// <param name="spot">Spot dessen Nachbarn hinzugef�gt werden sollen</param>
        private void AddNeighbors(Spot spot)
        {
            bool isOdd = (spot.Y % 2 == 1);
            int indexForX = spot.X - _xOffset;
            int indexForY = spot.Y - _yOffset;

            if (indexForX < spots.GetUpperBound(0))
                spot.Neighbors.Add(spots[indexForX + 1, indexForY]); // rechts 
            if (indexForX > 0)
                spot.Neighbors.Add(spots[indexForX - 1, indexForY]); // links 
            if (indexForY < spots.GetUpperBound(1))
                spot.Neighbors.Add(spots[indexForX, indexForY + 1]); // oben links (f�r ungerade); oben rechts (sonst)
            if (indexForY > 0)
                spot.Neighbors.Add(spots[indexForX, indexForY - 1]); // unten links (f�r ungerade); unten rechts (sonst)

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
        }

        // TODO Spot Array mit Graph ersetzen, Tilemap to spots aufl�sen (utils), get spot ersetzen (utils), AddNeighbors aus Spot l�schen
    }

}