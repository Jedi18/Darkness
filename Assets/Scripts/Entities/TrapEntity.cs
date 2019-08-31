using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEntity : ICellEntity {

    public Cell Cell
    { get; set; }

    public float destroyTime = 0.2f;
    public EntityManager entityManager;

    public GameObject gameObject { get; set; }

    public TrapEntity(Cell cell_in)
    {
        Cell = cell_in;
        entityManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EntityManager>();
    }

    public bool ExecuteAction()
    {
        return true;
    }

    public void Found()
    {

    }

    public void HasFinishedMoving()
    {

    }

    public bool ExecuteActionEntity(ICellEntity entity)
    {
        entityManager.DestroyEntity(entity, destroyTime);
        return true;
    }
}
