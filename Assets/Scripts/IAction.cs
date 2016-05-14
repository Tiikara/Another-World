using UnityEngine;
using System.Collections;

public abstract class IAction : MonoBehaviour {
    public abstract bool IsEndedAction();
    public abstract void Activate(IActionParameters actionParameters);

    public abstract void UpdateAction();
}
