using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Vector3 lastPlayerPosition;
    public bool initialized;

    // move camera if beyond this much percent of the screen width/height from the center
    public float moveIfBeyondPercentX;
    public float moveIfBeyondPercentY;

    // edges of the camera map in world coordinates
    public float horizontalExtent;
    public float verticalExtent;

    public Vector2 cameraCenter;

	// Use this for initialization
	void Start () {
        horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        verticalExtent = Camera.main.orthographicSize;
        initialized = false;

        cameraCenter = transform.position;
}

    public void SetInitialCellPosition(Vector3 initialPos)
    {
        lastPlayerPosition = initialPos;
        initialized = true;
    }

	// is called whenever the player is moved
	public void PlayerMoved(Vector2 currentPosition)
    {
        if(initialized)
        {
            float dispX = Mathf.Abs(currentPosition.x - cameraCenter.x);
            float dispY = Mathf.Abs(currentPosition.y - cameraCenter.y);

            if(dispX > horizontalExtent * moveIfBeyondPercentX)
            {
                // do stuff
            }

            if(dispY > verticalExtent * moveIfBeyondPercentY)
            {
                // do stuff
            }
        }
    }
}
