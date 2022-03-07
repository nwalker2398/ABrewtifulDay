using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchaMaker : MonoBehaviour
{
    private BoxCollider matchaMakerCollider;
    private const int MATCHA_WAIT_TIME = 5;
    public GameObject thoughtBubble;
    public GameObject machineDrink;
    private AudioSource audioSource;
    public AudioClip bell;
    public AudioClip pour;

    void Start()
    {
        thoughtBubble.active = false;
        audioSource = GetComponent<AudioSource>();
        matchaMakerCollider = GetComponent<BoxCollider>();
    }

    void OnMouseDown() {
        Debug.Log("Clicking Matcha!");
        Debug.Log(matchaRange.inMatchaRange);
        if (matchaRange.inMatchaRange && !thoughtBubble.active)
        {
            StartCoroutine(waitToMake());
        }
        else if (matchaRange.inMatchaRange && thoughtBubble.active)
        {
            pickUpMatcha();
        }        
    }

    /** waits MATCHA_WAIT_TIME amount of seconds before displaying the 
     * matcha bubble*/ 
    IEnumerator waitToMake()
    {
        Debug.Log("Started Matcha at timestamp : " + Time.time);
        audioSource.PlayOneShot(pour);
        yield return new WaitForSeconds(MATCHA_WAIT_TIME);
        Debug.Log("Finished Matcha at timestamp : " + Time.time);
        Debug.Log("Matcha is ready!");
        audioSource.PlayOneShot(bell);
        thoughtBubble.active = true;
    }

    /** Adds a coffee to the barista's tray and removes the bubble from above 
     * the coffee machine */ 
    void pickUpMatcha()
    {
        Debug.Log("Matcha being picked up!");
        //must be within x dist of machine to pick up
        //if gameobject with tag barrista colliding with coffee area

        thoughtBubble.active = false;

        Debug.Log(GetComponentInParent<Tray>());
        Debug.Log(GetComponent<Tray>());


        Tray.instance.trayCoffee.active = false;
        Tray.instance.trayMatcha.active = false;
        Tray.instance.trayBoba.active = false;
        Tray.instance.curDrink = machineDrink;
        Tray.instance.curDrink.active = true;
    }
}
