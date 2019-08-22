using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int gridX;
    public int gridY;

    public int cellWidth;
    public int cellHeight;

    [SerializeField]
    private Cell[,] grid;

    public GameObject cell;

    public float shiftLeft;
    public float shiftBottom;

	// Use this for initialization
	void Start () {
        grid = new Cell[gridX,gridY];
        shiftLeft = (gridX*cellWidth) / 2;
        shiftBottom = (gridY*cellHeight) / 2;

        InstantiateCells();
        DrawCells();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InstantiateCells()
    {
        for(int x=0;x<gridX;x++)
        {
            for(int y=0;y<gridY;y++)
            {
                grid[x, y] = new Cell(new Vector3(x * cellWidth - shiftLeft, y * cellHeight - shiftBottom, 0), cellWidth, cellHeight, x, y);
            }
        }
    }

    private void DrawCells()
    {
        for(int x=0;x<gridX;x++)
        {
            for(int y=0;y<gridY;y++)
            {
                Vector3 pos = grid[x, y].getCenterPosition();
                Instantiate(cell, pos, Quaternion.identity);
            }
        }
    }

    public Cell GetRandomCell()
    {
        int randx = (int)Random.Range(0, gridX);
        int randy = (int)Random.Range(0, gridY);

        return grid[randx, randy];
    }

    public Cell GetNextCellHorizontal(int h, Cell curCell)
    {
        // get cell to right of current cell
        int i = curCell.GetCellIndexX();
        int j = curCell.GetCellIndexY();

        if (h == 1)
        {
            // check if there exists a cell to the right of current cell, if not return current cell
            if(i == gridX-1)
            {
                return curCell;
            }
            else
            {
                return grid[i + 1, j];
            }
        }
        else if(h == -1)
        {
            if(i == 0)
            {
                return curCell;
            }
            else
            {
                return grid[i - 1, j];
            }
        }
        else
        {
            return curCell;
        }
    }

    public Cell GetNextCellVertical(int v, Cell curCell)
    {
        // get cell to right of current cell
        int i = curCell.GetCellIndexX();
        int j = curCell.GetCellIndexY();

        if (v == 1)
        {
            if (j == gridY - 1)
            {
                return curCell;
            }
            else
            {
                return grid[i, j + 1];
            }
        }
        else if (v == -1)
        {
            if (j == 0)
            {
                return curCell;
            }
            else
            {
                return grid[i, j - 1];
            }
        }
        else
        {
            return curCell;
        }
    }
}
