using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : ICellEntity {

    public Cell Cell { get; set; }

    public bool HasBeenFound { get; private set; }

    public GameObject gameObject { get; set; }

    public EnemyEntity(Cell cell)
    {
        Cell = cell;
        HasBeenFound = false;
    }

	public bool ExecuteAction()
    {
        return true;
    }

    public void Found()
    {
        if(!HasBeenFound)
        {
            HasBeenFound = true;
        }

        // Reenable the child renderer when it gets derendered

        Renderer[] childrenRenderers = gameObject.GetComponentsInChildren<Renderer>();

        // k = 1 to skip parent renderer
        for (int k = 1; k < childrenRenderers.Length; k++)
        {
            childrenRenderers[k].enabled = true;
        }
    }
}
