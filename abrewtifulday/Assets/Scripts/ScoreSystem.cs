using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] ProgressBar heartBar;
    [SerializeField] int maxScore;
    private float score = 0;
    static ScoreSystem instance;
    public TextMeshProUGUI finalScoreText;
    private float customerCount;

    void Awake()
    {
        instance = this;
    }

    public static float getMaxScore() {
        return instance.maxScore;
    }

    public static void setScore(float score) {
        instance.score = score;
    }

    public static float completionPercentage() {
        return instance.score / instance.maxScore;
    }

    public static void incrementScore(float points)
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

    public static void decrementScore(float points)
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

    public static float getCurrentScore() {
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

    public static void incrementCustomer() {
        instance.customerCount += 1
    }

    public static int getCustomerServedCount() {
        return instance.customerCount;
    }
}
