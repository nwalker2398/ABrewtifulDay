using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float maxProgress;

    void Start() {
        //maxProgress = slider.maxValue;
        slider.maxValue = ScoreSystem.getMaxScore();
        maxProgress = slider.maxValue;
    }

    public void SetMaxProgress(float progress) {
        slider.maxValue = progress;
        slider.value = progress;
    }

    public void SetInitialProgress(float progress) {
        slider.minValue = progress;
        slider.value = progress;
    }

    public void SetProgress(float progress) {
        slider.value = progress;
    }

    public bool IsDone() {
        if (slider.value >= maxProgress) {
            Debug.Log("Progress is done.");
            return true;
        }
        return false;
    }
}
