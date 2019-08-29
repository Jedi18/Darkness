using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGenerator : MonoBehaviour {

    public EntityManager entityManager;
    public Grid grid;

    public int initialNoOfEntities;
    public float initialWallPercent;
    public float initialTrapPercent;
    public float initialEnemyPercent;

	// Use this for initialization
	void Start () {
	}
	
    public void Generate(int numEntities, float wallPercent, float trapPercent, float enemyPercent)
    {
        int numWalls = (int)(wallPercent * numEntities);
        int numTraps = (int)(trapPercent * numEntities);
        int numEnemies = (int)(enemyPercent * numEntities);

        for(int i=0;i<numWalls;i++)
        {
            entityManager.AddWallEntity(grid.GetUnoccupiedRandomCell());
        }

        for(int j=0;j<numTraps;j++)
        {
            entityManager.AddTrapEntity(grid.GetUnoccupiedRandomCell());
        }

        for(int k=0;k<numEnemies;k++)
        {
            entityManager.AddEnemyEntity(grid.GetUnoccupiedRandomCell());
        }
    }
}
