using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Enemy> sceneEnemyList = new List<Enemy>();
    public List<Door> sceneDoorList = new List<Door>();

    public Player player = null;

    public TextMeshProUGUI weaponText = null;
    public GameObject gunUI = null;
    public GameObject bladeUI = null;

    public AudioSource punchSound = null;
    public AudioSource bladeSound = null;
    public AudioSource gunSound = null;

    bool isLevelComplete = false;

    GameObject screenManager = null;

    //PixelPerfectCamera pixelPerfectCamera;




    // Start is called before the first frame update
    void Start()
    {
        /*pixelPerfectCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PixelPerfectCamera>();

        pixelPerfectCamera.refResolutionX = Screen.width;
        pixelPerfectCamera.refResolutionY = Screen.height;*/

        gunUI.SetActive(false);
        bladeUI.SetActive(false);

        screenManager = GameObject.FindGameObjectWithTag("Screen Manager");

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        

        if (sceneEnemyList.Count == 0 )
        {
            GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemyGameObjects.Length; ++i)
            {
                sceneEnemyList.Add(enemyGameObjects[i].GetComponent<Enemy>());
            }
        }

        if (sceneDoorList.Count == 0)
        {
            GameObject[] doorGameObjects = GameObject.FindGameObjectsWithTag("Door");
            List<Door> unsortedDoorList = new List<Door>();
            // sorting the doors
            for (int i = 0; i < doorGameObjects.Length; ++i)
            {
                Door doorScript = (doorGameObjects[i].GetComponent<Door>());

                unsortedDoorList.Add(doorScript);
            }

            sceneDoorList = unsortedDoorList.OrderBy(gameObject => gameObject.name).ToList();
            // giving them IDs based on their sorted order
            for (int j = 0; j < sceneDoorList.Count; ++j)
            {
                Door doorScript = (sceneDoorList[j].GetComponent<Door>());
                doorScript.SetIdentifier(j);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        //EvaluateFullScreen();
        
        EvaluateRestart();

        if (player.HasAttack())
        {
            for (int i = 0;i < sceneEnemyList.Count; ++i)
            {
                // looking for stealth range boolean from stealth box collision
                // checking if enemy has seen prevents mashing the kill button when the enemy is charging towards tnem to prevent death
                if (sceneEnemyList[i].HasStealthReady() 
                    && !sceneEnemyList[i].HasEnemySeen())
                {
                    Debug.Log("Kill");
                    Armour enemyArmour = sceneEnemyList[i].GetComponentInChildren<Armour>();
                    Weapon enemyWeapon = sceneEnemyList[i].GetComponentInChildren<Weapon>();
                    Weapon playerWeapon = player.GetCurrentWeapon();

                    if (playerWeapon.GetColour() == enemyArmour.GetColour())
                    {
                        sceneEnemyList[i].gameObject.SetActive(false);
                        sceneEnemyList.RemoveAt(i);

                        PlayWeaponSound(playerWeapon);

                        playerWeapon.SetColour(enemyWeapon.GetColour());

                        if (weaponText.text != "Weapon: ")
                        {
                            weaponText.text = "Weapon: ";
                        }

                        if (playerWeapon.GetColour() == Equipment.colourGroup.Blade)
                        {
                            
                            bladeUI.SetActive(true);
                            gunUI.SetActive(false);
                            

                        }
                        else if (playerWeapon.GetColour() == Equipment.colourGroup.Bullets)
                        {
                            bladeUI.SetActive(false);
                            gunUI.SetActive(true);
                           
                        }
                        else if (playerWeapon.GetColour() == Equipment.colourGroup.None)
                        {
                            gunUI.SetActive(false);
                            bladeUI.SetActive(false);
                            weaponText.text = "Weapon: None";
                        }
                    }
                }
            }
        }

        EvaluateComplete();

        if (player.HasDoorTravel() && !player.gameObject.activeSelf)
        {
            TraverseDoors();

            //EvaluatePlayerDoor();
        }

        
    }
    // function for evaluating doors based on booleans the player has when using the door
    /*void EvaluatePlayerDoor()
    {
        // if player is inactive, by way of using the door, cycle through the doors, reposition them and reenale them
        if (!player.gameObject.activeSelf && player.HasDoorTravel())
        {

            Debug.Log("before: " + player.GetDoorTravelID());

            if (player.GetDoorTravelID() < 0)
            {
                player.SetDoorID(sceneDoorList.Count - 1);
            }
            else if (player.GetDoorTravelID() >= sceneDoorList.Count)
            {
                player.SetDoorID(0);
            }

            Debug.Log("after: " + player.GetDoorTravelID());

            player.gameObject.transform.position = sceneDoorList[player.GetDoorTravelID()].transform.position;
            player.SetDoorTravel(false);

            sceneDoorList[player.GetDoorTravelID()].EnableMarker(true);
        }

        
    }*/

    void TraverseDoors()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

            /*Door oldDoor = sceneDoorList[currentDoorID];

            oldDoor.EnableMarker(false);*/

            
            player.DecrementDoorID();

            
            // if doorID goes below 0, put it at max
            if (player.GetDoorTravelID() < 0)
            {
                
                player.SetDoorTravelID(sceneDoorList.Count - 1);
            }

            EvaluateDoorMarkers();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            
            player.IncrementDoorID();

            // if doorID goes above the list of doors, put it at 0
            if (player.GetDoorTravelID() >= sceneDoorList.Count)
            {
                
                player.SetDoorTravelID(0);
            }


            EvaluateDoorMarkers();
        }

        int currentDoorID = player.GetDoorTravelID();

        Door currentDoor = sceneDoorList[currentDoorID];

        player.gameObject.transform.position = currentDoor.transform.position;

        //currentDoor.EnableMarker(true);
    }

    void EvaluateDoorMarkers()
    {
        for (int i = 0; i < sceneDoorList.Count; ++i)
        {
            if (player.GetDoorTravelID() == i)
            {
                sceneDoorList[i].EnableMarker(true);
            }
            else
            {
                sceneDoorList[i].EnableMarker(false);
            }
        }
    }
    // reload active scene by finding the build index
    void EvaluateRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(currentIndex);
        }
    }

    void EvaluateComplete()
    {
        if (sceneEnemyList.Count == 0)
        {
            isLevelComplete = true;
        }

        if (!bladeSound.isPlaying && !gunSound.isPlaying && !punchSound.isPlaying
            && isLevelComplete)
        {
            Destroy(screenManager);
            SceneManager.LoadScene(0);
        }
    }

    void PlayWeaponSound (Weapon weapon)
    {
        if (weapon.GetColour() == Equipment.colourGroup.Blade)
        {

            bladeSound.Play();

        }
        else if (weapon.GetColour() == Equipment.colourGroup.Bullets)
        {
            
            gunSound.Play();
        }
        else if (weapon.GetColour() == Equipment.colourGroup.None)
        {
            
            punchSound.Play();
        }
    }
}
