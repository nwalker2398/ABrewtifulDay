using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour
{
    private int score = 0;
    [SerializeField] ProgressBar heartBar;
    [SerializeField] float gameTime;
    static ScoreSystem instance;
    private float timeRemaining;
    private bool isRunning;
    //[SerializeField] TMP_Text scoreText;

    void Awake() {
        instance = this;
    }

    public static void incrementScore(int points) {
        if (!instance.heartBar.IsDone()) {
            instance.score += points;
            //instance.scoreText.SetText(instance.score.ToString());
            Debug.Log(instance.score);
            instance.heartBar.SetProgress(instance.score);
        }
    }

    public static bool gameIsRunning() {
        return instance.isRunning;
    }

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
