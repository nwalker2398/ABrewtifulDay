using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] TMP_Text timeText;
    private float gameDuration = 120f;
    private float timeRemaining;
    private bool isPaused = false;

    public GameObject dailyRecapPanel;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = gameDuration;
        //Being(gameDuration);
    }

    // private void Being(int second) {
    //     timeRemaining = second;
    //     StartCoroutine(UpdateTimer());
    // }

    // private IEnumerator UpdateTimer() {
    //     while (timeRemaining >= 0) {
    //         if (!isPaused) {
    //             if (timeRemaining > 0) {
    //         timeText.SetText(timeRemaining.ToString());
    //         fill.fillAmount = Mathf.InverseLerp(0, gameDuration, timeRemaining);
    //         timeRemaining --;
    //         yield return new WaitForSeconds(1f);
    //     }
    //     OnEnd();
    //         }
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.GC.isPaused() && !isPaused)
        {
            timeHasEnd(timeRemaining);
            if (timeRemaining >= 0)
            {
                timeText.SetText(((int)timeRemaining).ToString());
                fill.fillAmount = Mathf.InverseLerp(0, (int)gameDuration, (int)timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
        }
    }

    public bool timeHasEnd(float timeRemaining)
    {
        if (timeRemaining <= 0)
        {
            Debug.Log("Time is up.");
            dailyRecapPanel.SetActive(true);
            GameController.GC.StopGame();
            return true;
        }
        return false;
    }

    //public void LoadNextLevel()
    //{
    //    // need to change to next level once we have multiple
    //    // currently just reloads the scene
    //    // attach to Next Day button on Daily Recap Panel
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    //}

    public void LoadStartMenu()
    {
        //add in stuff for loading start menu scene
    }
}
