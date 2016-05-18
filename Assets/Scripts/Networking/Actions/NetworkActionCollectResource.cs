using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Networking.Actions 
{
    [Serializable]
    class NetworkActionCollectResource : Action
    {
        int unitID;
        int resourceId;

        public NetworkActionCollectResource(Unit unit, BunchResource bunchResource)
        {
            unitID = unit.Id;
            resourceId = bunchResource.GetComponent<Unit>().Id;
        }

        public override void ProcessAction()
        {
            var unit = GameObject.FindGameObjectWithTag("GameController").
                GetComponent<UnitsController>().GetUnitById(unitID);
            var bunchResource = GameObject.FindGameObjectWithTag("GameController").
                GetComponent<UnitsController>().GetUnitById(resourceId).
                GetComponent<BunchResource>();
            unit.GetComponent<ActionController>().ClearQueue();
            unit.GetComponent<ResourceCollection>().CollectResource(bunchResource);
        }
    }
}
