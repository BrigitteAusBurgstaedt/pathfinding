using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    private Grid grid;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid(4, 2, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(Utilities.GetMouseWorldPosition(), 56);
        }
    }
}
