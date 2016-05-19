using UnityEngine;
using System.Collections;
using System;

public class Attack : IAction {

    class AttackParameters : IActionParameters
    {
        public Unit attackedUnit;
    }

    public float Damage;
    public float AttackPerSecond;
    public float AttackRadius;
    public GameObject Bullet;

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
        var unitPos = transform.position;
        var attackedUnitPos = unit.transform.position;

        float length = Mathf.Sqrt((unitPos.x - attackedUnitPos.x) * (unitPos.x - attackedUnitPos.x) + (unitPos.y - attackedUnitPos.y) * (unitPos.y - attackedUnitPos.y));

        if (AttackRadius > length)
        {
            var attackParams = new AttackParameters();
            attackParams.attackedUnit = unit;
            GetComponent<ActionController>().AddToQueue(this, attackParams);
            return;
        }

        var movement = GetComponent<Movement>();
        
        if(movement != null)
        {
            movement.RunToUnitRadius(unit, AttackRadius);

            var attackParams = new AttackParameters();
            attackParams.attackedUnit = unit;
            GetComponent<ActionController>().AddToQueue(this, attackParams);
        }
    }

    // Use this for initialization
    void Start () {
        timeLastAttack = DateTime.Now;

        attackCooldown = 1 / AttackPerSecond;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public override void Activate(IActionParameters actionParameters)
    {
        AttackParameters attackParameters = (AttackParameters)actionParameters;
        attackedUnit = attackParameters.attackedUnit;
    }

    public override void UpdateAction()
    {
        if (attackedUnit != null)
        {
            var attackUnitPos = transform.position;
            var attackedUnitPos = attackedUnit.transform.position;

            float length = Mathf.Sqrt((attackUnitPos.x - attackedUnitPos.x) * (attackUnitPos.x - attackedUnitPos.x) +
                (attackUnitPos.y - attackedUnitPos.y) * (attackUnitPos.y - attackedUnitPos.y));

            if (AttackRadius > length)
            {
                if (DateTime.Now >= timeLastAttack.AddSeconds(attackCooldown))
                {
                    var hitpoints = attackedUnit.GetComponent<HitPoints>();

                    if(Bullet != null)
                    {
                        var bullet = (Instantiate(Bullet, transform.position, new Quaternion()) as GameObject)
                            .GetComponent<IBullet>();
                        bullet.SetDamage(Damage);
                        bullet.SetTargetPosition(attackedUnitPos);
                    }
                    else
                    {
                        hitpoints.DealDamage(Damage);
                    }

                    timeLastAttack = DateTime.Now;

                    if (hitpoints.Health <= 0)
                        attackedUnit = null;
                }
            }
            else
            {
                GetComponent<ActionController>().ClearQueue();
                AttackUnit(attackedUnit);
                attackedUnit = null;
            }
        }
    }
}
