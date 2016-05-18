using UnityEngine;
using System.Collections;

public class ResourcesController : MonoBehaviour {

    public float MaxResources;

    float resources;
    UnityEngine.UI.Text text;

	// Use this for initialization
	void Start () {
        text = GameObject.Find("TextResourcesCount").GetComponent<UnityEngine.UI.Text>();

        resources = 200;
        text.text = ((int)resources).ToString();
    }

    public float Add(float value)
    {
        float added = value;
        float oldRes = resources;
        resources += value;
        if (resources > MaxResources)
        {
            resources = MaxResources;
            added = resources - oldRes;
        }

        text.text = ((int)resources).ToString();

        return added;
    }

    public void Reduce(float value)
    {
        resources -= value;
        if (resources < 0)
            resources = 0;
        text.text = ((int)resources).ToString();
    }

    public bool isHave(float value)
    {
        return resources >= value;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
