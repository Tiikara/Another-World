using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Networking.Actions 
{
    [Serializable]
    class NetworkActionCreateUnitWithMove : Action
    {
        int idBuildUnit;
        int ownerId;

        float posXcreate;
        float posYcreate;

        float posXmoveTo;
        float posYmoveTo;
        public NetworkActionCreateUnitWithMove(int _idBuildUnit, int _ownerId, Vector2 posCreate, Vector2 posMoveTo)
        {
            idBuildUnit = _idBuildUnit;
            ownerId = _ownerId;

            posXcreate = posCreate.x;
            posYcreate = posCreate.y;
            posXmoveTo = posMoveTo.x;
            posYmoveTo = posMoveTo.y;
        }

        public override void ProcessAction()
        {
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            GameObject unitObj = gameController.GetComponent<BuildController>()
                .GetObjectBuildById(idBuildUnit);
            var unit = gameController.GetComponent<UnitsController>().CreateUnit(unitObj.gameObject, 
                new Vector2(posXcreate, posYcreate), ownerId);
            unit.GetComponent<ActionController>().ClearQueue();
            unit.GetComponent<Movement>().Run(new Vector2(posXmoveTo, posYmoveTo));
        }
    }
}
