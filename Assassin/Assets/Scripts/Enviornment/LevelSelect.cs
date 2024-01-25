using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    public TextMeshProUGUI buttonText = null;

    GameObject screenManager = null;

    // Start is called before the first frame update
    void Start()
    {
        screenManager = GameObject.FindGameObjectWithTag("Screen Manager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int sceneIndex)
    {
        

        //Screen.fullScreen = true;

        SceneManager.LoadScene(sceneIndex);
        
    }
}
