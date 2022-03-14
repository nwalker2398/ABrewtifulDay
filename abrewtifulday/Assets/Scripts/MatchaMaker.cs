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
    private bool brewing = false;

    void Start()
    {
        thoughtBubble.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        matchaMakerCollider = GetComponent<BoxCollider>();
    }

    void OnMouseDown() {
        Debug.Log("Clicking Matcha!");
        Debug.Log(matchaRange.inMatchaRange);
        if (matchaRange.inMatchaRange && !thoughtBubble.active && !brewing)
        {
            print("Startng brew");
            brewing = true;
            StartCoroutine(waitToMake());
        }
        else if (matchaRange.inMatchaRange && thoughtBubble.active)
        {
            brewing = false;
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
        thoughtBubble.SetActive(true);
    }

    /** Adds a coffee to the barista's tray and removes the bubble from above 
     * the coffee machine */ 
    void pickUpMatcha()
    {
        Debug.Log("Matcha being picked up!");
        //must be within x dist of machine to pick up
        //if gameobject with tag barrista colliding with coffee area

        thoughtBubble.SetActive(false);

        Debug.Log(GetComponentInParent<Tray>());
        Debug.Log(GetComponent<Tray>());


        Tray.instance.trayCoffee.SetActive(false);
        Tray.instance.trayMatcha.SetActive(false);
        Tray.instance.trayBoba.SetActive(false);
        Tray.instance.curDrink = machineDrink;
        Tray.instance.curDrink.SetActive(true);
    }
}
