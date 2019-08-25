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
    public float cameraTimeToMove;

	// Use this for initialization
	void Start () {
        horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        verticalExtent = Camera.main.orthographicSize;
        initialized = false;

        cameraCenter = transform.position;
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
                StartCoroutine(MoveCamera(gameObject, transform.position, new Vector2(currentPosition.x - cameraCenter.x, 0), cameraTimeToMove));
            }

            if(dispY > verticalExtent * moveIfBeyondPercentY)
            {
                StartCoroutine(MoveCamera(gameObject, transform.position, new Vector2(0, currentPosition.y - cameraCenter.y), cameraTimeToMove));
            }
        }
        else
        {
            initialized = true;
        }
        // if null this will set it to the first position
        lastPlayerPosition = currentPosition;
    }

    // convert this function to a move camera by function, to move the camera by the required amount
    IEnumerator MoveCamera(GameObject obj, Vector3 source, Vector3 amount, float timeToMove)
    {
        Vector3 destination = source + amount;
        Debug.Log(destination);
        float startTime = Time.time;

        while (Time.time - startTime < timeToMove)
        {
            obj.transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / timeToMove);
            yield return null;
        }

        transform.position = destination;
        cameraCenter = transform.position;
    }
}
