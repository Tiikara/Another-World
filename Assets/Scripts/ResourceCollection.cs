using UnityEngine;
using System.Collections;
using System;

public class ResourceCollection : IAction {

    class ResourceCollectionParameters : IActionParameters
    {
        public BunchResource bunchResource;
    }

    public float MaxInventory;
    public float SpeedCollectionInSeconds;

    float currCollected = 0;
    ResourceCollectionParameters resourceCollectionParams;

    bool endCollection;

    public float CurrentCollected
    {
        get
        {
            return currCollected;
        }
    }

    public void ReduceCollected(float value)
    {
        currCollected -= value;
        if (currCollected < 0)
            currCollected = 0;
    }

    public void CollectResource(BunchResource bunchResource)
    {
        if (bunchResource.CountResources == 0)
            return;

        var resParams = new ResourceCollectionParameters();
        resParams.bunchResource = bunchResource;

        var actionController = GetComponent<ActionController>();

        if (Vector2.Distance(transform.position, bunchResource.transform.position) < 0.3)
        {
            actionController.AddToQueue(this, resParams);
            return;
        }

        var movement = GetComponent<Movement>();

        if(movement != null)
        {
            movement.RunToUnitRadius(bunchResource.GetComponent<Unit>(), 0.3f);
            actionController.AddToQueue(this, resParams);

            var mainBase = GameObject.Find("MainBase(Clone)").GetComponent<Unit>();
            if(mainBase != null)
            {
                GetComponent<UnloadResources>().UnloadToMainBaseAndCollect(mainBase, bunchResource);
            }
        }
    }

    public override void Activate(IActionParameters actionParameters)
    {
        resourceCollectionParams = (ResourceCollectionParameters)actionParameters;
        endCollection = false;
    }

    public override bool IsEndedAction()
    {
        return endCollection;
    }

    public override void UpdateAction()
    {
        if (Vector2.Distance(transform.position, resourceCollectionParams.bunchResource.transform.position) < 0.3)
        {
            float step = SpeedCollectionInSeconds * Time.deltaTime;

            if (step + currCollected > MaxInventory)
            {
                step = MaxInventory - currCollected;
                endCollection = true;
            }

            if(resourceCollectionParams.bunchResource.CountResources - step < 0)
            {
                step = resourceCollectionParams.bunchResource.CountResources;
                endCollection = true;
            }

            resourceCollectionParams.bunchResource.CountResources -= step;
            currCollected += step;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
