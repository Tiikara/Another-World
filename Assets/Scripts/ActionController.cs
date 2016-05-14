using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IActionParameters
{

}

public class ActionInfo
{
    public ActionInfo(IAction _action, IActionParameters _actionParams)
    {
        Action = _action;
        ActionParameters = _actionParams;
    }

    public IAction Action;
    public IActionParameters ActionParameters;
}

public class ActionController : MonoBehaviour {

    Queue<ActionInfo> queueActions = new Queue<ActionInfo>();
    IAction activeAction = null;

    public void AddToQueue(IAction action, IActionParameters actionParams)
    {
        queueActions.Enqueue(new ActionInfo(action, actionParams));
    }

    public void AddMainToQueue(IAction action, IActionParameters actionParams)
    {
        activeAction = null;
        queueActions.Clear();
        queueActions.Enqueue(new ActionInfo(action, actionParams));
    }

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
	    if(activeAction == null)
        {
            if(queueActions.Count != 0)
            {
                var actionInfo = queueActions.Dequeue();
                activeAction = actionInfo.Action;
                activeAction.Activate(actionInfo.ActionParameters);
            }
        }
        else
        {
            activeAction.UpdateAction();

            if(activeAction.IsEndedAction())
            {
                activeAction = null;
            }
        }
	}
}
