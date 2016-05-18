using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Networking.Actions 
{
    [Serializable]
    class NetworkActionMovementToUnit : Action
    {
        int unitID;
        int unitIDMove;

        public NetworkActionMovementToUnit(Unit unit, Unit unitMove)
        {
            unitID = unit.Id;
            unitIDMove = unitMove.Id;
        }

        public override void ProcessAction()
        {
            var unit = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitsController>().GetUnitById(unitID);
            var unitMove = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitsController>().GetUnitById(unitIDMove);
            unit.GetComponent<ActionController>().ClearQueue();
            unit.GetComponent<Movement>().RunToUnit(unitMove);
        }
    }
}
