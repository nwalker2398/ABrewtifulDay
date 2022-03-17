using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float maxProgress = 99;

    void Start() {
        slider.maxValue = ScoreSystem.getMaxScore();
        print(slider.maxValue);
        maxProgress = slider.maxValue;
    }

    public void SetMaxProgress(float progress) {
        print("Setting max value: " + progress);
        slider.maxValue = progress;
        slider.value = progress;
    }

    public void SetInitialProgress(float progress) {
        slider.minValue = progress;
        slider.value = progress;
    }

    public void SetProgress(float progress) {
        Debug.Log($"Set progress: {progress}/{maxProgress}");
        slider.value = progress;
    }

    public bool IsDone() {
        Debug.Log($"Is done: value: {slider.value} / {maxProgress}");
        if (slider.value >= maxProgress) {
            Debug.Log("Progress is done.");
            return true;
        }
        return false;
    }
}
