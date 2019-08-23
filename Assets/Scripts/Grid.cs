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
    public Color highlitedCellColor;
    public Color originalCellColor;

    public float shiftLeft;
    public float shiftBottom;

    private GameObject[,] cells;
    Cell currentHighlightedCell;

	// Use this for initialization
	void Start () {
        grid = new Cell[gridX,gridY];
        cells = new GameObject[gridX, gridY];
        shiftLeft = (gridX*cellWidth) / 2;
        shiftBottom = (gridY*cellHeight) / 2;
        currentHighlightedCell = null;

        InstantiateCells();
        DrawCells();
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
                cells[x,y] = Instantiate(cell, pos, Quaternion.identity);
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

        if(curCell == null)
        {
            return null;
        }

        // get cell to right of current cell
        int i = curCell.GetCellIndexX();
        int j = curCell.GetCellIndexY();

        if (h == 1)
        {
            // check if there exists a cell to the right of current cell, if not return current cell
            if(i == gridX-1)
            {
                return null;
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
                return null;
            }
            else
            {
                return grid[i - 1, j];
            }
        }
        else
        {
            return null;
        }
    }

    //Slightly modified version to return current cell whenever the actual funciton returns null
    public Cell GetNextCellHorizontalMove(int h, Cell curCell)
    {
        Cell newCell = GetNextCellHorizontal(h, curCell);

        if(newCell == null)
        {
            return curCell;
        }
        else
        {
            return newCell;
        }
    }

    public Cell GetNextCellVerticalMove(int h, Cell curCell)
    {
        Cell newCell = GetNextCellVertical(h, curCell);

        if (newCell == null)
        {
            return curCell;
        }
        else
        {
            return newCell;
        }
    }

    public Cell GetNextCellVertical(int v, Cell curCell)
    {
        if(curCell == null)
        {
            return  null;
        }

        // get cell to right of current cell
        int i = curCell.GetCellIndexX();
        int j = curCell.GetCellIndexY();

        if (v == 1)
        {
            if (j == gridY - 1)
            {
                return null;
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
                return null;
            }
            else
            {
                return grid[i, j - 1];
            }
        }
        else
        {
            return null;
        }
    }

    // returns cell at given position, else returns null
    public Cell GetCellAtWorldPosition (Vector3 coord)
    {
        // add shift left and bottom to account for subtraction done earlier 

        int i = (int)Mathf.Floor((coord.x + shiftLeft) / cellWidth);
        int j = (int)Mathf.Floor((coord.y + shiftBottom) / cellHeight);

        if (i >= gridX || j >= gridY || i < 0 || j < 0)
        {
            return null;
        }
        else
        {
            return grid[i, j];
        }
    }
    public void HighlightCell(Cell cell)
    {
        if(cell == null)
        {
            return;
        }
        if (cell != currentHighlightedCell)
        {     
            if(currentHighlightedCell != null)
            {
                Renderer oldCellRenderer = cells[currentHighlightedCell.GetCellIndexX(), currentHighlightedCell.GetCellIndexY()].GetComponent<Renderer>();
                oldCellRenderer.material.color = originalCellColor;
            }

            currentHighlightedCell = cell;
            Renderer cellRenderer = cells[cell.GetCellIndexX(), cell.GetCellIndexY()].GetComponent<Renderer>();
            cellRenderer.material.color = highlitedCellColor;
        }
    }
}
