using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Cell : MonoBehaviour
    {
        public List<List<Cell>> Grid;

        public bool AliveNextRound { get; set; }
        public void Transit()
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
                gameObject.GetComponent<Renderer>().material.color = value ? Color.HSVToRGB(0, 0, .9f) : Color.HSVToRGB(0, 0, .0f);
            }
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Cell GetNeighbor(int x, int y)
        {
            if (Y + y < 0 || Y + y >= Grid.Count - 1 || X + x < 0 || X + x >= Grid[Y + y].Count - 1)
                return null;

            return Grid[Y + y][X + x];
        }

        public int CountNeighbors()
        {
            int count = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    Cell neighbor = GetNeighbor(x, y);
                    if (neighbor != null && neighbor.Alive)
                        count++;
                }
            }

            return count;
        }

        void OnMouseDown()
        {
            Alive = !Alive;
        }
    }
}
