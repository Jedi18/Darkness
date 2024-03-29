﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Grid grid;
    private Cell currentCell;
    public EntityManager entityManager;
    public EntityGenerator entityGenerator;
    public CameraController cameraController;
    public GameState gameState;

    public float playerMoveTime;

    public Light highlightCellLight;
    public Light directionalLight;

    public bool lightSwitchedOn = true;

	// Use this for initialization
	void Start () {
        //InvokeRepeating("spawnAtRandomPositions", 0.2f, 0.5f);
        Invoke("spawnAtCenter", 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
        
        if(gameState.PlayerAlive)
        {
            HandlePlayerInput();
            CellRelativeToPlayerHighlighting();
        }
    }

    private void HandlePlayerInput()
    {
        /* Gets corresponding position from grid depending upon input
         * and then calls SetPosition.
         * only works when one key is pressed
         * */
        if (Input.GetKeyDown("up") || Input.GetKeyDown("down") || Input.GetKeyDown("left") || Input.GetKeyDown("right") || Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
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

            Cell nextCell;

            if (x == 0)
            {
                nextCell = grid.GetNextCellVerticalMove((int)y, currentCell);
            }
            else // y == 0
            {
                nextCell = grid.GetNextCellHorizontalMove((int)x, currentCell);
            }

            // if cell has entity, check to see if entity is passable
            if (entityManager.HasEntity(nextCell))
            {
                ICellEntity entity = entityManager.GetEntity(nextCell);

                if (entity.ExecuteAction())
                {
                    SetPosition(nextCell);
                }
            }
            else
            {
                SetPosition(nextCell);
            }
        }

        if(Input.GetKeyDown("t"))
        {
            ToggleLight();
        }
    }

    private void SetPosition(Cell cell)
    {
        LightOrDarkenCurrentCell(false);

        currentCell = cell;
        Vector3 cellPosition = currentCell.getCenterPosition();
        // transform.position.z must be negative for it to properly render above the cells
        Vector3 newPosition = new Vector3(cellPosition.x, cellPosition.y, gameObject.transform.position.z);
        StartCoroutine(MovePlayer(transform.position, newPosition, playerMoveTime));

        // update camera on player movement
        cameraController.PlayerMoved(newPosition);

        LightOrDarkenCurrentCell(true);
    }

    private void spawnAtRandomPositions()
    {
        SetPosition(grid.GetUnoccupiedRandomCell());

        //spawn entities
        entityGenerator.Generate(entityGenerator.initialNoOfEntities, entityGenerator.initialWallPercent, entityGenerator.initialTrapPercent,entityGenerator.initialEnemyPercent);
    }

    private void spawnAtCenter()
    {
        SetPosition(grid.GetCellAtIndex(grid.gridX/2, grid.gridY/2));
        //spawn entities
        entityGenerator.Generate(entityGenerator.initialNoOfEntities, entityGenerator.initialWallPercent, entityGenerator.initialTrapPercent,entityGenerator.initialEnemyPercent);
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

        if(lightSwitchedOn)
        {
            RenderHighlightedEntity(false);
        }

        if(angle >= 45 && angle < 135)
        {
            // top
            grid.HighlightCell(grid.GetNextCellVertical(1, currentCell), currentCell, highlightCellLight);
        }
        else if(angle >= -45 && angle < 45)
        {
            // left
            grid.HighlightCell(grid.GetNextCellHorizontal(-1, currentCell), currentCell, highlightCellLight);
        }
        else if(angle >= -135 && angle < -45)
        {
            // bottom
            grid.HighlightCell(grid.GetNextCellVertical(-1, currentCell), currentCell, highlightCellLight);
        }
        else
        {
            // right
            grid.HighlightCell(grid.GetNextCellHorizontal(1, currentCell), currentCell, highlightCellLight);
        }

        if(lightSwitchedOn)
        {
            RenderHighlightedEntity(true);
        }
    }

    public void LightOrDarkenCurrentCell(bool light)
    {
        if(currentCell != null)
        {
            if(light)
            {
                grid.GetGameObjectAtCell(currentCell).layer = LayerMask.NameToLayer(grid.lightedCell);
            }
            else
            {
                grid.GetGameObjectAtCell(currentCell).layer = LayerMask.NameToLayer(grid.unlightedCell);
            }
        }

    }

    IEnumerator MovePlayer(Vector3 source, Vector3 destination, float timeToMove)
    {
        float startTime = Time.time;

        while(Time.time - startTime < timeToMove)
        {
            transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / timeToMove);
            yield return null;
        }

        transform.position = destination;
    }

    // Redudant code, not used now
    public void RenderNearbyEntities(bool render_on)
    {
        if(currentCell != null)
        {
            ICellEntity[] entities = entityManager.GetNearbyCellEntities(currentCell);

            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i] != null)
                {
                    if (render_on)
                    {
                        entities[i].gameObject.GetComponent<Renderer>().enabled = true;

                        Renderer[] childrenRenderers = entities[i].gameObject.GetComponentsInChildren<Renderer>();

                        for(int k=0;k<childrenRenderers.Length;k++)
                        {
                            childrenRenderers[k].enabled = true;
                        }
                    }
                    else
                    {
                        entities[i].gameObject.GetComponent<Renderer>().enabled = false;

                        Renderer[] childrenRenderers = entities[i].gameObject.GetComponentsInChildren<Renderer>();
                        for (int k = 0; k < childrenRenderers.Length; k++)
                        {
                            childrenRenderers[k].enabled = false;
                        }
                    }
                }
            }
        }
    }

    public void RenderHighlightedEntity(bool render_on)
    {
        if(grid.currentHighlightedCell != null && entityManager.HasEntity(grid.currentHighlightedCell))
        {
            ICellEntity cellEntity = entityManager.GetEntity(grid.currentHighlightedCell);

            if (render_on)
            {
                cellEntity.gameObject.GetComponent<Renderer>().enabled = true;
                Renderer[] childrenRenderers = cellEntity.gameObject.GetComponentsInChildren<Renderer>();

                for (int k = 0; k < childrenRenderers.Length; k++)
                {
                    childrenRenderers[k].enabled = true;
                }
            }
            else
            {
                cellEntity.gameObject.GetComponent<Renderer>().enabled = false;

                Renderer[] childrenRenderers = cellEntity.gameObject.GetComponentsInChildren<Renderer>();

                for (int k = 0; k < childrenRenderers.Length; k++)
                {
                    childrenRenderers[k].enabled = false;
                }
            }

            // execute on found action
            cellEntity.Found();
        }
    }

    public Cell GetCurrentCell()
    {
        return currentCell;
    }

    public void ToggleLight()
    {
        highlightCellLight.enabled = highlightCellLight.enabled == true ? false : true;
        gameObject.GetComponentInChildren<Light>().enabled = gameObject.GetComponentInChildren<Light>().enabled == true ? false : true;
        directionalLight.enabled = directionalLight.enabled == true ? false : true;
        lightSwitchedOn = lightSwitchedOn == true ? false : true;

        // to not render highlighted entity on switching off the light
        RenderHighlightedEntity(false);
    }

    /*public void LightOrDarkenSurroundingCells(bool light)
    {
        if(currentCell != null)
        {
            GameObject[] gameObjects = grid.GetNearbyGameObjectOfCell(currentCell);

            for(int i=0;i<gameObjects.Length;i++)
            {
                if (gameObjects[i] != null)
                {
                    if(light)
                    {
                        gameObjects[i].layer = LayerMask.NameToLayer(grid.lightedCell);
                    }
                    else
                    {
                        gameObjects[i].layer = LayerMask.NameToLayer(grid.unlightedCell);
                    }
                }
            }
        }

    } */
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
