﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEntity : ICellEntity
{
    public Cell Cell
    { get; set; }

    public GameObject gameObject { get; set; }

    public WallEntity(Cell cell_in)
    {
        Cell = cell_in;
    }

    public bool ExecuteAction()
    {
        // stop player from entering this cell
        return false;
    }

    public void Found()
    {

    }

    public void HasFinishedMoving()
    {

    }

    public bool ExecuteActionEntity(ICellEntity entity)
    {
        return false;
    }
}
