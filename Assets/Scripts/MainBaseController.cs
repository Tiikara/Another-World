using UnityEngine;
using System.Collections;

public class MainBaseController : MonoBehaviour {

    Unit mainBase = null;

    public double RaduisBuilding;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	    if(mainBase == null)
            mainBase = GetComponent<UnitsController>().CreateUnit(
            Resources.Load<GameObject>("Units/MainBase"),
            new Vector2(3, 3), 0);
    }
}
