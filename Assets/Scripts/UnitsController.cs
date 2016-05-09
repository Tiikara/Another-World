using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsController : MonoBehaviour {

    List<Unit> units = new List<Unit>();

    public IEnumerable<Unit> Units
    {
        get
        {
            return units;
        }
    }

    public Unit CreateUnit(GameObject gameObject, Vector3 position)
    {
        GameObject obj = Instantiate(gameObject, position, new Quaternion()) as GameObject;

        Unit unit = obj.GetComponent<Unit>();

        units.Add(unit);

        return unit;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
