using UnityEngine;
using System.Collections;

public class SortLayers : MonoBehaviour {
	
	int highestLayer;
	int baseLayer;

	int defaultHead;
	int defaultLeftEye;
	int defaultRightEye;
	int defaultChest;
	int defaultArmour;
	int defaultLeftArm;
	int defaultWeapon;
	int defaultRightArm;
	int defaultLeftLeg;
	int defaultRightLeg;

	GameObject head;
	GameObject leftEye;
	GameObject rightEye;
	GameObject chest;
	GameObject armour;
	GameObject leftArm;
	GameObject weapon;
	GameObject rightArm;
	GameObject leftLeg;
	GameObject rightLeg;


	// Use this for initialization
	void Awake () 
	{
		highestLayer = 0;
		baseLayer = 0;

		// Will hold default values so changes can be made to sorting layers
		// And then reverted back
		defaultHead = 0;
		defaultLeftEye = 0;
		defaultRightEye = 0;
		defaultChest = 0;
		defaultArmour = 0;
		defaultLeftArm = 0;
		defaultWeapon = 0;
		defaultRightArm = 0;
		defaultLeftLeg = 0;
		defaultRightLeg = 0;

		// Head & Children
		head = transform.Find("Head").gameObject;
		leftEye = head.transform.Find("LeftEye").gameObject;
		rightEye = head.transform.Find("RightEye").gameObject;
		// Chest & Children
		chest = transform.Find("Chest").gameObject;
		armour = chest.transform.Find("Armour").gameObject;
		// Right Arm & Children
		rightArm = transform.Find("RightArm").gameObject;
		weapon = rightArm.transform.Find("Weapon").gameObject;
		// Other limbs
		leftArm = transform.Find("LeftArm").gameObject;
		leftLeg = transform.Find("LeftLeg").gameObject;
		rightLeg = transform.Find("RightLeg").gameObject;

		// TEMP: sort based on layer set in the Editor
		int tempStart = GetComponent<SpriteRenderer>().sortingOrder;
		Sort(tempStart);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	// Sort player or enemy limbs based on the parameter
	// Will eventually be used when characters are drawn sequentially
	// starting Layer would be the highest layer of the previous character
	// Or 0 if the first character in the list
	void Sort(int startingLayer)
	{
		baseLayer = startingLayer;
		GetComponent<SpriteRenderer>().sortingOrder = baseLayer;

		// Layer "0"
		defaultHead = baseLayer;
		defaultLeftArm = baseLayer;
		// Layer "1"
		defaultLeftEye = baseLayer + 1;
		defaultRightEye = baseLayer + 1;
		defaultLeftLeg = baseLayer + 1;
		defaultRightLeg = baseLayer + 1;
		// Layer "2"
		defaultChest = baseLayer + 2;
		// Layer "3"
		defaultArmour = baseLayer + 3;
        defaultWeapon = baseLayer + 3;
        //Layer "4"
        defaultRightArm = baseLayer + 4;

		// rightArm is sorted to the highest layer, so set this to the same
		// sequential characters will then start from the next layer up from weapon
		highestLayer = defaultRightArm;

		// Set sorting orders
		SetDefault();


	}
	// Set Or Restore Default values
	public void SetDefault()
	{
		// Layer "0"
		head.GetComponent<SpriteRenderer>().sortingOrder = defaultHead;
		leftArm.GetComponent<SpriteRenderer>().sortingOrder = defaultLeftArm;
		// Layer "1"
		leftEye.GetComponent<SpriteRenderer>().sortingOrder = defaultLeftEye;
		rightEye.GetComponent<SpriteRenderer>().sortingOrder = defaultRightEye;
		leftLeg.GetComponent<SpriteRenderer>().sortingOrder = defaultLeftLeg;
		rightLeg.GetComponent<SpriteRenderer>().sortingOrder = defaultRightLeg;
		// Layer "2"
		chest.GetComponent<SpriteRenderer>().sortingOrder = defaultChest;
		// Layer "3"
		armour.GetComponent<SpriteRenderer>().sortingOrder = defaultArmour;
        weapon.GetComponent<SpriteRenderer>().sortingOrder = defaultWeapon;     
		//Layer "4"
        rightArm.GetComponent<SpriteRenderer>().sortingOrder = defaultRightArm;
	}
	// Used for procedural generation when implemented
	// Will be used to tell the next character implemented where their base layer should start
	public int GetNextLayer()
	{
		// ensures limbs will be on seperate layers
		// to avoid unexpected ordering with same layer sprites
		// I.e highest layered sprite "Weapon" ends 4 layers up from base
		// Next character will start one on from that at 5 layers up from base
		return highestLayer + 1;
	}
}
