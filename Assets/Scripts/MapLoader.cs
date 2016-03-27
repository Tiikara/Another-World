using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour {

    enum TypeLand
    {
        Free, Rock, Tree
    }

    TypeLand[,] mapArray;

    // Use this for initialization
    void Start()
    {
        Texture2D texMap = Resources.Load("Maps/test", typeof(Texture2D)) as Texture2D;

        Color[] pix = texMap.GetPixels(0, 0, texMap.width, texMap.height);

        mapArray = new TypeLand[texMap.height, texMap.width];

        Dictionary<Color, TypeLand> types = new Dictionary<Color, TypeLand>();

        types[new Color(0, 0, 0)] = TypeLand.Rock;
        types[new Color(1, 1, 1)] = TypeLand.Free;
        types[new Color(0, 1, 0)] = TypeLand.Tree;

        for (int i = 0, orig_i = 0; i < texMap.height; i++)
            for (int j = 0; j < texMap.width; j++, orig_i++)
            {
                mapArray[i, j] = types[pix[orig_i]];
            }

        Sprite spriteLand = Resources.Load("Maps/Sprites/desert", typeof(Sprite)) as Sprite;
        Sprite spriteTree = Resources.Load("Maps/Sprites/tree", typeof(Sprite)) as Sprite;
        Sprite spriteRock = Resources.Load("Maps/Sprites/rock", typeof(Sprite)) as Sprite;

        GameObject landObject = new GameObject("Land");
        GameObject treeObject = new GameObject("tree");
        GameObject rockObject = new GameObject("rock");

        SpriteRenderer spriteRendererTree = treeObject.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRendererLand = landObject.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRendererRock = rockObject.AddComponent<SpriteRenderer>();

        spriteRendererTree.sprite = spriteTree;
        spriteRendererLand.sprite = spriteLand;
        spriteRendererRock.sprite = spriteRock;

        for (int i = 0, orig_i = 0; i < texMap.height; i++)
            for (int j = 0; j < texMap.width; j++, orig_i++)
            {
                GameObject gameObject= null;

                switch(mapArray[i, j])
                {
                    case TypeLand.Tree:
                        gameObject = treeObject;
                        break;
                    case TypeLand.Free:
                        gameObject = landObject;
                        break;
                    case TypeLand.Rock:
                        gameObject = rockObject;
                        break;
                }

                Instantiate(gameObject, new Vector2(j * 0.5f, i * 0.5f), new Quaternion());
            }

        Destroy(landObject);
        Destroy(treeObject);
        Destroy(rockObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
