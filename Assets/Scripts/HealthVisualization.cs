using UnityEngine;
using System.Collections;

public class HealthVisualization : MonoBehaviour {

    GameObject healthBar;
    GameObject healthBarBorder;

    bool isVisible = false;

	// Use this for initialization
	void Start () {
        healthBar = Resources.Load<GameObject>("Prefabs/HealthBar");
        healthBar = Instantiate(healthBar, new Vector2(0, 0), new Quaternion()) as GameObject;
        healthBar.GetComponent<SpriteRenderer>().color = Color.green;
        healthBar.transform.parent = transform;
        healthBar.transform.localPosition = new Vector2(0, 1);

        healthBarBorder = Instantiate(healthBar, new Vector2(0, 0), new Quaternion()) as GameObject;
        healthBarBorder.GetComponent<SpriteRenderer>().color = new Color(0.039f, 0.478f, 0.023f);
        healthBarBorder.transform.parent = transform;
        healthBarBorder.transform.localPosition = new Vector2(0, 1);

        healthBar.GetComponent<SpriteRenderer>().enabled = false;
        healthBarBorder.GetComponent<SpriteRenderer>().enabled = false;
    }
	
    public void SetValue(float value)
    {
        if(isVisible == false && value != 1)
        {
            healthBar.GetComponent<SpriteRenderer>().enabled = true;
            healthBarBorder.GetComponent<SpriteRenderer>().enabled = true;
        }
        else if(isVisible == true && value == 1)
        {
            healthBar.GetComponent<SpriteRenderer>().enabled = false;
            healthBarBorder.GetComponent<SpriteRenderer>().enabled = false;
        }

        healthBar.transform.localScale = new Vector3(value * 90, 1);
        healthBar.transform.localPosition = new Vector3(-(1-value) * 90.0f * 0.005f, 1);
    }
}
