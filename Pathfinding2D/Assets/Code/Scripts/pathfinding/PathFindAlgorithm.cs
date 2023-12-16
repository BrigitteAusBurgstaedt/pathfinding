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
        public Graph Graph { set; get; }

        /// <summary>
        /// Liste die für jede Iteration des Algorithmus die bearbeiteten Knoten enthält. Wird genutzt um den Algorithmus schrittweise darzustellen.
        /// </summary>
        public List<List<Spot>> Iterations { private set; get; } = new();

        /// <summary>
        /// Lädt den Graphen.
        /// </summary>
        /// <param name="tilemap">Die Karte, welche die Pfad Teile für die Knotenpunkte enthält</param>
        protected PathFindAlgorithm(Tilemap tilemap)
        {
            LoadGraph(tilemap);
        }

        /// <summary>
        /// Graph neu laden. Ableiten der Knotenpunkte aus der Tilemap.
        /// </summary>
        /// <param name="tilemap">Die Karte, welche die Pfad Teile für die Knotenpunkte enthält</param>
        public void LoadGraph (Tilemap tilemap)
        {
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;
            Spot rootSpot = new Spot(bounds.xMin, bounds.yMin, tilemap.HasTile(new Vector3Int(bounds.xMin, bounds.yMin, 0)));   // TODO: PathTile verwenden

            Graph = new Graph(bounds.size.x + 1, bounds.size.y + 1, rootSpot);  // Size + 1 da Length gebraucht wird

            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
                {
                    if (x == bounds.xMin && y == bounds.yMin) continue; // Das Erste Teil wurde schon als Wurzel eingefügt
                    Graph.AddSpot(new Spot(x, y, tilemap.HasTile(new Vector3Int(x, y, 0))));
                }
            }

            Graph.AddNeighborsForAllSpots();
        }

        /// <summary>
        /// Überprüfung ob die Ausgangsknoten erreichbar sind. Achtung: Garantiert nicht, dass es einen Pfad gibt.
        /// </summary>
        /// <param name="start">Startknoten des Pfads</param>
        /// <param name="end">Zielknoten des Pfads</param>
        /// <returns></returns>
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

        /// <summary>
        /// Erzeugt dem Path über den jeweiligen Vorgänger und gibt diesen als Liste zurück. 
        /// </summary>
        /// <param name="start">Startpunkt als Koordinaten</param>
        /// <param name="end">Zielpunkt als Koordinaten</param>
        /// <returns>Gefundenen Pfad oder leere Liste, wenn keiner gefunden wurde.</returns>
        public List<Spot> CreatePath(Vector2Int start, Vector2Int end)
        {
            return CreatePath(Graph.GetSpot(start.x, start.y), Graph.GetSpot(end.x, end.y));
        }

        /// <summary>
        /// Erzeugt dem Path über den jeweiligen Vorgänger und gibt diesen als Liste zurück. 
        /// </summary>
        /// <param name="start">Startknoten</param>
        /// <param name="end">Zielknoten</param>
        /// <returns>Gefundenen Pfad oder leere Liste, wenn keiner gefunden wurde.</returns>
        public List<Spot> CreatePath(Spot start, Spot end) 
        {
            if (!IsValidPath(start, end))
            {
                Debug.Log("Kein valider Pfad!");
                return new List<Spot>();
            }

            if (!SearchPath(start, end))
            {
                Debug.Log("Kein Pfad gefunden!");
                return new List<Spot>();
            }

            List<Spot> Path = new List<Spot>();
            var temp = end;
            Path.Add(temp);
            while (temp.Previous != null) // TODO: Evtl. endlos Loop verhindern 
            {
                Path.Add(temp.Previous);
                temp = temp.Previous;
            }
            return Path;
        }

        /// <summary>
        /// Der jeweilige Suchalgorithmus.
        /// </summary>
        /// <param name="start">Startknoten</param>
        /// <param name="end">Zielknoten</param>
        /// <returns>true wenn ein Pfad gefunden wurde, false wenn kein Pfad Gefunden wurde</returns>
        protected abstract bool SearchPath(Spot start, Spot end);
    }
}