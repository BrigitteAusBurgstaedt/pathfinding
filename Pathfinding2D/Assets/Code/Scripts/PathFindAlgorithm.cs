using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    /// <summary>
    /// Abstrakte Klasse für alle Pfadsuchalgorithmen im Projekt.
    /// </summary>
    public abstract class PathFindAlgorithm
    {
        /// <summary>
        /// Abgeleitete Knotenpunkte des Graphen.
        /// </summary>
        public Spot[,] Spots { get; set; }

        protected PathFindAlgorithm(Tilemap tilemap)
        {
            TilemapToSpots(tilemap);
        }

        /// <summary>
        /// Diese Methode wandelt die Tilemap in Spots um für die Suchalgorithmen.
        /// </summary>
        /// <param name="tilemap"></param>
        /// <returns> Ein zweidimensionales Array mit aus Knotenpunkten. </returns>
        public void TilemapToSpots(Tilemap tilemap)
        {
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;
            Spots = new Spot[tilemap.cellBounds.size.x, tilemap.cellBounds.size.y];

            // Die Bounds können negative Werte annehmen und sind daher nicht als Indizes geeignet
            for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
            {
                for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
                {
                    Spots[i, j] = new Spot(x, y, tilemap.HasTile(new Vector3Int(x, y, 0)));
                }
            }

            for (int i = 0; i < Spots.GetLength(0); i++)
            {
                for (int j = 0; j < Spots.GetLength(1); j++)
                {
                    Spots[i,j].AddNeighbors(Spots, i, j);
                }
            }

        }

        public Spot GetSpot(Camera camera, Tilemap tilemap)
        {
            int xOffset = Spots[0, 0].X;
            int yOffset = Spots[0, 0].Y;

            Vector3Int gridPos = tilemap.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));

            return Spots[gridPos.x - xOffset, gridPos.y - yOffset];
        }

        protected bool IsValidPath(Spot start, Spot end)
        {
            if (end == null)
                return false;
            if (start == null)
                return false;
            if (!end.IsWalkable)
                return false;
            return true;
        }

        public abstract List<Spot> CreatePath(Spot start, Spot end, int maxLength);
    }
}