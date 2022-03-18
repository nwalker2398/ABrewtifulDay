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
    private int customerCount;

    int [] playerScores = new int[20];
    int [] customerServed = new int[20];

    void Awake()
    {
        instance = this;
    }

    public static float getMaxScore() {
        return instance.maxScore;
    }

    public static void setMaxScore(int maxScore)
    {
         instance.maxScore = maxScore;
    }

    public static void setScore(float score) {
        instance.score = score;
        instance.heartBar.SetProgress(instance.score);
        //instance.SetFinalScoreText();
    }

    public static float completionPercentage() {
        return instance.score / instance.maxScore;
    }

    public static void incrementScore(float points)
    {
        // if (!instance.heartBar.IsDone())
        // {
        //     instance.score += points;
        //     //instance.scoreText.SetText(instance.score.ToString());
        //     //Debug.Log(instance.score);
        //     instance.heartBar.SetProgress(instance.score);
        //     //instance.SetFinalScoreText();
        //     instance.playerScores[LevelController.LC.currentLevel] = instance.playerScores[LevelController.LC.currentLevel] + (int)points;
        // }

        instance.score += points;
        //instance.scoreText.SetText(instance.score.ToString());
        //Debug.Log(instance.score);
        instance.heartBar.SetProgress(instance.score);
        //instance.SetFinalScoreText();
        instance.playerScores[LevelController.LC.currentLevel] = instance.playerScores[LevelController.LC.currentLevel] + (int)points;
    }

    public static void decrementScore(float points)
    {
        Debug.Log("Decrement score");
        // if (!instance.heartBar.IsDone())
        // {
        //     instance.score -= points;
        //     //instance.scoreText.SetText(instance.score.ToString());
        //     //Debug.Log(instance.score);
        //     instance.heartBar.SetProgress(instance.score);
        //     //instance.SetFinalScoreText();

        //     instance.playerScores[LevelController.LC.currentLevel] = instance.playerScores[LevelController.LC.currentLevel] + (int)points;
        // }
        
        if (instance.score > 0) {
            instance.score -= points;
            //instance.scoreText.SetText(instance.score.ToString());
            //Debug.Log(instance.score);
            instance.heartBar.SetProgress(instance.score);
            //instance.SetFinalScoreText();

            instance.playerScores[LevelController.LC.currentLevel] = instance.playerScores[LevelController.LC.currentLevel] + (int)points;
        }
    }

    public static float getCurrentLevelScore() {
        return instance.playerScores[LevelController.LC.currentLevel];
    }

    public static void SetFinalScoreText()
    {
        int custServed = instance.playerScores[LevelController.LC.currentLevel];
        if (custServed > instance.maxScore) {
            custServed = instance.maxScore;
        }
        instance.finalScoreText.text = $"{instance.customerServed[LevelController.LC.currentLevel]} Customers Served\n{custServed}/{instance.maxScore} Hearts Collected";
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
        instance.customerServed[LevelController.LC.currentLevel] = instance.customerServed[LevelController.LC.currentLevel] + 1;
    }

    public static int getCurrentLevelCustomerServedCount() {
        return instance.customerServed[LevelController.LC.currentLevel];
    }
}
