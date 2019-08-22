using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell{

    public Vector3 topLeftPos;
    public int cellWidth;
    public int cellHeight;
    public GameObject currentElement;
    private int cellIndexX;
    private int cellIndexY;

    private Vector3 centerPos;

    public Cell(Vector3 pos, int width, int height, int x, int y)
    {
        topLeftPos = pos;
        cellWidth = width;
        cellHeight = height;
        cellIndexX = x;
        cellIndexY = y;

        centerPos = new Vector3(topLeftPos.x + cellWidth / 2, topLeftPos.y + cellHeight / 2, 0);
    }

    public Vector3 getCenterPosition()
    {
        //return new Vector3(topLeftPos.x + cellWidth / 2,  topLeftPos.y + cellHeight / 2, 0);
        return centerPos;
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
