using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    public string Name;
    public int OwnerId;
    public float Radius;

    Material materialSelection = null;
    Color selectionColor = Color.red;

    public void SetSelectionColor(Color color)
    {
        selectionColor = color;
        if(materialSelection != null)
            materialSelection.SetColor("_Color", selectionColor);
    }

    void Start() {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = Resources.Load("Materials/OutlineSlection", typeof(Material)) as Material;
        materialSelection = spriteRenderer.material;
        materialSelection.SetColor("_Color", selectionColor);
    }

    void OnMouseEnter()
    {
        materialSelection.SetFloat("_OutLineSpreadX", 0.015f);
        materialSelection.SetFloat("_OutLineSpreadY", 0.015f);
    }

    void OnMouseExit()
    {
        materialSelection.SetFloat("_OutLineSpreadX", 0);
        materialSelection.SetFloat("_OutLineSpreadY", 0);
    }
}
