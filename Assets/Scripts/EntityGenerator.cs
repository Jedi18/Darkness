using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGenerator : MonoBehaviour {

    public EntityManager entityManager;
    public Grid grid;

    public int initialNoOfEntities;
    public float initialWallPercent;
    public float initialTrapPercent;

	// Use this for initialization
	void Start () {
	}
	
    public void Generate(int numEntities, float wallPercent, float trapPercent)
    {
        int numWalls = (int)(wallPercent * numEntities);
        int numTraps = (int)(trapPercent * numEntities);

        for(int i=0;i<numWalls;i++)
        {
            entityManager.AddWallEntity(grid.GetUnoccupiedRandomCell());
        }

        for(int j=0;j<numTraps;j++)
        {
            entityManager.AddTrapEntity(grid.GetUnoccupiedRandomCell());
        }
    }
}
