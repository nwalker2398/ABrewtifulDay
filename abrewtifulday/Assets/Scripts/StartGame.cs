using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartFirstLevel()
    {
        SceneManager.LoadScene("Cafe1");
    }
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void StartBackstory()
    {
        SceneManager.LoadScene("Backstory");
    }
}
