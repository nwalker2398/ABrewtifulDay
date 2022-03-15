using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaMaker : MonoBehaviour
{
    private BoxCollider bobaMakerCollider;
    private const int BOBA_WAIT_TIME = 5;
    public GameObject thoughtBubble;
    public GameObject machineDrink;
    private AudioSource audioSource;
    public AudioClip bell;
    public AudioClip pour;
    private bool brewing = false;

    void Start()
    {
        thoughtBubble.active = false;
        audioSource = GetComponent<AudioSource>();
        bobaMakerCollider = GetComponent<BoxCollider>();
    }

    void Update() 
    {
        if (GameController.GC.stopped) {
            brewing = false;
        }
    }

    void OnMouseDown() {
        Debug.Log("Clicking Boba!");
        Debug.Log(BobaRange.inBobaRange);
        if (BobaRange.inBobaRange && !thoughtBubble.active && !brewing)
        {
            brewing = true;
            StartCoroutine(waitToMake());
        }
        else if (BobaRange.inBobaRange && thoughtBubble.active)
        {
            brewing = false;
            pickUpBoba();
        }        
    }

    /** waits BOBA_WAIT_TIME amount of seconds before displaying the 
     * boba bubble*/ 
    IEnumerator waitToMake()
    {
        Debug.Log("Started Boba at timestamp : " + Time.time);
        audioSource.PlayOneShot(pour);
        yield return new WaitForSeconds(BOBA_WAIT_TIME);
        Debug.Log("Finished Boba at timestamp : " + Time.time);
        Debug.Log("Boba is ready!");
        audioSource.PlayOneShot(bell);
        thoughtBubble.active = true;
    }

    /** Adds a boba to the barista's tray and removes the bubble from above 
     * the boba machine */ 
    void pickUpBoba()
    {
        Debug.Log("Boba being picked up!");
        //must be within x dist of machine to pick up
        //if gameobject with tag barrista colliding with coffee area

        thoughtBubble.active = false;

        Debug.Log(GetComponentInParent<Tray>());
        Debug.Log(GetComponent<Tray>());


        Tray.instance.trayCoffee.SetActive(false);
        Tray.instance.trayMatcha.SetActive(false);
        Tray.instance.trayBoba.SetActive(false);
        Tray.instance.curDrink = machineDrink;
        Tray.instance.curDrink.SetActive(true);
    }
}
