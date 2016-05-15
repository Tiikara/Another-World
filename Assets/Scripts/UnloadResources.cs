using UnityEngine;
using System.Collections;
using System;

public class UnloadResources : IAction
{
    class UnloadResourcesParameters : IActionParameters
    {
        public Unit mainBase;
        public BunchResource bunchResource = null;
    }

    UnloadResourcesParameters unloadParams;
    ResourceCollection resourceCollection;
    ResourcesController resourcesController;

    float radiusUnload = 0.8f;

    bool unloaded;

    public void UnloadToMainBase(Unit mainBase)
    {
        var unParams = new UnloadResourcesParameters();
        unParams.mainBase = mainBase;
        GetComponent<ActionController>().AddToQueue(this, unParams);
    }

    public void UnloadToMainBaseAndCollect(Unit mainBase, BunchResource bunchResource)
    {
        var unParams = new UnloadResourcesParameters();
        unParams.mainBase = mainBase;
        unParams.bunchResource = bunchResource;
        GetComponent<ActionController>().AddToQueue(this, unParams);
    }

    

    // Use this for initialization
    void Start () {
        resourceCollection = GetComponent<ResourceCollection>();
        resourcesController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ResourcesController>();
    }

    public override bool IsEndedAction()
    {
        return unloaded;
    }

    public override void Activate(IActionParameters actionParameters)
    {
        unloadParams = (UnloadResourcesParameters)actionParameters;
        unloaded = false;
    }

    public override void UpdateAction()
    {
        if (resourceCollection.CurrentCollected == 0)
        {
            unloaded = true;
            return;
        }

        if (Vector2.Distance(unloadParams.mainBase.transform.position,
                            transform.position) > radiusUnload)
        {
            var actionController = GetComponent<ActionController>();
            actionController.ClearQueue();
            GetComponent<Movement>().RunToUnitRadius(unloadParams.mainBase, radiusUnload);
            actionController.AddToQueue(this, unloadParams);
        }
        else
        {
            float added = resourcesController.Add(resourceCollection.CurrentCollected);
            resourceCollection.ReduceCollected(added);

            if(unloadParams.bunchResource != null)
            {
                var actionController = GetComponent<ActionController>();
                actionController.ClearQueue();
                resourceCollection.CollectResource(unloadParams.bunchResource);
            }

            unloaded = true;
        }
    }
}
