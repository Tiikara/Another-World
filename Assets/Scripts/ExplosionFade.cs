using UnityEngine;
using System.Collections;
using System;

public class ExplosionFade : MonoBehaviour {

    DateTime timeEndAnimation;
    SpriteRenderer spriteRenderer;
    bool isEndedAnimation = false;
    float alpha = 1;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

        if(isEndedAnimation == false)
        {
            if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                isEndedAnimation = true;
                timeEndAnimation = DateTime.Now;
            }
        } else if(timeEndAnimation.AddSeconds(5) <= DateTime.Now)
        {
            float step = 0.5f * Time.deltaTime;
            alpha -= step;
            if (alpha < 0)
                alpha = 0;
            spriteRenderer.color = new Color(1,1,1,alpha);

            if (alpha == 0)
                Destroy(gameObject);
        }
	}
    
}
