using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateFullScreen();
    }

    void EvaluateFullScreen()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = true;
        }
    }
}
