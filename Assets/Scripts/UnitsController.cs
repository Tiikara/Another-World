using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsController : MonoBehaviour {

    General general;
    TeamInfo teamInfo;

    List<Unit> units = new List<Unit>();

    public IEnumerable<Unit> Units
    {
        get
        {
            return units;
        }
    }

    public Unit GetUnitById(int id)
    {
        foreach(Unit unit in units)
        {
            if (unit.Id == id)
                return unit;
        }

        return null;
    }

    public void KillUnit(Unit unit)
    {
        Instantiate(unit.OnDestroyObject, unit.transform.position, new Quaternion());
        units.Remove(unit);
        Destroy(unit.gameObject);
    }

    public Unit CreateUnit(GameObject gameObject, Vector3 position, int ownerId)
    {
        GameObject obj = Instantiate(gameObject, position, new Quaternion()) as GameObject;

        Unit unit = obj.GetComponent<Unit>();

        unit.OwnerId = ownerId;
        
        Color selectionColor = Color.black;

        if(general.ControlId == ownerId)
        {
            selectionColor = Color.green;
        }
        else
        switch(teamInfo.GetStatus(general.ControlId, ownerId))
        {
            case TeamInfo.Status.War:
                selectionColor = Color.red;
                break;
            case TeamInfo.Status.Ally:
                selectionColor = Color.blue;
                break;
            case TeamInfo.Status.Neutral:
                selectionColor = Color.yellow;
                break;
        }
        unit.SetSelectionColor(selectionColor);

        units.Add(unit);

        return unit;
    }

    // Use this for initialization
    void Start () {
        general = GetComponent<General>();
        teamInfo = GetComponent<TeamInfo>();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
