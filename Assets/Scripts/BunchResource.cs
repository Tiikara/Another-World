using UnityEngine;
using System.Collections;

public class BunchResource : MonoBehaviour {

    public float CountResources;

    float maxRes;

    void Start()
    {
        maxRes = CountResources;
    }
    void Update()
    {
        if(CountResources / maxRes < 0.25 && transform.localScale.x > 0.5f)
        {
            transform.localScale = new Vector2(0.5f,0.5f);
        }

        if(CountResources == 0 && transform.localScale.x > 0.2f)
        {
            transform.localScale = new Vector2(0.2f, 0.2f);
        }

    }
    
}
