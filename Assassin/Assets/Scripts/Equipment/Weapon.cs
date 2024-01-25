using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Equipment {

    public List<Vector2> weaponPositions = new List<Vector2>();

    // Use this for initialization
    void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    protected override void UpdateSprite()
    {
        /*if (colGroup == colourGroup.None)
        {
            spriteRender.sprite = generatorRef.GetWeaponSprite((int)colourGroup.None);

        }
        else if (colGroup == colourGroup.RedBlade)
        {
            spriteRender.sprite = generatorRef.GetWeaponSprite((int)colourGroup.RedBlade);

        }
        else if (colGroup == colourGroup.BlueBullets)
        {
            spriteRender.sprite = generatorRef.GetWeaponSprite((int)colourGroup.BlueBullets);

        }*/

        spriteRender.sprite = generatorRef.GetWeaponSprite((int)colGroup);

        transform.localPosition = weaponPositions[(int)colGroup];

        base.UpdateSprite();
    }
}
