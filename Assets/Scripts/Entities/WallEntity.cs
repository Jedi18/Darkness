using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEntity : ICellEntity
{
    public Cell Cell
    { get; set; }

    public GameObject gameObject { get; set; }

    public WallEntity(Cell cell_in, GameObject pref)
    {
        Cell = cell_in;
        gameObject = pref;
    }

    public bool ExecuteAction()
    {
        // stop player from entering this cell
        return false;
    }
}
