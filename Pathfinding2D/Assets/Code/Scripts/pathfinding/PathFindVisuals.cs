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
        private List<Spot> steps = new List<Spot>();
        private float waitTime = 0.1f;
        private Coroutine nextStep = null;

        // Start is called before the first frame update
        void Start()
        {
            PathFindManager pathFindManager = GetComponent<PathFindManager>();
            PathFindManagerAll pathFindManagerAll = GetComponent<PathFindManagerAll>();

            if (pathFindManager != null)
            {
                pathFindManager.OnAlgoInit += PathFindManager_OnAlgoInit;
                pathFindManager.OnDestroyAllCoins += PathFindManager_OnDestroyAllCoins;
                pathFindManager.OnDrawCost += PathFindManager_OnDrawCost;
                pathFindManager.OnDrawRoad += PathFindManager_OnDrawRoad;
            }
            if (pathFindManagerAll != null)
            {
                pathFindManagerAll.OnAlgoInit += PathFindManager_OnAlgoInit;
                pathFindManagerAll.OnDestroyAllCoins += PathFindManager_OnDestroyAllCoins;
                pathFindManagerAll.OnDrawCost += PathFindManager_OnDrawCost;
                pathFindManagerAll.OnDrawRoad += PathFindManager_OnDrawRoad;
            }
        }

        private void PathFindManager_OnDrawRoad(object sender, PathFindManager.OnDrawRoadArgs e)
        {
            for (int i = 1; i < e.road.Count - 1; i++) // Start und Ziel sollen nicht mit angezeigt werden als Pfad
            {
                roadMap.SetTile(new Vector3Int(e.road[i].X, e.road[i].Y, 0), roadTile);
            }
        }

        private void PathFindManager_OnDrawCost(object sender, PathFindManager.OnDrawCostArgs e)
        {
            foreach (Spot s in e.graph.Spots)
            {
                if (s.IsWalkable)
                {
                    coin.CoinText.SetText(s.Cost.ToString() + "€");
                    Instantiate(coin, tilemap.CellToWorld(new Vector3Int(s.X, s.Y, 0)), transform.rotation);
                }
            }
        }

        private void PathFindManager_OnDestroyAllCoins(object sender, EventArgs e)
        {
            if (nextStep != null) StopCoroutine(nextStep);
            GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin");
            foreach (GameObject obj in allCoins)
            {
                Destroy(obj);
            }
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
            waitTime = (pathFindAlgorithm.GetType() == typeof(Dijkstra)) ? 0.01f : 0.1f;

            if (nextStep != null)
            {
                StopCoroutine(nextStep);
                GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin");
                foreach (GameObject obj in allCoins)
                {
                    Destroy(obj);
                }
            }
            pathFindAlgorithm.OnSearchCompleted += PathFindAlgorithm_OnSearchCompleted;
            Debug.Log("Algo gefunden " + pathFindAlgorithm);
        }

        private void PathFindAlgorithm_OnSearchCompleted(object sender, PathFindAlgorithm.OnSearchCompletedArgs args)
        {
            steps = args.Steps;
            if (nextStep != null)
            {
                StopCoroutine(nextStep);
                GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin");
                foreach (GameObject obj in allCoins)
                {
                    Destroy(obj);
                }
            }
            nextStep = StartCoroutine(NextStep());
        }

        private void DrawNextStep()
        {
            Spot s = steps[0];
            steps.RemoveAt(0);


            if (pathFindAlgorithm.GetType().Equals(typeof(BreadthFirst)) || pathFindAlgorithm.GetType().Equals(typeof(DepthFirst)))
            {
                coin.CoinText.SetText(s.Visited.ToString() + ".");
                Vector3 position = tilemap.CellToWorld(new Vector3Int(s.X, s.Y));
                Instantiate(coin, position, transform.rotation);
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
    }
}
