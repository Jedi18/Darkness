using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEntity : ICellEntity
{
    public Cell Cell
    { get; set; }

    public WallEntity(Cell cell_in)
    {
        Cell = cell_in;
    }

    public bool ExecuteAction()
    {
        // stop player from entering this cell
        return false;
    }
}
