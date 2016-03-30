using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    bool pressedButton = false;
    Vector2 startWorldPosition;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            startWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pressedButton = true;
        }
        else if(Input.GetMouseButtonUp(2))
        {
            pressedButton = false;
        }
        else if(pressedButton)
        {
            Vector2 curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(curMousePos != startWorldPosition)
            {
                Vector2 deltaPos = curMousePos - startWorldPosition;

                Vector2 newPos = (Vector2)transform.position - deltaPos;

                transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            }

            
        }
    }
}
