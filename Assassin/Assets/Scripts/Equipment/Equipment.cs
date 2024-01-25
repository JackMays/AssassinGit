using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment : MonoBehaviour {

	public enum colourGroup
	{
		None,
		Blade,
		Bullets,
	}

	public colourGroup colGroup;
	

	protected Generator generatorRef;



	// every equipment has a sprite
	protected SpriteRenderer spriteRender;


	// Use this for initialization
	void Awake () 
	{
		spriteRender = gameObject.GetComponent<SpriteRenderer>();
		generatorRef = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();

		UpdateSprite();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	protected virtual void UpdateSprite()
	{
		if (colGroup == colourGroup.None)
		{
			spriteRender.color = generatorRef.GetNoneCol();
			
			
		}
		else if (colGroup == colourGroup.Blade)
		{
			spriteRender.color = generatorRef.GetBladeCol();
            
        }
		else if (colGroup == colourGroup.Bullets)
		{
			spriteRender.color = generatorRef.GetBulletCol();
            
        }

        
    }

	public void SetColour(colourGroup passedColour)
	{
		colGroup = passedColour;
		UpdateSprite();
	}

	public colourGroup GetColour()
	{
		return colGroup;
	}
}
