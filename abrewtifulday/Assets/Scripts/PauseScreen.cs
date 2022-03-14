using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject haungsScreen;
    public TextMeshProUGUI heartRatio;
    public TextMeshProUGUI customersServed;


    void Start()
    {

    }

    void Update()
    {
    }

    public void skiplLevel()
    {
        GameController.GC.resumeGame();
        LevelController.nextLevel();
    }

    public void escPressed()
    {
        if (mainScreen.activeInHierarchy || haungsScreen.activeInHierarchy)
            resume();
        else
        {
            Debug.Log("Pause");
            loadMainScreen();
            GameController.GC.PauseGame();
            haungsScreen.SetActive(false);
        }
    }

    private void loadMainScreen()
    {
        mainScreen.SetActive(true);
        heartRatio.text = "Hearts Earned: " + ScoreSystem.getCurrentScore() + "/" + ScoreSystem.getMaxScore();
        customersServed.text = "Customers Served: " + ScoreSystem.getCustomerServedCount();
    }

    public void showHaungsScreen()
    {
        Debug.Log("Haungs screen");
        mainScreen.SetActive(false);
        haungsScreen.SetActive(true);
        Debug.Log(mainScreen.activeInHierarchy);
        Debug.Log(haungsScreen.activeInHierarchy);
    }

    public void showMainScreen()
    {
        // For some reason this was the only way to make this work
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("PauseScreen"))
        {
            o.GetComponent<PauseScreen>().haungsScreen.SetActive(false);
            o.GetComponent<PauseScreen>().mainScreen.SetActive(true);
        }
    }

    public void restartLevel()
    {
        Debug.Log("Restart");
        GameController.GC.resumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

    public void resume()
    {
        Debug.Log("Resume");
        mainScreen.SetActive(false);
        haungsScreen.SetActive(false);
        GameController.GC.resumeGame();
    }
}
