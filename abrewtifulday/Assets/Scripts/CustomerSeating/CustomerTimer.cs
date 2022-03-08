
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
    [SerializeField] float waitDuration;
    private float timeRemaining;
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
        if (!isPaused)
        {
            //timeHasEnd(timeRemaining);
            if (timeRemaining >= 0)
            {
                timeText.SetText(((int)timeRemaining).ToString());
                fill.fillAmount = Mathf.InverseLerp(0, (int)waitDuration, (int)timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
        }
    }

    public void startTimer() {
        isPaused = false;
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
}
