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
    [SerializeField] float gameDuration;
    private float timeRemaining;
    private bool isPaused = false;

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
        if (!isPaused) {
            timeHasEnd(timeRemaining);
            if (timeRemaining >= 0) {
                timeText.SetText(((int)timeRemaining).ToString());
                fill.fillAmount = Mathf.InverseLerp(0, (int)gameDuration, (int)timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
        }
    }

    public bool timeHasEnd(float timeRemaining) {
        if (timeRemaining <= 0) {
            Debug.Log("Time is up.");
            return true;
        }
        return false;
    }
}
