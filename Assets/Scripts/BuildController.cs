using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.Networking.Actions;

public class BuildController : MonoBehaviour {


    Queue<int> queueIdBuildUnits = new Queue<int>();
    
    public List<GameObject> ObjectsToBuild;
    BuildInfo buildInfo = null;
    int curIdBuild;
    double startTimeSecsBuilding = -1;

    UnitsController unitsController;
    ResourcesController resourcesController;

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
        resourcesController = GetComponent<ResourcesController>();
    }

    public GameObject GetObjectBuildById(int id)
    {
        return ObjectsToBuild[id];
    }


    public void OnClickBuildUnit(int idBuildUnit)
    {
        if (this.buildInfo != null || queueIdBuildUnits.Count != 0)
            return;

        BuildInfo buildInfo = ObjectsToBuild[idBuildUnit].GetComponent<BuildInfo>();

        if(resourcesController.isHave(buildInfo.Cost))
        {
            resourcesController.Reduce(buildInfo.Cost);
            queueIdBuildUnits.Enqueue(idBuildUnit);
            StartBuild();
        }
    }

    void StartBuild()
    {
        if (buildInfo == null)
        {
            if(queueIdBuildUnits.Count != 0)
            {
                curIdBuild = queueIdBuildUnits.Dequeue();
                buildInfo = ObjectsToBuild[curIdBuild].GetComponent<BuildInfo>();
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
                GameObject barrack = null;

                foreach (var _barrack in GameObject.FindGameObjectsWithTag("Barrack"))
                {
                    if(_barrack.GetComponent<Unit>().OwnerId == GetComponent<General>().ControlId)
                    {
                        barrack = _barrack;
                        break;
                    }
                }
                
                if(barrack != null)
                {
                    LockStepManager.Instance.AddAction(new NetworkActionCreateUnitWithMove(curIdBuild,
                        GetComponent<General>().ControlId, barrack.transform.position,
                        new Vector2(barrack.transform.position.x - 0.9f, barrack.transform.position.y - 0.9f)));
                    buildInfo = null;
                    StartBuild();
                }
                else
                {
                    resourcesController.Add(buildInfo.Cost);
                    buildInfo = null;
                    
                    // Clear build queue and return costs
                    foreach (var buildIdInfoQ in queueIdBuildUnits)
                    {
                        resourcesController.Add(ObjectsToBuild[buildIdInfoQ].GetComponent<BuildInfo>().Cost);
                    }

                    queueIdBuildUnits.Clear();
                    OnFinishBuild();
                }
                
                
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
	}
}
