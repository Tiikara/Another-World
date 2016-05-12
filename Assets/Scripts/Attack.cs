using UnityEngine;
using System.Collections;
using System;

public class Attack : IAction {

    public float Damage;
    public float AttackPerSecond;
    public float AttackRadius;

    float attackCooldown;

    Unit attackedUnit = null;

    public override bool IsEndedAction()
    {
        if(attackedUnit.GetComponent<HitPoints>().Health <= 0)
        {
            return true;
        }

        return false;
    }

    void AttackUnit(Unit unit)
    {
        attackedUnit = unit;
    }

    // Use this for initialization
    void Start () {
        attackCooldown = 1 / attackCooldown;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
