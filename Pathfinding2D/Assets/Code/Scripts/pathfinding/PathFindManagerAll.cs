using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static pathfinding.PathFindManager;

namespace pathfinding
{
    public class PathFindManagerAll : MonoBehaviour
    {
        public event EventHandler<OnAlgoInitArgs> OnAlgoInit;
        public event EventHandler<OnDrawRoadArgs> OnDrawRoad;
        public event EventHandler<OnDrawCostArgs> OnDrawCost;
        public event EventHandler OnDestroyAllCoins;

        [SerializeField] private string nextScene;
        public int indexOfAlgorithm { get; set; }
        private int _indexOfAlgorithm;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tilemap roadMap;
        [SerializeField] private Vector2Int startPos;
        private PathFindAlgorithm pathFindAlgorithm;
        private List<Spot> roadPath = new();
        private new Camera camera;

        // Start is called before the first frame update
        void Start()
        {
            roadMap.CompressBounds();
            camera = Camera.main;

            pathFindAlgorithm = new AStar(tilemap);

            OnAlgoInit?.Invoke(this, new OnAlgoInitArgs() { pathFindAlgorithm = pathFindAlgorithm });
        }

        public void ChangeAlgorithm(int index)
        {
            _indexOfAlgorithm = index;
            switch (_indexOfAlgorithm)
            {
                case 1:
                    pathFindAlgorithm = new BreadthFirst(tilemap);
                    break;
                case 2:
                    pathFindAlgorithm = new DepthFirst(tilemap);
                    break;
                case 3:
                    pathFindAlgorithm = new Dijkstra(tilemap);
                    break;
                case 4:
                    pathFindAlgorithm = new AStar(tilemap);
                    break;
                default:
                    pathFindAlgorithm = new AStar(tilemap);
                    break;
            }
            OnAlgoInit?.Invoke(this, new OnAlgoInitArgs() { pathFindAlgorithm = pathFindAlgorithm });
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1)) // Rechte Maustaste runter gedrückt -> Start an Position setzten
            {
                Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tilemap.WorldToCell(world);
                startPos = new Vector2Int(gridPos.x, gridPos.y);
            }
            if (Input.GetMouseButton(2)) // Mittlere Maustaste betätigt -> Road an Position löschen
            {
                Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tilemap.WorldToCell(world);
                roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
                OnDestroyAllCoins?.Invoke(this, EventArgs.Empty);
            }
            if (Input.GetMouseButtonDown(0)) // Linke Maustaste runter gedrückt -> Pfad zur Position Zeichnen
            {
                Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPos = tilemap.WorldToCell(world);

                if (roadPath != null && roadPath.Count > 0) // Löschen der alten Pfadliste
                    roadPath.Clear();

                pathFindAlgorithm.LoadGraph(tilemap); // TODO Übergangslösung verbessern (löst das Memory Problem)
                roadPath = pathFindAlgorithm.CreatePath(startPos, new Vector2Int(gridPos.x, gridPos.y));
                if (!roadPath.Any())
                    return;
               
                startPos = new Vector2Int(roadPath[0].X, roadPath[0].Y);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnDrawRoad?.Invoke(this, new OnDrawRoadArgs() { road = roadPath });
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SceneManager.LoadScene(nextScene);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnDrawCost?.Invoke(this, new OnDrawCostArgs() { graph = pathFindAlgorithm.Graph });
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                OnDestroyAllCoins?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
