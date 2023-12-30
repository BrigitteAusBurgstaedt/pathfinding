using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace pathfinding
{
    public class PathFindVisuals : MonoBehaviour
    {
        [SerializeField] private Coin coin;
        [SerializeField] private CoinAStar coinAStar;
        public Tilemap tilemap;
        public Tilemap roadMap;
        public TileBase roadTile;
        private PathFindAlgorithm pathFindAlgorithm;
        private List<Spot> roadPath = new List<Spot>();
        private List<Spot> steps = new List<Spot>();
        public Vector2Int start;
        [SerializeField] private float waitTime = 0.1f;

        // Start is called before the first frame update
        void Start()
        {
            PathFindManager pathFindManager = GetComponent<PathFindManager>();
            pathFindManager.OnAlgoInit += PathFindManager_OnAlgoInit;
        }

        IEnumerator NextStep()
        {
            while (steps.Any())
            {
                DrawNextStep();
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void PathFindManager_OnAlgoInit(object sender, PathFindManager.OnAlgoInitArgs args)
        {
            pathFindAlgorithm = args.pathFindAlgorithm; 
            pathFindAlgorithm.OnSearchCompleted += PathFindAlgorithm_OnSearchCompleted;
        }

        private void PathFindAlgorithm_OnSearchCompleted(object sender, PathFindAlgorithm.OnSearchCompletedArgs args)
        {
            Debug.Log("Hallo? " + pathFindAlgorithm);
            steps = args.Steps;
            StartCoroutine(NextStep());
        }

        private void DrawRoad()
        {
            for (int i = 1; i < roadPath.Count - 1; i++) // Start und Ziel sollen nicht mit angezeigt werden als Pfad
            {
                roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
            }
        }

        private void DrawNextStep()
        {
            Spot s = steps[0];
            steps.RemoveAt(0);


            if (pathFindAlgorithm.GetType().Equals(typeof(BreadthFirst)) || pathFindAlgorithm.GetType().Equals(typeof(DepthFirst)))
            {
                coin.CoinText.SetText(s.Visited.ToString() + ".");
                Vector3 position = tilemap.CellToWorld(new Vector3Int(s.X, s.Y));
                Instantiate(coinAStar, position, transform.rotation);
            }
            else if (pathFindAlgorithm.GetType().Equals(typeof(Dijkstra)))
            {
                if (s.Distance == int.MaxValue)
                    coin.CoinText.SetText("∞");
                else
                    coin.CoinText.SetText(s.Distance.ToString());
                Instantiate(coin, tilemap.CellToWorld(new Vector3Int(s.X, s.Y, 0)), transform.rotation);
            }
            else if (pathFindAlgorithm.GetType().Equals(typeof(AStar)))
            {
                coinAStar.gCost.SetText(s.G.ToString());
                coinAStar.fCost.SetText(s.F.ToString());
                coinAStar.hCost.SetText(s.H.ToString());
                Vector3 position = tilemap.CellToWorld(new Vector3Int(s.X, s.Y));
                Instantiate(coinAStar, position, transform.rotation);
            }
        }

        /*
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
        */

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
