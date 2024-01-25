using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Enemy : MonoBehaviour {

	public Rigidbody2D enemy;
	
	public TextMesh stealthDisplay;
	public TextMesh sightDisplay;

	public EnemySight sightRange;

	public Transform playerLastKnown;

	public List<GameObject> patrolPointList = new List<GameObject>();


	public float rotateValue = 0.0f;
    public float OverwatchTurnDelay = 0.0f;
	public float chaseSpeed = 0.0f;
	public float patrolSpeed = 0.0f;

	public bool isPatrol = true;

	Animator animator;

	int movingID;

    float turnIncrement = 0.0f;

    bool isStealthRange;

	// Use this for initialization
	void Awake () 
	{

		animator = GetComponent<Animator>();

        movingID = Animator.StringToHash("Moving");

        isStealthRange = false;

		playerLastKnown = GameObject.FindGameObjectWithTag("Player").transform;

		float startingRot = gameObject.transform.eulerAngles.y;

		// this is to make sure diaplays are properly orientated even before the first Turn() is applied
		// if a enemy faces left to begin with, they'll need to have their display texts locally rotated to offset being rotated as children
        stealthDisplay.transform.localEulerAngles = new Vector3(0.0f, startingRot, 0.0f);
        sightDisplay.transform.localEulerAngles = new Vector3(0.0f, startingRot, 0.0f);

		// Temp solution to pushing stationary patrols until a more robust solution with Mass can be written
		// if not patrolling; therefore stood still and have no need to be moved
		// freeze x axis so that they cant be pushed by physics
		// a bitmask OR to also freeze rotation in addition to position X and simply having exclusively one or the other
		if (!isPatrol)
		{
			enemy.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }



        
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (isStealthRange)
		{
			stealthDisplay.text = " Stealth Kill: E";

			/*if (Input.GetKeyDown(KeyCode.E))
			{
                gameObject.SetActive(false);
            }*/
			

		}
		else
		{
			stealthDisplay.text = "";
		}

		/*if (!sightRange.HasSeen())
		{
            Patrol();
        }*/


    }

    private void FixedUpdate()
    {
        if (sightRange.HasSeen()) 
		{
			Chase();
			
		}
		else
		{
			if (isPatrol)
			{
                Patrol();
            }
			
		}
    }

    void Patrol()
	{
		// no patrol points means they'll stay put and turn
		if (patrolPointList.Count < 2)
		{
			Overwatch();
        }
		else
		{
			Walk();
		}

		
	}
	// move forward when seeing player
	// implementation for function called in Update is using Delta Time and directkly adding velocity
	// implementation for Fixzed Update involves adding force using the fixed intervals, delta time is unessecary as is already used
	void Chase()
	{
        //transform.LookAt(playerLastKnown);

		// if the stealth prompt is visible, disable this in order to clear it
		if (isStealthRange)
		{
			isStealthRange = false;
		}

        Vector2 finalSpeed = transform.right * chaseSpeed;
        //Vector2 finalSpeed = transform.right * chaseSpeed * Time.deltaTime;

        enemy.AddForce(finalSpeed);
        //enemy.velocity = finalSpeed;
		

        // if animation for moving/walking isnt playing, play it
        if (!animator.GetBool(movingID))
		{
            animator.SetBool(movingID, true);
        }

		

	}

	// turn every so often while standing still
	void Overwatch()
	{
        turnIncrement += 1 * Time.deltaTime;

        if (turnIncrement >= OverwatchTurnDelay)
        {
			Turn();

            turnIncrement = 0.0f;

        }

        // if animation for moving/walking is playing, such as from chase
        // return it to false as they should be stood still when on overwatch
        if (animator.GetBool(movingID))
        {
            animator.SetBool(movingID, false);
        }
    }

	void Turn()
	{
        float yRotate = gameObject.transform.rotation.y;

		

        if (yRotate == 0.0f)
        {
            gameObject.transform.eulerAngles = new Vector3(0.0f, rotateValue, 0.0f);
            stealthDisplay.transform.localEulerAngles = new Vector3(0.0f, rotateValue, 0.0f);
            sightDisplay.transform.localEulerAngles = new Vector3(0.0f, rotateValue, 0.0f);
        }
        else
        {
            gameObject.transform.eulerAngles = Vector3.zero;
            stealthDisplay.transform.localEulerAngles = Vector3.zero;
            sightDisplay.transform.localEulerAngles = Vector3.zero;
        }
    }

	void Walk()
	{
        Vector2 finalSpeed = transform.right * patrolSpeed;
        //Vector2 finalSpeed = transform.right * patrolSpeed * Time.deltaTime;

        enemy.AddForce(finalSpeed);
        //enemy.velocity = finalSpeed;
        //Debug.Log("Walk: " + enemy.velocity);

		// if animation for moving/walking isnt playing, play it
        if (!animator.GetBool(movingID))
        {
            animator.SetBool(movingID, true);
        }
    }

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag("Player"))
		{
			Debug.Log ("Player Enemy Enter");
			
			// Prevents the prompt appearing just as the enem gets into range and before death
			if (!sightRange.HasSeen())
			{
                isStealthRange = true;
            }
			
		}

		if (coll.gameObject.CompareTag("Patrol") && !sightRange.HasSeen())
		{
			for (int i = 0; i < patrolPointList.Count; ++i)
			{
                if (coll.gameObject == patrolPointList[i])
                {
					Turn();
                }
            }

			
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag("Player"))
		{
			Debug.Log ("Player Enemy Exit");
			
			isStealthRange = false;
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

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision");

            if (sightRange.HasSeen())
			{
				SceneManager.LoadScene(0);
			}
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision");

            if (sightRange.HasSeen())
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public bool HasStealthReady()
	{
		return isStealthRange;
	}

	public bool HasEnemySeen()
	{
		return sightRange.HasSeen();
	}


}
