using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    // Use this for initialization
    public ICellEntity[,] entities;
    public GameObject[] prefabs;
    public Grid grid;

    public void InitializeEntities(int x, int y)
    {
        entities = new ICellEntity[x, y];
    }

    public bool HasEntity(Cell cell)
    {
        if(cell == null)
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
        if(cell == null)
        {
            return null;
        }

        Cell[] cells = grid.GetNearbyCells(cell);
        ICellEntity[] nearbyEntities = new ICellEntity[9];

        int o = 0;

        for(int i=0;i<cells.Length;i++)
        {
            if(cells[i] != null)
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
}
