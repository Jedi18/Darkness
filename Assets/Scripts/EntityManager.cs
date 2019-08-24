﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    // Use this for initialization
    public ICellEntity[,] entities;
    public GameObject[] prefabs;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeEntities(int x, int y)
    {
        entities = new ICellEntity[x, y];
    }

    public bool HasEntity(Cell cell)
    {
        return !(entities[cell.GetCellIndexX(), cell.GetCellIndexY()] == null);
    }

    // assumes hasentity has been called before being called
    public ICellEntity GetEntity(Cell cell)
    {
        return entities[cell.GetCellIndexX(), cell.GetCellIndexY()];
    }

    public void AddWallEntity(Cell cell)
    {
        ICellEntity ent = new WallEntity(cell, prefabs[0]);
        entities[cell.GetCellIndexX(), cell.GetCellIndexY()] = ent;
        Instantiate(ent.gameObject, new Vector3(cell.getCenterPosition().x, cell.getCenterPosition().y, -2), Quaternion.identity);
    }
}