using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;

    // Start is called before the first frame update
    private void Start()
    {
        pathfinding = new Pathfinding(16, 9);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Utilities.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i=0; i<path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f + pathfinding.GetGrid().GetOriginPosition(), new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f + pathfinding.GetGrid().GetOriginPosition(), Color.green, 100f);
                }
            } else
            {
                Debug.Log("Kein Pfad gefunden!");
            }
        }
    }
}
