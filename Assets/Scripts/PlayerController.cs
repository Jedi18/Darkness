using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Grid grid;
    private Cell currentCell;

	// Use this for initialization
	void Start () {
        //InvokeRepeating("spawnAtRandomPositions", 0.2f, 0.5f);
        Invoke("spawnAtRandomPositions", 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        /* Gets corresponding position from grid depending upon input
         * and then calls SetPosition.
         * only works when one key is pressed
         * */
        if (Input.GetKeyDown("up") || Input.GetKeyDown("down") || Input.GetKeyDown("left") || Input.GetKeyDown("right"))
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            // in case both horizontal and vertical are pressed, do nothing
            if (x != 0 && y != 0)
            {
                return;
            }

            if (x == 0 && y == 0)
            {
                return;
            }

            if (x == 0)
            {
                SetPosition(grid.GetNextCellVertical((int)y, currentCell));
            }
            else if (y == 0)
            {
                SetPosition(grid.GetNextCellHorizontal((int)x, currentCell));
            }
        }
    }

    private void SetPosition(Cell cell)
    {
        currentCell = cell;
        gameObject.transform.position = currentCell.getCenterPosition();
    }

    private void spawnAtRandomPositions()
    {
        SetPosition(grid.GetRandomCell());
    }
}
