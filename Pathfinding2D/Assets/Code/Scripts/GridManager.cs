using pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;
    PathFindAlgorithm pathFindAlgorithm;
    List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    public Vector2Int start;

    // Start is called before the first frame update
    void Start()
    {
        roadMap.CompressBounds();
        camera = Camera.main;

        pathFindAlgorithm = new AStar(tilemap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) // Rechte Maustaste betätigt -> Start an Position setzten
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            start = new Vector2Int(gridPos.x, gridPos.y);
        }
        if (Input.GetMouseButton(2)) // Mittlere Maustaste betätigt -> Road an Position löschen
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }
        if (Input.GetMouseButton(0)) // Linke Maustaste betätigt -> Pfad zur Position Zeichnen
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);

            if (roadPath != null && roadPath.Count > 0) // Löschen der alten Pfadliste
                roadPath.Clear();

            pathFindAlgorithm.LoadGraph(tilemap); // TODO Übergangslösung verbessern (löst das Memory Problem)
            roadPath = pathFindAlgorithm.CreatePath(start , new Vector2Int(gridPos.x, gridPos.y));
            if (!roadPath.Any())
                return;

            DrawRoad();
            start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
        }
    }

    private void DrawRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
        }
    }
}
