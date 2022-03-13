using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject haungsScreen;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            mainScreen.SetActive(!mainScreen.active);
        }
    }

    public void showHaungsScreen()
    {
        mainScreen.SetActive(false);
        haungsScreen.SetActive(true);
    }

    public void showMainScreen()
    {
        Debug.Log("main screen");
        haungsScreen.SetActive(false);
        mainScreen.SetActive(false);
    }
}
