using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace pathfinding
{
    public class Breitensuche : PathFindAlgorithm
    {

        public Breitensuche(Tilemap tilemap) : base(tilemap) { }

        public override List<Spot> CreatePath(Spot start, Spot end, int maxLength)
        {
            throw new NotImplementedException();
        }
    }
}