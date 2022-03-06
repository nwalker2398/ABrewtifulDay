using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] ProgressBar heartBar;
    [SerializeField] int maxScore;
    private int score = 0;
    static ScoreSystem instance;
    public TextMeshProUGUI finalScoreText;

    void Awake()
    {
        instance = this;
    }

    public static int getMaxScore() {
        return instance.maxScore;
    }

    public static void setScore(int score) {
        instance.score = score;
    }

    public static int completionPercentage() {
        return instance.score / instance.maxScore;
    }

    public static void incrementScore(int points)
    {
        if (!instance.heartBar.IsDone())
        {
            instance.score += points;
            //instance.scoreText.SetText(instance.score.ToString());
            //Debug.Log(instance.score);
            instance.heartBar.SetProgress(instance.score);
            instance.SetFinalScoreText();
        }
    }

    public static void decrementScore(int points)
    {
        if (!instance.heartBar.IsDone())
        {
            instance.score -= points;
            //instance.scoreText.SetText(instance.score.ToString());
            //Debug.Log(instance.score);
            instance.heartBar.SetProgress(instance.score);
            instance.SetFinalScoreText();
        }
    }

    public static int getCurrentScore() {
        return instance.score;
    }

    private void SetFinalScoreText()
    {
        finalScoreText.text = instance.score.ToString() + "\nCustomers Served!";
    }

    // public static bool gameIsRunning() {
    //     return instance.isRunning;
    // }

    // Start is called before the first frame update
    void Start()
    {
        instance.heartBar.SetInitialProgress(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
