﻿using UnityEngine;
using System.Collections;
using System;

public class Movement : IAction
{
    class MovementParameters : IActionParameters
    {
        public Unit moveToUnit = null;
        public Unit moveToUnitCheckRadius = null;
        public float radius;
        public Vector2 ?targetPosition = null;
    }

    public float Speed;

    MovementParameters moveParams;
    bool moveEnd = false;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void RunToUnitRadius(Unit unit, float radius)
    {
        MovementParameters movementParams = new MovementParameters();
        movementParams.moveToUnitCheckRadius = unit;
        movementParams.radius = radius;
        GetComponent<ActionController>().AddToQueue(this, movementParams);
    }

    public void RunToUnit(Unit unit)
    {
        MovementParameters movementParams = new MovementParameters();
        movementParams.moveToUnit = unit;
        GetComponent<ActionController>().AddToQueue(this, movementParams);
    }

    public void Run(Vector2 target)
    {
        MovementParameters movementParams = new MovementParameters();
        movementParams.targetPosition = target;
        GetComponent<ActionController>().AddToQueue(this, movementParams);
    }

    public override bool IsEndedAction()
    {
        return moveEnd;
    }

    public override void Activate(IActionParameters actionParameters)
    {
        moveEnd = false;
        moveParams = (MovementParameters)actionParameters;
    }

    public override void UpdateAction()
    {
        Vector2 targetPos = Vector2.down;
        if(moveParams.moveToUnit != null)
        {
            targetPos = moveParams.moveToUnit.transform.position;
        }
        else if(moveParams.targetPosition != null)
        {
            targetPos = (Vector2)moveParams.targetPosition;
        }
        else if (moveParams.moveToUnitCheckRadius != null)
        {
            targetPos = moveParams.moveToUnitCheckRadius.transform.position;
        }

        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        // Checking conditions for end
        if ((Vector2)transform.position == targetPos)
        {
            moveEnd = true;
        }
        else if (moveParams.moveToUnitCheckRadius != null)
        {
            var attackUnitPos = transform.position;
            var attackedUnitPos = moveParams.moveToUnitCheckRadius.transform.position;

            float length = Mathf.Sqrt((attackUnitPos.x - attackedUnitPos.x) * (attackUnitPos.x - attackedUnitPos.x) +
                (attackUnitPos.y - attackedUnitPos.y) * (attackUnitPos.y - attackedUnitPos.y));

            if (moveParams.radius > length)
                moveEnd = true;
        }
    }
}
