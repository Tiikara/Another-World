using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            if(selectedUnits.Count != 0)
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

                            attack.GetComponent<ActionController>().ClearQueue();
                            attack.AttackUnit(unit);
                            return;
                        }
                        else if(selectedUnits[0].GetComponent<ResourceCollection>() != null && 
                            unit.GetComponent<BunchResource>() != null)
                        {
                            var bunchRes = unit.GetComponent<BunchResource>();
                            var resColl = selectedUnits[0].GetComponent<ResourceCollection>();

                            resColl.GetComponent<ActionController>().ClearQueue();
                            resColl.CollectResource(bunchRes);
                            return;
                        }
                        else
                        {
                            Movement movement = selectedUnits[0].GetComponent<Movement>();

                            if (movement != null)
                            {
                                movement.GetComponent<ActionController>().ClearQueue();
                                movement.RunToUnit(unit);
                                return;
                            }
                        }
                    }
                }

            foreach (var unit in selectedUnits)
            {
                if(controlId == unit.OwnerId)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    Movement movement = unit.GetComponent<Movement>();
                    if (movement != null)
                    {
                        movement.GetComponent<ActionController>().ClearQueue();
                        movement.Run(mousePos);
                    }
                }
            }
        }

        if(isInit == false)
        {
            GameObject barracks = Resources.Load<GameObject>("Units/Barracks");
            unitsController.CreateUnit(barracks, new Vector2(6, 6), 0);

            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Soldier"), new Vector2(9, 9), 1);
            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Soldier"), new Vector2(5, 9), 2);

            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Mineral"), new Vector2(2, 5), 2);

            unitsController.CreateUnit(Resources.Load<GameObject>("Units/Worker"), new Vector2(2, 4), 0);

            isInit = true;
        }
    }
}
