using UnityEngine;
using System.Collections;

public class HitPoints : MonoBehaviour {

    public float Health;

    float maxHealth;

    HealthVisualization healthVisualization;

    public void DealDamage(float damage)
    {
        Health -= damage;
        healthVisualization.SetValue(Health / maxHealth);

        if (Health < 0)
        {
            Health = 0;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitsController>().KillUnit(GetComponent<Unit>());
        }
    }

	// Use this for initialization
	void Start () {
        maxHealth = Health;

        healthVisualization = gameObject.AddComponent<HealthVisualization>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
