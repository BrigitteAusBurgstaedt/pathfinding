using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace pathfinding
{
    /// <summary>
    /// Ein Spot entspricht einem Knoten im Graphen.
    /// </summary>
    public class Spot
    {
        /// <summary>
        /// x-Koordinate im HexGrid
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// y-Koordinate im HexGrid
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gesamt Kosten, wird automatisch beim Setzen von G berechnet
        /// </summary>
        public int F { get => _f; }

        /// <summary>
        /// Kosten bis zum Knoten, beim Setzen wird F automatisch berechnet
        /// </summary>
        public int G 
        {
            get => _g;
            set 
            {
                _f = _h + value;
                _g = value;
            } 
        }

        /// <summary>
        /// Gesch�tzte Kosten bis zum Ziel, beim Setzen wird F automatisch berechnet
        /// </summary>
        public int H 
        { 
            get => _h;
            set 
            {
                _f = _g + value;
                _h = value;
            }
        }

        /// <summary>
        /// Gibt den Besuchtheitsstatus des Knoten an. 0 == noch nicht besucht, jede andere Zahl gibt an, bei welcher Iteration der Knoten besucht wurde.
        /// </summary>
        public int Visited { get; set; } = 0;

        public int Distance { get; set; } = int.MaxValue;

        public int Cost { get; private set; }

        /// <summary>
        /// Gibt an ob der Knoten begehbar ist
        /// </summary>
        public bool IsWalkable { get; set; } = false;
        public List<Spot> Neighbors { get; set; }
        public Spot Previous { get; set; } = null;
        private int _f;
        private int _g;
        private int _h;

        public Spot(int x, int y, bool isWalkable, int cost)
        {
            X = x;
            Y = y;
            _f = 0;
            _g = cost;
            _h = 0;
            Neighbors = new List<Spot>();
            IsWalkable = isWalkable;
            Cost = cost;
        }

        /// <summary>
        /// Debug Methode die alle Nachbarn eines Spots ausgibt.
        /// </summary>
        public void PrintNeighbors()
        {
            StringBuilder sb = new StringBuilder("Spot(" + this.X + ", " + this.Y + ") \nNachbarn:\n");
           
            foreach (var neighbor in Neighbors)
            {
                sb.Append( "\t(" + neighbor.X + ", " + neighbor.Y + ")");
            }

            Debug.Log(sb.ToString());
        }
    }
}
