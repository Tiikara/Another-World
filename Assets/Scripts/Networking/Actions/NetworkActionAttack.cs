using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Networking.Actions 
{
    [Serializable]
    class NetworkActionAttack : Action
    {
        int unitID;
        int unitIDAttack;

        public NetworkActionAttack(Unit unit, Unit unitAttack)
        {
            unitID = unit.Id;
            unitIDAttack = unitAttack.Id;
        }

        public override void ProcessAction()
        {
            var unit = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitsController>().GetUnitById(unitID);
            var unitAttack = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitsController>().GetUnitById(unitIDAttack);
            unit.GetComponent<ActionController>().ClearQueue();
            unit.GetComponent<Attack>().AttackUnit(unitAttack);
        }
    }
}
