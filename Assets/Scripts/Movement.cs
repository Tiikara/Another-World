using UnityEngine;
using System.Collections;

public class Movement : IAction
{

    public float Speed;

    bool active = false;
    Vector2 targetPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(active)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if((Vector2)transform.position == targetPosition)
            {
                active = false;
            }
        }
	}

    public void Run(Vector2 target)
    {
        targetPosition = target;
        active = true;
    }

    public override bool IsEndedAction()
    {
        return !active;
    }
}
