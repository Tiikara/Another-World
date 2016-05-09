﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildController : MonoBehaviour {

    Queue<BuildInfo> queueBuildUnits = new Queue<BuildInfo>();
    BuildInfo buildInfo = null;
    double startTimeSecsBuilding = -1;

    GameObject barracks;

    UnitsController unitsController;

    DateTime timeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public delegate void MethodStartBuild(string name);
    public delegate void MethodUpdateProgress(double progress);
    public delegate void MethodFinishBuild();
    public event MethodStartBuild OnStartBuild;
    public event MethodUpdateProgress OnUpdateProgressBuild;
    public event MethodFinishBuild OnFinishBuild;

    double lastProgressValue;

    void Start()
    {
        unitsController = GetComponent<UnitsController>();
        
    }

    public void OnClickBuildUnit(GameObject unit)
    {
        queueBuildUnits.Enqueue(unit.GetComponent<BuildInfo>());
        
        StartBuild();
    }

    void StartBuild()
    {
        if (buildInfo == null)
        {
            if(queueBuildUnits.Count != 0)
            {
                buildInfo = queueBuildUnits.Dequeue();
                startTimeSecsBuilding = DateTime.Now.Subtract(timeUtc).TotalSeconds;
                lastProgressValue = 0;
                OnStartBuild(buildInfo.GetComponent<Unit>().Name);
            }
            else
            {
                OnFinishBuild();
            }
            
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if(buildInfo != null)
        {
            double nowSeconds = DateTime.Now.Subtract(timeUtc).TotalSeconds;

            if(startTimeSecsBuilding + buildInfo.TimeBuildSec < nowSeconds)
            {
                var unit = unitsController.CreateUnit(buildInfo.gameObject, barracks.transform.position);
                unit.GetComponent<Movement>().Run(new Vector2(barracks.transform.position.x - 0.5f, barracks.transform.position.y - 0.5f));
                buildInfo = null;
                StartBuild();
            }
            else
            {
                double progress = (nowSeconds - startTimeSecsBuilding) / buildInfo.TimeBuildSec;
                progress = Math.Round(progress, 2);
                if (progress != lastProgressValue)
                {
                    lastProgressValue = progress;
                    OnUpdateProgressBuild(progress);
                }
            }
        }

        if(barracks == null)
        {
            barracks = GameObject.Find("Barracks(Clone)");
        }
	}
}