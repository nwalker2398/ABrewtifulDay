using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMaker : MonoBehaviour
{
    public static BoxCollider coffeeMakerCollider;
    private const int COFFEE_WAIT_TIME = 4;
    public GameObject thoughtBubble;
    public GameObject machineDrink;
    private AudioSource audioSource;
    public AudioClip bell;
    public AudioClip pour;
    public static bool coffeeClicked = false;
    public static bool coffeePickedUp = false;
    public static bool brewing = false;

    void Start()
    {
        thoughtBubble.active = false;
        audioSource = GetComponent<AudioSource>();
        coffeeMakerCollider = GetComponent<BoxCollider>();
    }

    void Update() 
    {
        if (GameController.GC.stopped) {
            brewing = false;
        }
    }

    void OnMouseDown() {
        Debug.Log("Clicking Coffee!");
        Debug.Log(coffeeRange.inCoffeeRange);
        if (coffeeRange.inCoffeeRange && !thoughtBubble.active && !brewing)
        {
            brewing = true;
            StartCoroutine(waitToMake());
        }
        else if (coffeeRange.inCoffeeRange && thoughtBubble.active)
        {
            brewing = false;
            pickUpCoffee();
        }        
    }

    /** waits COFFEE_WAIT_TIME amount of seconds before displaying the 
     * coffee bubble*/ 
    IEnumerator waitToMake()
    {
        Debug.Log("Started Coffee at timestamp : " + Time.time);
        audioSource.PlayOneShot(pour);
        yield return new WaitForSeconds(COFFEE_WAIT_TIME);
        Debug.Log("Finished Coffee at timestamp : " + Time.time);
        Debug.Log("coffee is ready!");
        audioSource.PlayOneShot(bell);
        thoughtBubble.active = true;
    }

    /** Adds a coffee to the barista's tray and removes the bubble from above 
     * the coffee machine */ 
    void pickUpCoffee()
    {
        Debug.Log("Coffee being picked up!");
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
