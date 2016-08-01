using UnityEngine;
using System.Collections.Generic;
using System;
using Assets;

public class GridMasterScript : MonoBehaviour
{
    public static GridMasterScript Instance;

    public static GameObject CellTemplate;
    public GameObject MyCellTemplate;

    public List<List<Cell>> cellgrid;

    public int width = 30;
    public int height = 30;

    public int updateSkips = 60;
    private int updateCounter = 0;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        CellTemplate = MyCellTemplate;
        cellgrid = new List<List<Cell>>();

        for (int y = 0; y < height; y++)
        {
            cellgrid.Add(new List<Cell>());
            for (int x = 0; x < width; x++)
            {
                GameObject go = (GameObject)Instantiate(CellTemplate, new Vector3((-width / 2) + x, (height / 2) - y, -1), Quaternion.identity);
                Cell c = go.GetComponent<Cell>();
                c.Grid = cellgrid;
                c.X = x;
                c.Y = y;
                c.Alive = false;
                cellgrid[y].Add(c);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateCounter++;
        if (updateCounter % updateSkips != 0)
            return;

        AdvanceField();
    }

    public void AdvanceField()
    {
        cellgrid.ForEach((List<Cell> l) => l.ForEach((Cell c) => DoConwayStep(c)));
        cellgrid.ForEach((List<Cell> l) => l.ForEach((Cell c) => c.Transit()));
    }

    public void RandomizeField()
    {
        cellgrid.ForEach((List<Cell> l) => l.ForEach((Cell c) => c.Alive = UnityEngine.Random.Range(0, 2) == 0));
    }

    public void ClearField()
    {
        cellgrid.ForEach((List<Cell> l) => l.ForEach((Cell c) => { c.AliveNextRound = false; c.Transit(); }));
    }

    private void DoConwayStep(Cell c)
    {
        int count = c.CountNeighbors();

        if (c.Alive)
        {
            c.AliveNextRound = false;
            if (count >= 2 && count <= 3)
            {
                c.AliveNextRound = true;
            }
        }
        else
        {
            if (count == 3)
            {
                c.AliveNextRound = true;
            }
        }
    }
}
