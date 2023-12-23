using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class PathFindVisuals : MonoBehaviour
    {
        public string NextScene;
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

            pathFindAlgorithm = new AStar(tilemap);
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
                DestroyAllCoins();
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
               
                start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DrawRoad();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SceneManager.LoadScene(NextScene);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DrawCost();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                DestroyAllCoins();
            }


            if (showIterations)
            {
                if (pathFindAlgorithm.Iterations.Any())
                {
                    time -= Time.deltaTime;
                    if (time < 0f)
                    {
                        time = 0.2f;
                        DrawNextStep();             
                    }
                } else 
                {
                    showIterations = false;
                }

            }
        }

        private void DrawRoad()
        {
            for (int i = 1; i < roadPath.Count - 1; i++) // Start und Ziel sollen nicht mit angezeigt werden als Pfad
            {
                roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
            }
        }

        private void DrawVisitedIteration()
        {
            foreach (Spot s in pathFindAlgorithm.Iterations[0])
            {
                coin.CoinText.SetText(s.Visited.ToString() + ".");
                Instantiate(coin, tilemap.CellToWorld(new Vector3Int(s.X, s.Y, 0)), transform.rotation);
            }
            pathFindAlgorithm.Iterations.RemoveAt(0);
        }

        private void DrawDistanceIteration() 
        {
            foreach (Spot s in pathFindAlgorithm.Iterations[0])
            {
                if (s.Distance == int.MaxValue)
                    coin.CoinText.SetText("∞");
                else
                    coin.CoinText.SetText(s.Distance.ToString());
                Instantiate(coin, tilemap.CellToWorld(new Vector3Int(s.X, s.Y, 0)), transform.rotation);
            }
            pathFindAlgorithm.Iterations.RemoveAt(0);
        }

        private void DrawNextStep()
        {
            Vector3 position = new Vector3();
            Instantiate(pathFindAlgorithm.GetVisualNextStep(tilemap, position), position, transform.rotation);
        }

        private void DrawCost()
        {
            foreach (Spot s in pathFindAlgorithm.Graph.Spots)
            {
                if (s.IsWalkable)
                {
                    coin.CoinText.SetText(s.Cost.ToString());
                    Instantiate(coin, tilemap.CellToWorld(new Vector3Int(s.X, s.Y, 0)), transform.rotation);
                }
            }
        }

        private void DestroyAllCoins()
        {
            GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin");
            foreach (GameObject obj in allCoins)
            {
                Destroy(obj);
            }
        }
    }
}
