using pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class PathFindVisuals : MonoBehaviour
    {
        public Coin coin;
        public Tilemap tilemap;
        public Tilemap roadMap;
        public TileBase roadTile;
        public Vector3Int[,] spots;
        PathFindAlgorithm pathFindAlgorithm;
        List<Spot> roadPath = new List<Spot>();
        new Camera camera;
        public Vector2Int start;
        private float time = 0f;
        private bool showIterations = false;

        // Start is called before the first frame update
        void Start()
        {
            roadMap.CompressBounds();
            camera = Camera.main;

            pathFindAlgorithm = new BreadthFirst(tilemap);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1)) // Rechte Maustaste runter gedrückt -> Start an Position setzten
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
            if (Input.GetMouseButtonDown(0)) // Linke Maustaste runter gedrückt -> Pfad zur Position Zeichnen
            {
                Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tilemap.WorldToCell(world);

                if (roadPath != null && roadPath.Count > 0) // Löschen der alten Pfadliste
                    roadPath.Clear();

                pathFindAlgorithm.LoadGraph(tilemap); // TODO Übergangslösung verbessern (löst das Memory Problem)
                roadPath = pathFindAlgorithm.CreatePath(start, new Vector2Int(gridPos.x, gridPos.y));
                if (!roadPath.Any())
                    return;

                showIterations = true;

                DrawRoad();
                start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
            }

            if (showIterations)
            {
                if (pathFindAlgorithm.Iterations.Any())
                {
                    time -= Time.deltaTime;
                    if (time < 0f)
                    {
                        DrawVisitedIteration();
                        time = 0.5f;
                    }
                } else 
                {
                    showIterations = false;
                }

            }
        }

        private void DrawRoad()
        {
            for (int i = 0; i < roadPath.Count; i++)
            {
                roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
            }
        }

        private void DrawVisitedIteration()
        {
            foreach (Spot s in pathFindAlgorithm.Iterations[0])
            {
                coin.CoinText.SetText(s.Visited.ToString());
                Instantiate(coin, tilemap.CellToWorld(new Vector3Int(s.X, s.Y, 0)), transform.rotation);
            }
            pathFindAlgorithm.Iterations.RemoveAt(0);
        }
    }
}
