using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject marker = null;
    public GameObject enterPrompt = null;
    
    Player player;
    
    int identifier = 0;

    bool playerInRange = false;
    bool inUse = false;

    // Awake is called before the first frame update
    void Awake()
    {
        marker.SetActive(false);
        enterPrompt.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //player = GameObject.FindGameObjectWithTag("Player");

        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange || inUse)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inUse = !inUse;
                EnableMarker(inUse);
            }

            if (inUse)
            {

                player.gameObject.SetActive(false);
                player.SetDoorTravelling(true);
                //player.SetDoorTravelID(identifier);

                EnablePrompt(false);

                
                



                //player.SetActive(false);
                //Debug.Log("active false");

            }
            else
            {
                // Player set to visible and no longer door hopping
                // remove here marker, but we are visible by the door again so make enter prompt appear
                player.gameObject.SetActive(true);
                player.SetDoorTravelling(false);
                
                EnableMarker(false);
                

            }
            // Modify the door the player is at and wants to go to so game manager can shuffle them
            /*if (!player.HasDoorTravel())
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    int newDoorID = identifier + 1;
                    Debug.Log("ID:" + identifier);

                    player.SetDoorID(newDoorID);
                    player.SetDoorTravel(true);

                    EnableMarker(false);
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    int newDoorID = identifier - 1;
                    

                    player.SetDoorID(newDoorID);
                    player.SetDoorTravel(true);

                    EnableMarker(false);
                }
            }*/
            

        }

        

        



        
    }

    public int GetID()
    { 
        return identifier; 
    }  

    public void SetPlayerInRange(bool isPlayer)
    {
        playerInRange = isPlayer;
    }

    public void SetIdentifier(int id)
    {
        identifier = id;
        Debug.Log(gameObject.name + " ID: " + identifier);
    }

    public void EnableMarker(bool enable)
    {
        marker.SetActive(enable);
    }

    public void EnablePrompt(bool enable)
    {
        enterPrompt.SetActive(enable);    
    }
}
