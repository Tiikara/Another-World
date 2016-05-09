using UnityEngine;
using System.Collections;

public class MainBaseController : MonoBehaviour {

    Unit mainBase;

    public double RaduisBuilding;

	// Use this for initialization
	void Start () {
        mainBase = GetComponent<UnitsController>().CreateUnit(
            Resources.Load<GameObject>("Units/MainBase"), 
            new Vector2(3, 3));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
