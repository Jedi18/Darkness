using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public float enemyMoveToNextCellTime = 2f;

    // Use this for initialization
    public ICellEntity[,] entities;
    public GameObject[] prefabs;
    public Grid grid;
    public EntityGenerator entityGenerator;
    public PlayerController player;

    public EnemyEntity[] activatedEnemies;
    int enemyIndex = 0;

    public void InitializeEntities(int x, int y)
    {
        entities = new ICellEntity[x, y];
        activatedEnemies = new EnemyEntity[(int)(entityGenerator.initialNoOfEntities * entityGenerator.initialEnemyPercent) + 1];

        InvokeRepeating("MoveEnemies", 0.5f, enemyMoveToNextCellTime);
    }

    public bool HasEntity(Cell cell)
    {
        if (cell == null)
        {
            return false;
        }

        return !(entities[cell.GetCellIndexX(), cell.GetCellIndexY()] == null);
    }

    // assumes hasentity has been called before being called
    public ICellEntity GetEntity(Cell cell)
    {
        return entities[cell.GetCellIndexX(), cell.GetCellIndexY()];
    }

    // assumes cell has been checked for any containing entities
    public void AddWallEntity(Cell cell)
    {
        ICellEntity ent = new WallEntity(cell);
        entities[cell.GetCellIndexX(), cell.GetCellIndexY()] = ent;
        GameObject go = Instantiate(prefabs[0], new Vector3(cell.getCenterPosition().x, cell.getCenterPosition().y, -2), Quaternion.identity);
        ent.gameObject = go;
    }

    // assumes cell has been checked for any containing entities
    public void AddTrapEntity(Cell cell)
    {
        ICellEntity ent = new TrapEntity(cell);
        entities[cell.GetCellIndexX(), cell.GetCellIndexY()] = ent;
        GameObject go = Instantiate(prefabs[1], new Vector3(cell.getCenterPosition().x, cell.getCenterPosition().y, -2), Quaternion.identity);
        ent.gameObject = go;
    }

    public void AddEnemyEntity(Cell cell)
    {
        ICellEntity ent = new EnemyEntity(cell);
        entities[cell.GetCellIndexX(), cell.GetCellIndexY()] = ent;
        GameObject go = Instantiate(prefabs[2], new Vector3(cell.getCenterPosition().x, cell.getCenterPosition().y, -2), Quaternion.identity);
        ent.gameObject = go;
    }

    public ICellEntity[] GetNearbyCellEntities(Cell cell)
    {
        if (cell == null)
        {
            return null;
        }

        Cell[] cells = grid.GetNearbyCells(cell);
        ICellEntity[] nearbyEntities = new ICellEntity[9];

        int o = 0;

        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                if (HasEntity(cells[i]))
                {
                    nearbyEntities[o] = GetEntity(cells[i]);
                }
            }

            o++;
        }

        return nearbyEntities;
    }

    public void MoveEntity(Cell oldCell, Cell newCell)
    {
        // oldcell != newCell condition in case the enemy is at the borders of the grid, so recieves same cell
        if (oldCell != newCell)
        {
            entities[newCell.GetCellIndexX(), newCell.GetCellIndexY()] = entities[oldCell.GetCellIndexX(), oldCell.GetCellIndexY()];
            entities[oldCell.GetCellIndexX(), oldCell.GetCellIndexY()] = null;
        }
    }

    /* ---------   Coroutine and public function to move an object ---------- */

    IEnumerator MoveObject(GameObject obj, Vector3 source, Vector3 destination, float timeToMove, ICellEntity cellEntity)
    {
        float startTime = Time.time;

        while (Time.time - startTime < timeToMove)
        {
            obj.transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / timeToMove);
            yield return null;
        }

        transform.position = destination;

        // for cell entities
        if (cellEntity != null)
        {
            cellEntity.HasFinishedMoving();
        }
    }

    public void MoveCellObject(GameObject obj, Vector3 source, Vector3 destination, float timeToMove, ICellEntity cellEntity)
    {
        StartCoroutine(MoveObject(obj, source, destination, timeToMove, cellEntity));
    }

    /* --------------------- Move object code finished -------------------- */

    public void ActivateEnemy(EnemyEntity entity)
    {
        activatedEnemies[enemyIndex] = entity;
        enemyIndex++;
        StartCoroutine(SetCanBeMovedEnemy(entity));
    }

    // maintains list of activated enemies and at small time intervals loops through them and makes them move towards the player
    public void MoveEnemies()
    {
        if (player.lightSwitchedOn)
        {
            for (int i = 0; i < enemyIndex; i++)
            {
                if(activatedEnemies[i] != null)
                {
                    if (activatedEnemies[i].canBeMoved)
                    {
                        activatedEnemies[i].MoveTowardsPlayer();
                    }
                }
            }
        }
    }

    IEnumerator SetCanBeMovedEnemy(EnemyEntity enemyEntity)
    {
        yield return new WaitForSeconds(enemyEntity.minCountdownBeforeMoving);
        enemyEntity.canBeMoved = true;
    }

    public void DestroyEntity(ICellEntity entity, float time)
    {
        StartCoroutine(DestroyEntityAfter(entity, time));
    }

    IEnumerator DestroyEntityAfter(ICellEntity entity, float time)
    {
        yield return new WaitForSeconds(time);
        Cell entityCell = entity.Cell;
        entities[entityCell.GetCellIndexX(), entityCell.GetCellIndexY()] = null;

        // remove from activated enemies list
        for(int i=0;i<enemyIndex;i++)
        {
            if(activatedEnemies[i] == entity)
            {
                activatedEnemies[i] = null;
            }
        }

        Destroy(entity.gameObject);
    }
}
