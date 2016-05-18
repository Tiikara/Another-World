using UnityEngine;
using System.Collections;

public class HealthVisualization : MonoBehaviour {

    GameObject healthBar;
    GameObject healthBarBorder;

    bool isVisible = false;

	// Use this for initialization
	void Start () {
        var bounds = GetComponent<SpriteRenderer>().bounds;
        var localTopRight = new Vector3(bounds.extents.x, bounds.extents.y, 0.0f) + transform.position;
        localTopRight = transform.InverseTransformPoint(localTopRight);

        healthBar = Resources.Load<GameObject>("Prefabs/HealthBar");
        healthBar = Instantiate(healthBar, new Vector2(0, 0), new Quaternion()) as GameObject;
        healthBar.GetComponent<SpriteRenderer>().color = Color.green;
        healthBar.transform.parent = transform;
        healthBar.transform.localPosition = new Vector2(0, localTopRight.y + 0.3f);
        healthBar.transform.localScale = new Vector2(90, 5);

        healthBarBorder = Instantiate(healthBar, new Vector2(0, 0), new Quaternion()) as GameObject;
        healthBarBorder.GetComponent<SpriteRenderer>().color = new Color(0.039f, 0.478f, 0.023f);
        healthBarBorder.transform.parent = transform;
        healthBarBorder.transform.localPosition = new Vector2(0, localTopRight.y + 0.3f);
        healthBarBorder.transform.localScale = new Vector2(90, 5);

        healthBar.GetComponent<SpriteRenderer>().sortingOrder = 1;
        healthBarBorder.GetComponent<SpriteRenderer>().sortingOrder = 0;

         healthBar.GetComponent<SpriteRenderer>().enabled = false;
         healthBarBorder.GetComponent<SpriteRenderer>().enabled = false;
    }
	
    public void SetValue(float value)
    {
        if(isVisible == false && value != 1)
        {
            healthBar.GetComponent<SpriteRenderer>().enabled = true;
            healthBarBorder.GetComponent<SpriteRenderer>().enabled = true;

            isVisible = true;
        }
        else if(isVisible == true && value == 1)
        {
            healthBar.GetComponent<SpriteRenderer>().enabled = false;
            healthBarBorder.GetComponent<SpriteRenderer>().enabled = false;

            isVisible = false;
        }

        healthBar.transform.localScale = new Vector3(value * 90, healthBar.transform.localScale.y);
        healthBar.transform.localPosition = new Vector3(-(1-value) * 90.0f * 0.005f, healthBar.transform.localPosition.y);
    }
}
