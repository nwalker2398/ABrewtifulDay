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
    }

    public void StopGame()
    {
        paused = true;
        stopBarista();
    }
    
    // Make sure to save state for unpausing later
    public void PauseGame()
    {
        paused = true;
        stopBarista();
    }

    void stopBarista()
    {
        GameObject barista = GameObject.FindGameObjectWithTag("Barista");
        barista.GetComponent<CharacterController>().enabled = false;
        barista.GetComponent<Animator>().enabled = false;
    }
}
