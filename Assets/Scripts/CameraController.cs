﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    bool pressedButton = false;
    Vector2 startWorldPosition;
    public float SpeedMovement;

    MapLoader mapLoader;

    // Use this for initialization
    void Start () {
         mapLoader = GameObject.Find("Controllers").GetComponent<MapLoader>();
	}

    void checkBorderPosition()
    {
        Vector3 screenMap = Camera.main.WorldToScreenPoint(new Vector3(mapLoader.MapSizeWidth, mapLoader.MapSizeHeight, Camera.main.transform.position.z));
        Vector3 startScreenMap = Camera.main.WorldToScreenPoint(new Vector3(0, 0, Camera.main.transform.position.z));

        // border map
        if (screenMap.x < Screen.width)
        {
            Vector3 newScreenPos = new Vector3(screenMap.x - Screen.width / 2.0f, Screen.height / 2.0f, 0);
            Camera.main.transform.position = Camera.main.ScreenToWorldPoint(newScreenPos);
        }

        if (screenMap.y < Screen.height)
        {
            Vector3 newScreenPos = new Vector3(Screen.width / 2.0f, screenMap.y - Screen.height / 2.0f, 0);
            Camera.main.transform.position = Camera.main.ScreenToWorldPoint(newScreenPos);
        }

        if (startScreenMap.x > 0)
        {
            Vector3 newScreenPos = new Vector3(Screen.width / 2.0f + startScreenMap.x, Screen.height / 2.0f, 0);
            Camera.main.transform.position = Camera.main.ScreenToWorldPoint(newScreenPos);
        }

        if (startScreenMap.y > 0)
        {
            Vector3 newScreenPos = new Vector3(Screen.width / 2.0f, startScreenMap.y + Screen.height / 2.0f, 0);
            Camera.main.transform.position = Camera.main.ScreenToWorldPoint(newScreenPos);
        }
    }

    public void SetPosition(Vector2 position)
    {
        Camera.main.transform.position = new Vector3( position.x, position.y, Camera.main.transform.position.z);

        checkBorderPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetButton("UpCamera"))
        {
            float step = SpeedMovement * Time.deltaTime;
            var curPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(curPos.x, curPos.y + step, curPos.z);
        }

        if (Input.GetButton("DownCamera"))
        {
            float step = SpeedMovement * Time.deltaTime;
            var curPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(curPos.x, curPos.y - step, curPos.z);
        }

        if (Input.GetButton("LeftCamera"))
        {
            float step = SpeedMovement * Time.deltaTime;
            var curPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(curPos.x - step, curPos.y, curPos.z);
        }

        if (Input.GetButton("RightCamera"))
        {
            float step = SpeedMovement * Time.deltaTime;
            var curPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(curPos.x + step, curPos.y + step, curPos.z);
        }

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
        
        checkBorderPosition();
    }
}
