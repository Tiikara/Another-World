using UnityEngine;
using System.Collections;

public abstract class IBullet : MonoBehaviour {

    public abstract void SetDamage(float damage);
    public abstract void SetTargetPosition(Vector2 position);
    
}
