using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController GC;
    public bool stopped = false;
    public bool paused = false;

    void Awake()
    {
        if (GC != null)
        {
            GameObject.Destroy(this);
            this.enabled = false;
        }
        else
        {
            GC = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (!stopped)
            {
                print("Stopping Game");
                StopGame();
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            foreach(GameObject o in GameObject.FindGameObjectsWithTag("PauseScreen"))
            {
                o.GetComponent<PauseScreen>().escPressed();
            }
        }
    }

    public void StopGame()
    {
        paused = true;
        stopBarista();
    }

    public void resumeGame() {
        paused = false;
        resumeBarista();
    }

    // Make sure to save state for unpausing later
    public void PauseGame()
    {
        paused = true;
        stopBarista();
    }

    public bool isPaused()
    {
        return paused;
    }

    void stopBarista()
    {
        GameObject barista = GameObject.FindGameObjectWithTag("Barista");
        barista.GetComponent<CharacterController>().enabled = false;
        barista.GetComponent<Animator>().enabled = false;
    }

    void resumeBarista()
    {
        GameObject barista = GameObject.FindGameObjectWithTag("Barista");
        barista.GetComponent<CharacterController>().enabled = true;
        barista.GetComponent<Animator>().enabled = true;
    }
}
