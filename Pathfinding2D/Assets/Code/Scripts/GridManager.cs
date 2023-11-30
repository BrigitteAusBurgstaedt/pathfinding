using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;
    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        roadMap.CompressBounds();
        camera = Camera.main;

        astar = new Astar(tilemap);
    }

    private void DrawRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
        }
    }
    // Update is called once per frame
    public Vector2Int start;
    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            start = new Vector2Int(gridPos.x, gridPos.y);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }
        if (Input.GetMouseButton(0))
        {

            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);

            if (roadPath != null && roadPath.Count > 0)
                roadPath.Clear();

            astar.UpdateSpots(tilemap);
            roadPath = astar.CreatePath(start, new Vector2Int(gridPos.x, gridPos.y), 1000);
            if (roadPath == null)
                return;

            DrawRoad();
            start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
        }
    }
}
