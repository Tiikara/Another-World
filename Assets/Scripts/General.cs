﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Networking.Actions;

public class General : MonoBehaviour {
    
    List<Unit> selectedUnits = new List<Unit>();
    List<GameObject> selectorCircles = new List<GameObject>();

    UnitsController unitsController;
    TeamInfo teamInfo;

    int controlId = 0;

    bool isInit = false;

    public int ControlId
    {
        get
        {
            return controlId;
        }
    }
        

    GameObject _circleSelector;

    // Use this for initialization
    void Start () {
        unitsController = GetComponent<UnitsController>();
        teamInfo = GetComponent<TeamInfo>();
        
        _circleSelector = Resources.Load<GameObject>("Prefabs/Circle");
        GameObject.Find("SceneManager").GetComponent<NetworkManager>().OnGameStart += OnGameStart;
    }

    void OnGameStart()
    {
        NetworkManager networkManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<NetworkManager>();
        
        controlId = networkManager.playersIds[Network.player.ToString()];
        
        int numberOfPlayers = networkManager.NumberOfPlayers;
        var players = networkManager.players;
        
        unitsController.CreateUnit(Resources.Load<GameObject>("Units/Barracks"), new Vector2(6, 6), 0);
        unitsController.CreateUnit(Resources.Load<GameObject>("Units/Worker"), new Vector2(2, 4), 0);
        unitsController.CreateUnit(Resources.Load<GameObject>("Units/MainBase"), new Vector2(3, 3), 0);

        if(numberOfPlayers >= 2)
        {
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Barracks"), new Vector2(18, 20.5f), 1);
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Worker"), new Vector2(22, 23.5f), 1);
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/MainBase"), new Vector2(21, 22.5f), 1);
        }
        
        if(numberOfPlayers >= 3)
        {
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Barracks"), new Vector2(2, 20.5f), 2);
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Worker"), new Vector2(6, 23.5f), 2);
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/MainBase"), new Vector2(5, 22.5f), 2);
        }

        if(numberOfPlayers >= 4)
        {
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Barracks"), new Vector2(18, 4.5f), 3);
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Worker"), new Vector2(22, 7.5f), 3);
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/MainBase"), new Vector2(21, 6.5f), 3);
        }

        BunchResource br = unitsController.CreateUnit(Resources.Load<GameObject>("Units/Mineral"), new Vector2(2, 5), 4)
            .GetComponent<BunchResource>();
        BunchResource br2 = unitsController.CreateUnit(Resources.Load<GameObject>("Units/Mineral"), new Vector2(23.5f, 23), 4)
            .GetComponent<BunchResource>();

        br.CountResources = 2100;
        br2.CountResources = 2100;
        br = unitsController.CreateUnit(Resources.Load<GameObject>("Units/Mineral"), new Vector2(12.25f, 12.25f), 4)
            .GetComponent<BunchResource>();
        br2 = unitsController.CreateUnit(Resources.Load<GameObject>("Units/Mineral"), new Vector2(2.5f, 22.5f), 4)
            .GetComponent<BunchResource>();
        BunchResource br3 = unitsController.CreateUnit(Resources.Load<GameObject>("Units/Mineral"), new Vector2(23, 1.5f), 4)
            .GetComponent<BunchResource>();

        br.CountResources = 99000;
        br2.CountResources = 2100;
        br3.CountResources = 2100;

        foreach(var objMainBase in GameObject.FindGameObjectsWithTag("MainBase"))
        {
            var unitMainBase = objMainBase.GetComponent<Unit>();
            if(unitMainBase.OwnerId == controlId)
            {
                Camera.main.GetComponent<CameraController>().SetPosition(unitMainBase.transform.position);
                break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var circle in selectorCircles)
                Destroy(circle);

            selectorCircles.Clear();
            selectedUnits.Clear();

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (var unit in unitsController.Units)
            {
                var unitPos = unit.transform.position;

                float length = Mathf.Sqrt((unitPos.x - mousePos.x) * (unitPos.x - mousePos.x) + (unitPos.y - mousePos.y) * (unitPos.y - mousePos.y));
                float radius = unit.GetComponent<Unit>().Radius;

                if(radius > length)
                {
                    selectedUnits.Add(unit);
                    var circleSelector = Instantiate(_circleSelector, new Vector2(unit.transform.position.x, unit.transform.position.y), new Quaternion()) as GameObject;
                    circleSelector.GetComponent<SpriteRenderer>().material.SetFloat("_Radius", radius);
                    circleSelector.transform.parent = unit.transform;
                    Color colorCircle = new Color();

                    if(controlId == unit.OwnerId)
                    {
                        colorCircle = Color.green;
                    }
                    else
                    switch(teamInfo.GetStatus(controlId, unit.OwnerId))
                    {
                        case TeamInfo.Status.War:
                            colorCircle = Color.red;
                            break;
                        case TeamInfo.Status.Neutral:
                            colorCircle = Color.yellow;
                            break;
                        case TeamInfo.Status.Ally:
                            colorCircle = Color.blue;
                            break;
                    }

                    circleSelector.GetComponent<SpriteRenderer>().material.SetColor("_Color", colorCircle);
                    selectorCircles.Add(circleSelector);
                    break;
                }
            }
        }
        else
        if(Input.GetMouseButtonDown(1))
        {
            if (selectedUnits.Count != 0)
            {
                if (selectedUnits[0].OwnerId != controlId)
                    return;

                foreach (var unit in unitsController.Units)
                {
                    var unitPos = unit.transform.position;
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    float lengthClicked = Mathf.Sqrt((unitPos.x - mousePos.x) * (unitPos.x - mousePos.x) + (unitPos.y - mousePos.y) * (unitPos.y - mousePos.y));
                    float radius = unit.GetComponent<Unit>().Radius;

                    if (radius > lengthClicked)
                    {
                        if (controlId != unit.OwnerId && teamInfo.GetStatus(controlId, unit.OwnerId) == TeamInfo.Status.War)
                        {
                            Attack attack = selectedUnits[0].GetComponent<Attack>();

                            if (attack == null)
                                break;

                            LockStepManager.Instance.AddAction(new NetworkActionAttack(selectedUnits[0], unit));
                            return;
                        }
                        else if (selectedUnits[0].GetComponent<ResourceCollection>() != null &&
                            unit.GetComponent<BunchResource>() != null)
                        {
                            var bunchRes = unit.GetComponent<BunchResource>();
                            var resColl = selectedUnits[0].GetComponent<ResourceCollection>();

                            LockStepManager.Instance.AddAction(new NetworkActionCollectResource(selectedUnits[0],
                                bunchRes));
                            return;
                        }
                        else
                        {
                            Movement movement = selectedUnits[0].GetComponent<Movement>();

                            if (movement != null)
                            {
                                LockStepManager.Instance.AddAction(new NetworkActionMovementToUnit(selectedUnits[0], unit));
                                return;
                            }
                        }
                    }
                }

                foreach (var unit in selectedUnits)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    Movement movement = unit.GetComponent<Movement>();
                    if (movement != null)
                    {
                        NetworkActionMovement na = new NetworkActionMovement(unit, mousePos);
                        LockStepManager.Instance.AddAction(na);
                        // movement.GetComponent<ActionController>().ClearQueue();
                        // movement.Run(mousePos);
                    }
                }
            }
        }
        
    }
}
