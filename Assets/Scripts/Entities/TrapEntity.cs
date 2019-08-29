using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEntity : ICellEntity {

    public Cell Cell
    { get; set; }

    public GameObject gameObject { get; set; }

    public TrapEntity(Cell cell_in)
    {
        Cell = cell_in;
    }

    public bool ExecuteAction()
    {
        return true;
    }

    public void Found()
    {

    }
}
