using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class General : MonoBehaviour {

    List<GameObject> units = new List<GameObject>();
    List<GameObject> selectedUnits = new List<GameObject>();
    // Use this for initialization
    void Start () {
        GameObject soldier = Resources.Load<GameObject>("Units/Soldier");
        
        for(int i=0;i<3;i++)
        {
            soldier = Instantiate(soldier, new Vector2(i*2, 0), new Quaternion()) as GameObject;

            units.Add(soldier);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            selectedUnits.Clear();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (var unit in units)
            {
                var unitPos = unit.transform.position;

                float length = Mathf.Sqrt((unitPos.x - mousePos.x) * (unitPos.x - mousePos.x) + (unitPos.y - mousePos.y) * (unitPos.y - mousePos.y));

                if(unit.GetComponent<Unit>().Radius > length)
                {
                    selectedUnits.Add(unit);
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

                unit.transform.position = mousePos;
            }
        }
    }
}
