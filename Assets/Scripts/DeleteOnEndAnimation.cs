using UnityEngine;
using System.Collections;

public class DeleteOnEndAnimation : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95)
        {
            Destroy(gameObject);
        }
    }
}
