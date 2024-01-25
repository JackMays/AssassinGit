using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float rotateValue = 0.0f;
	public float moveSpeed = 0.0f;


	Rigidbody2D player;

	Weapon currentWeapon;

	Animator animator;

	int movingID;
	
	int doorTravelId = 0;


	bool rotate = false;
	bool isAttack = false;
	bool isDoorTravel = false;

	// Use this for initialization
	void Awake () 
	{

		player = gameObject.GetComponent<Rigidbody2D>();

		currentWeapon = GetComponentInChildren<Weapon>();

		animator = gameObject.GetComponent<Animator>();

		movingID = Animator.StringToHash("Moving");
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if (player.velocity != Vector2.zero)
		{
			animator.SetBool(movingID, true);
		}
		else
		{
			animator.SetBool(movingID, false);
		}*/

		/*if (player.velocity.magnitude > 0 && player.velocity.magnitude > 1.0f)
		{
			animator.SetBool(movingID, true);
		}
		else
		{
			animator.SetBool(movingID, false);
		}*/
		if (rotate)
		{
			transform.eulerAngles = new Vector3(0.0f, rotateValue, 0.0f);
		}
		else
		{
			transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
		}

		isAttack = Input.GetKeyDown(KeyCode.E);
		if (isAttack)
		{
			Debug.Log(isAttack);
		}
		


    }

	void FixedUpdate()
	{
		//move when WASD or arrow keys affect horizontally axis
		player.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0.0f));
        //player.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0.0f);

        if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal") > 0)
		{
			animator.SetBool(movingID, true);
		}
		else
		{
			animator.SetBool(movingID, false);
		}

		if (Input.GetAxis("Horizontal") < 0)
		{
			rotate = true;
		}
		else if (Input.GetAxis("Horizontal") > 0)
		{
			rotate = false;
		}


		//vertical and horizontal movement
		/*player.AddForce(new Vector2(Input.GetAxis("Horizontal") * forceMultiplier * Time.deltaTime,
		                            Input.GetAxis("Vertical") * forceMultiplier * Time.deltaTime));*/
	}

    public void ResetWalking()
    {
        animator.SetBool(movingID, false);

    }

	

	public void SetDoorTravelID(int id)
	{
		doorTravelId = id;

        
    }

	public void IncrementDoorID()
	{
		++doorTravelId;
	}
    public void DecrementDoorID()
    {
        --doorTravelId;
    }

    public void SetDoorTravelling(bool travel)
	{
		isDoorTravel = travel;
	}

	public Weapon GetCurrentWeapon()
	{
		return currentWeapon;
	}

	public int GetDoorTravelID() 
	{ 
		return doorTravelId;
	}	

    public bool HasAttack()
	{
		return isAttack;
	}

	public bool HasDoorTravel()
	{
		return isDoorTravel;
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		
		if (coll.gameObject.CompareTag("Hiding Spot"))
		{
			HidingSpot hidingSpot = coll.gameObject.GetComponent<HidingSpot>();
			hidingSpot.SetPlayerInRange(true);
		}

        if (coll.gameObject.CompareTag("Door"))
        {
            Door door = coll.gameObject.GetComponent<Door>();
            door.SetPlayerInRange(true);

			// we only need the enter prompt when the player is visible
			// this is to prvent it from appearing when a player travels to a door
			if (gameObject.activeSelf)
			{
				door.EnablePrompt(true);
			}

			if (!isDoorTravel && doorTravelId != door.GetID())
			{
				doorTravelId = door.GetID();
			}
        }
    }
	void OnTriggerExit2D(Collider2D coll)
	{

		if (coll.gameObject.CompareTag("Hiding Spot"))
		{
			Debug.Log ("leaving");

			HidingSpot hidingSpot = coll.gameObject.GetComponent<HidingSpot>();
			hidingSpot.SetPlayerInRange(false);
		}

        if (coll.gameObject.CompareTag("Door"))
        {
            Door door = coll.gameObject.GetComponent<Door>();
            door.SetPlayerInRange(false);

			door.EnablePrompt(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Wall"))
		{
			float offsetY = transform.position.y + 0.0020f;
			Vector3 offsetPos = new Vector3(transform.position.x, offsetY, transform.position.z);

			transform.SetPositionAndRotation(offsetPos, transform.rotation);

		}
	}

}
