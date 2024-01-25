using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public TextMesh sightDisplay;

    bool isSightRange;

    // Initialisation
    void Awake()
    {
        isSightRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSightRange)
        {
            sightDisplay.text = "Seen";
        }
        else
        {
            sightDisplay.text = "";
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name.Contains("Player"))
        {
            Debug.Log("Happening Enemy Enter");
            Debug.Log(coll.name);

            isSightRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name.Contains("Player"))
        {
            Debug.Log("Happening Enemy Exit");

            isSightRange = false;
        }
    }

    public bool HasSeen()
    {
        return isSightRange;
    }
}
