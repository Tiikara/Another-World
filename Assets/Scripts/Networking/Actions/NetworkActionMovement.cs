using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Networking.Actions 
{
    [Serializable]
    class NetworkActionMovement : Action
    {
        int unitID;
        
        float posX;
        float posY;
        public NetworkActionMovement(Unit unit, Vector2 pos)
        {
            unitID = unit.Id;
            posX = pos.x;
            posY = pos.y;
        }

        public override void ProcessAction()
        {
            var unit = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitsController>().GetUnitById(unitID);
            unit.GetComponent<ActionController>().ClearQueue();
            unit.GetComponent<Movement>().Run(new Vector2(posX, posY));
        }
    }
}
