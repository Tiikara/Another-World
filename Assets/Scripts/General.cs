using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class General : MonoBehaviour {
    
    List<Unit> selectedUnits = new List<Unit>();
    List<GameObject> selectorCircles = new List<GameObject>();

    UnitsController unitsController;

    GameObject _circleSelector;

    // Use this for initialization
    void Start () {
        unitsController = GetComponent<UnitsController>();
        
        GameObject barracks = Resources.Load<GameObject>("Units/Barracks");
        unitsController.CreateUnit(barracks, new Vector2(6, 6));

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
                    //circleSelector.GetComponent<SpriteRenderer>().material.SetFloat("_Radius", radius);
                    circleSelector.transform.parent = unit.transform;
                    selectorCircles.Add(circleSelector);
                    break;
                }
            }
        }
        else
        if(Input.GetMouseButtonDown(1))
        {
            foreach(var unit in selectedUnits)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Movement movement = unit.GetComponent<Movement>();
                if(movement != null)
                    movement.Run(mousePos);
            }
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("down");
        //Application.LoadLevel("SomeLevel");
    }
}
