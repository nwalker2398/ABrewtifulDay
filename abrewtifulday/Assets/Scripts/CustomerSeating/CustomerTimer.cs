
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CustomerTimer : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] TMP_Text timeText;
    private float waitDuration = 99f;
    private float timeRemaining = 99f;
    private bool isPaused = true;
    private bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = waitDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && !GameController.GC.paused)
        {
            if (timeRemaining >= 0)
            {
                timeText.SetText(((int)timeRemaining).ToString());
                fill.fillAmount = Mathf.InverseLerp(0, (int)waitDuration, (int)timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
        }
    }

    public void startTimer(float newWaitDuration) {
        waitDuration = newWaitDuration;
        timeRemaining = waitDuration;
        isPaused = false;
    }

    public void pauseTimer() {
        isPaused = true;
    }

    public bool timeHasEnd()
    {
        if (timeRemaining <= 0)
        {
            return true;
        }
        return false;
    }

    public float getRemainingTimeRatio() {
        return timeRemaining/waitDuration;
    }

    public float getTimeRemaining() {
        return timeRemaining;
    }

    public float getWaitingDuration() {
        return waitDuration;
    }
}
