using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace pathfinding
{
    /// <summary>
    /// Ein Graph enthält die Knoten (Spots)
    /// Basis ist ein zweidimensionales Spot Array welches die Karte mit allen möglichen Pfaden widerspiegelt.
    /// </summary>
    public class Graph
    {
        private int _xOffset;   // Gibt die Verschiebung zw. x-Koordinate und dem Index der 0. Dimension
        private int _yOffset;   // Gibt die Verschiebung zw. y-Koordinate und dem Index der 1. Dimension
        // notwendig da Indizes nicht negativ werden können, Koordinaten hingegen schon.
        private Spot[,] spots;

        public Graph(int sizeX, int sizeY, Spot rootSpot)
        {
            spots = new Spot[sizeX, sizeY];
            _xOffset = rootSpot.X;
            _yOffset = rootSpot.Y;
            AddSpot(rootSpot);
        }

        /// <summary>
        /// Einen Spot zu dem Graphen hinzufügen.
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
        /// Für einen Spot alle Nachbarn hinzufügen
        /// </summary>
        /// <param name="spot">Spot dessen Nachbarn hinzugefügt werden sollen</param>
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
                spot.Neighbors.Add(spots[indexForX, indexForY + 1]); // oben links (für ungerade); oben rechts (sonst)
            if (indexForY > 0)
                spot.Neighbors.Add(spots[indexForX, indexForY - 1]); // unten links (für ungerade); unten rechts (sonst)

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

        // TODO Spot Array mit Graph ersetzen, Tilemap to spots auflösen (utils), get spot ersetzen (utils), AddNeighbors aus Spot löschen
    }

}