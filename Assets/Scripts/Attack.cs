using UnityEngine;
using System.Collections;
using System;

public class Attack : IAction {

    public float Damage;
    public float AttackPerSecond;
    public float AttackRadius;

    float attackCooldown;

    DateTime timeLastAttack;
    Unit attackedUnit = null;

    public override bool IsEndedAction()
    {
        if(attackedUnit == null)
        {
            return true;
        }

        return false;
    }

    public void AttackUnit(Unit unit)
    {
        attackedUnit = unit;
    }

    // Use this for initialization
    void Start () {
        timeLastAttack = DateTime.Now;

        attackCooldown = 1 / AttackPerSecond;
	}
	
	// Update is called once per frame
	void Update () {
	    if(attackedUnit != null)
        {
            var attackUnitPos = transform.position;
            var attackedUnitPos = attackedUnit.transform.position;

            float length = Mathf.Sqrt((attackUnitPos.x - attackedUnitPos.x) * (attackUnitPos.x - attackedUnitPos.x) + 
                (attackUnitPos.y - attackedUnitPos.y) * (attackUnitPos.y - attackedUnitPos.y));

            if(AttackRadius > length)
            {
                if(DateTime.Now >= timeLastAttack.AddSeconds(attackCooldown))
                {
                    var hitpoints = attackedUnit.GetComponent<HitPoints>();

                    hitpoints.DealDamage(Damage);

                    timeLastAttack = DateTime.Now;

                    if (hitpoints.Health <= 0)
                        attackedUnit = null;
                }
            }
            else
            {
                attackedUnit = null;
            }
        }
	}
}
