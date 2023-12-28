using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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

        }

        // Update is called once per frame
        void Update()
        {


            if (showIterations)
            {
                time -= Time.deltaTime;
                if (time < 0f)
                {
                    time = 0.2f;
                    if (!DrawNextStep()) showIterations = false;
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


        private bool DrawNextStep()
        {
            if (!Steps.Any())
            {
                position = new Vector3Int();
                return null;
            }

            coinAStar = new CoinAStar();

            Spot s = Steps[0];
            Steps.RemoveAt(0);
            coinAStar.gCost.SetText(s.G.ToString());
            coinAStar.fCost.SetText(s.F.ToString());
            coinAStar.hCost.SetText(s.H.ToString());
            position = tilemap.CellToWorld(new Vector3Int(s.X, s.Y));
            Instantiate(coinAStar, position, transform.rotation);
            return coinAStar;
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
