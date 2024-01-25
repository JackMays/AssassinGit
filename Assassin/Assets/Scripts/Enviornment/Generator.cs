using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour 
{
	public Color noneCol;
	public Color BladeCol;
	public Color BulletCol;

    public List<Sprite> weaponSpritesList = new List<Sprite>();
    public List<Sprite> armourSpritesList = new List<Sprite>();

    public GameObject tilePrefab;

	GameObject canvas;

	public Vector2 basePos;

	public int xSize;
	public int ySize;
	public int floors;

	public float instSpacing;

	// Use this for initialization
	void Awake() 
	{
		canvas = GameObject.FindGameObjectWithTag("Canvas");

		Generate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Generate()
	{
		Vector2 newPos = basePos;
		int xLimit = xSize - 1;
		int yLimit = ySize - 1;

		for (int i = 0; i < floors; ++i) 
		{
            for (int y = 0; y < ySize; ++y)
            {
                for (int x = 0; x < xSize; ++x)
                {
                    if ((x == 0 || y == 0) || (x == xLimit || y == yLimit))
                    {
                        //Instantiate(tilePrefab, newPos, Quaternion.identity);

                        GameObject wallTile = Instantiate(tilePrefab, newPos, Quaternion.identity);

                        wallTile.transform.parent = canvas.transform;
                    }


                    newPos.x += instSpacing;
                }

                newPos.y -= instSpacing;
                newPos.x = basePos.x;
            }
        }

		
	}

	public Color GetNoneCol()
	{
		return noneCol;
	}

	public Color GetBladeCol()
	{
		return BladeCol;
	}

	public Color GetBulletCol()
	{
		return BulletCol;
	}

	public Sprite GetWeaponSprite(int id)
	{
		return weaponSpritesList[id];
	}

	public Sprite GetArmourSprite(int id)
	{
		return armourSpritesList[id];
	}
}
