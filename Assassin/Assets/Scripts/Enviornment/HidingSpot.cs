using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour 
{
	public BoxCollider2D collisionCollider =  null;

	Rigidbody2D rbBox;
	
	SpriteRenderer spriteRender;
	Sprite[] hidingSpotSprites;
	Sprite defaultSprite;

	GameObject player;

	bool playerInRange = false;
	bool inUse = false;


	// Use this for initialization
	void Awake () 
	{
		rbBox = GetComponent<Rigidbody2D>();

		spriteRender = GetComponent<SpriteRenderer>();
		hidingSpotSprites = Resources.LoadAll<Sprite>("Enviornment Assets/Crate");
		defaultSprite = hidingSpotSprites[0];

		player = GameObject.FindGameObjectWithTag ("Player");
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// if in range activate hiding spot
		// or if inUse, thereby meaning the player is inactive and needs to be activated again
		if (playerInRange || inUse)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				inUse = !inUse;
			}

            if (inUse)
            {
                spriteRender.sprite = hidingSpotSprites[1];

                //player.GetComponent<Player>().ResetWalking();
                player.SetActive(false);

            }
            else
            {
                spriteRender.sprite = defaultSprite;
                player.SetActive(true);

            }

        }

		

	}
	public void SetPlayerInRange(bool isPlayer)
	{
		playerInRange = isPlayer;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Wall"))
		{
			if (rbBox.gravityScale != 0)
			{
				rbBox.gravityScale = 0;
			}

			if (collisionCollider.enabled)
			{
				collisionCollider.enabled = false;
			}
		}
    }
}
