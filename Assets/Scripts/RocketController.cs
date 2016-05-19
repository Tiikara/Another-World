using UnityEngine;
using System.Collections;

public class RocketController : IBullet {

    Vector2 targetPos;
    float damage;

    public float RadiusExplosion;
    public float Speed;

    public override void SetTargetPosition(Vector2 position)
    {
        targetPos = position;
        float angle = Vector2.Angle(new Vector2(0, 1), 
            new Vector2(position.x - transform.position.x, position.y - transform.position.y));

        if (position.x - transform.position.x > 0)
            angle = -angle;

        transform.Rotate(0, 0, angle);
    }

    public override void SetDamage(float damage)
    {
        this.damage = damage;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        
        if((Vector2)transform.position == targetPos)
        {
            var unitsController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitsController>();

            foreach (var unit in unitsController.Units)
            {
                if(Vector2.Distance(unit.transform.position, transform.position) < RadiusExplosion)
                {
                    var hp = unit.GetComponent<HitPoints>();
                    if(hp != null)
                    {
                        hp.DealDamage(damage);
                    }
                }
            }

            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Explosion"), transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }
}
