using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell{

    Material material;
    public Vector3 topLeftPos;
    public int cellWidth;
    public int cellHeight;
    public GameObject currentElement;
    private int cellIndexX;
    private int cellIndexY;

    public Cell(Vector3 pos, int width, int height, int x, int y)
    {
        topLeftPos = pos;
        cellWidth = width;
        cellHeight = height;
        cellIndexX = x;
        cellIndexY = y;
    }

    public Vector3 getCenterPosition()
    {
        return new Vector3(topLeftPos.x + cellWidth / 2,  topLeftPos.y + cellHeight / 2, 0);
    }

    public int GetCellIndexX()
    {
        return cellIndexX;
    }

    public int GetCellIndexY()
    {
        return cellIndexY;
    }
}
