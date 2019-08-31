using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : ICellEntity {

    public Cell Cell { get; set; }

    public bool HasBeenFound { get; private set; }
    public bool Moving { get; set; }

    public GameObject gameObject { get; set; }

    public float enemyMoveTime = 0.2f;
    public EntityManager entityManager;
    public Grid grid;

    public EnemyEntity(Cell cell)
    {
        Cell = cell;
        HasBeenFound = false;
        Moving = false;

        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        entityManager = gameManager.GetComponent<EntityManager>();
        grid = gameManager.GetComponent<Grid>();
    }

	public bool ExecuteAction()
    {
        return true;
    }

    public void Found()
    {
        // Reenable the child renderer when it gets derendered

        Renderer[] childrenRenderers = gameObject.GetComponentsInChildren<Renderer>();

        // k = 1 to skip parent renderer
        for (int k = 1; k < childrenRenderers.Length; k++)
        {
            childrenRenderers[k].enabled = true;
        }

        if (!HasBeenFound)
        {
            HasBeenFound = true;
        }

        if(!Moving)
        {
            entityManager.TestMove(Cell, this);
        }
    }

    public void MoveToCell(Cell cell)
    {
        Moving = true;
        entityManager.MoveEntity(Cell, cell);
        this.Cell = cell;
        
        entityManager.MoveCellObject(gameObject, gameObject.transform.position, cell.getCenterPositionForEntity(), enemyMoveTime, this);
    }

    public void HasFinishedMoving()
    {
        Moving = false;
        CheckIfOutsideHighlightedRegion();
    }

    void CheckIfOutsideHighlightedRegion()
    {
        /* checks to see if object has moved out of highlighted region, in which case it should
         * derendered( except for the eyes).                                                 */

        if(grid.currentHighlightedCell != Cell)
        {
            Renderer[] childrenRenderers = gameObject.GetComponentsInChildren<Renderer>();

            // hide parent renderer
            childrenRenderers[0].enabled = false;

            // k = 1 to skip parent renderer
            for (int k = 1; k < childrenRenderers.Length; k++)
            {
                childrenRenderers[k].enabled = true;
            }
        }
    }
}
