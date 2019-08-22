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
        CellRelativeToPlayerHighlighting();    
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
                SetPosition(grid.GetNextCellVerticalMove((int)y, currentCell));
            }
            else if (y == 0)
            {
                SetPosition(grid.GetNextCellHorizontalMove((int)x, currentCell));
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

    private void CellRelativeToPlayerHighlighting()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // determine where is the point is relative to player

        // get angle between vector(point to player) and x axis
        Vector2 disp = point - transform.position;
        float angle = Vector2.SignedAngle(disp, Vector3.left);

        // right - 135 to 180 and -180 to -135
        // top  - 45 to 135
        // left - -45 to 45
        // bottom - -135 to -45

        if(angle >= 45 && angle < 135)
        {
            // top
            grid.HighlightCell(grid.GetNextCellVertical(1, currentCell));
        }
        else if(angle >= -45 && angle < 45)
        {
            // left
            grid.HighlightCell(grid.GetNextCellHorizontal(-1, currentCell));
        }
        else if(angle >= -135 && angle < -45)
        {
            // bottom
            grid.HighlightCell(grid.GetNextCellVertical(-1, currentCell));
        }
        else
        {
            // right
            grid.HighlightCell(grid.GetNextCellHorizontal(1, currentCell));
        }
    }

    /*private void UpdateMousePosition()
{
    Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Cell selectedCell = grid.GetCellAtWorldPosition(point);

    if(selectedCell != null)
    {
        grid.HighlightCell(selectedCell);
    }
} */
}
