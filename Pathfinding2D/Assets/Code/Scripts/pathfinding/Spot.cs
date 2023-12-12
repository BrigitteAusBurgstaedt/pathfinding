using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEditorInternal;
using System;
using System.Text;

namespace pathfinding
{
    /// <summary>
    /// Ein Spot entspricht einem Knoten im Graphen.
    /// </summary>
    public class Spot
    {
        public int X { get; set; } // x-Koordinate im HexGrid
        public int Y { get; set; } // x-Koordinate im HexGrid
        public int F { get => _f; } // Gesamt Kosten
        public int G 
        {
            get => _g;
            set 
            {
                _f = _h + value;
                _g = value;
            } 
        } // Kosten bis zum Knoten, beim Setzen wird F automatisch berechnet
        public int H 
        { 
            get => _h;
            set 
            {
                _f = _h + value;
                _h = value;
            }
        } // Geschätzte Kosten bis zum Ziel, beim Setzen wird F automatisch berechnet
        public bool IsWalkable { get; set; } = false;
        public List<Spot> Neighbors { get; set; }
        public Spot Previous { get; set; } = null;
        private int _f;
        private int _g;
        private int _h;

        public Spot(int x, int y, bool isWalkable)
        {
            X = x;
            Y = y;
            _f = 0;
            _g = 0;
            _h = 0;
            Neighbors = new List<Spot>();
            IsWalkable = isWalkable;
        }

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
