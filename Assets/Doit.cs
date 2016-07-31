using UnityEngine;
using System.Collections.Generic;
using System;

public class Doit : MonoBehaviour
{
    private List<List<Cell>> cellgrid;

    public int width = 30;
    public int height = 30;

    public int updateSkips = 60;
    private int updateCounter = 0;

    // Use this for initialization
    void Start()
    {
        cellgrid = new List<List<Cell>>();

        for (int y = 0; y < height; y++)
        {
            cellgrid.Add(new List<Cell>());
            for (int x = 0; x < width; x++)
            {
                Cell c = new Cell(cellgrid, x, y, width, height);
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

        //Debug.Log("Stepping @" + DateTime.Now.ToString());
        cellgrid.ForEach((List<Cell> l) => l.ForEach((Cell c) => doConwayStep(c)));
        cellgrid.ForEach((List<Cell> l) => l.ForEach((Cell c) => c.transit()));
    }

    private void doConwayStep(Cell c)
    {
        int count = c.countNeighbors();

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

public class Cell
{
    private static GameObject cellTemplate;
    private List<List<Cell>> grid;

    public bool AliveNextRound { get; set; }
    public void transit()
    {
        Alive = AliveNextRound;
    }

    private bool alive;
    public bool Alive
    {
        get
        {
            return alive;
        }
        set
        {
            alive = value;
            GameObject.GetComponent<Renderer>().material.color = value ? Color.HSVToRGB(0, 0, .9f) : Color.HSVToRGB(0, 0, .0f);
        }
    }

    public int X { get; set; }
    public int Y { get; set; }

    public GameObject GameObject { get; set; }

    private void createCellTemplate()
    {
        if (cellTemplate == null)
            cellTemplate = GameObject.FindGameObjectWithTag("Player");
    }

    public Cell(List<List<Cell>> grid, int x, int y, int width, int height)
    {
        this.createCellTemplate();
        this.grid = grid;
        X = x;
        Y = y;
        GameObject = (GameObject)UnityEngine.Object.Instantiate(cellTemplate, new Vector3((-width / 2) + x, (height / 2) - y, -1), Quaternion.identity);
        Alive = UnityEngine.Random.Range(0, 2) == 0;
    }

    public Cell getNeighbor(int x, int y)
    {
        if (Y + y < 0 || Y + y >= grid.Count-1 || X + x < 0 || X + x >= grid[Y+y].Count-1)
            return null;

        return grid[Y + y][X + x];
    }

    public int countNeighbors()
    {
        int count = 0;
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Cell neighbor = getNeighbor(x, y);
                if (neighbor != null && neighbor.Alive)
                    count++;
            }
        }

        return count;
    }
}