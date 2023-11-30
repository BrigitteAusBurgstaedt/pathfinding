using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Breitensuche : Suchalgorithmus
{

    public Breitensuche(Tilemap tilemap) : base(tilemap){}
    
    public override List<Spot> CreatePath(Vector2Int start, Vector2Int end, int maxLength)
    {
        throw new NotImplementedException();
    }
}