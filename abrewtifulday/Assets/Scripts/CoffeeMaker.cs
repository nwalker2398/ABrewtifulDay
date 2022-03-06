using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMaker : MonoBehaviour
{
    private BoxCollider coffeeMakerCollider;
    private const int COFFEE_WAIT_TIME = 5;
    public GameObject thoughtBubble;
    public GameObject trayCoffee;
    private AudioSource audioSource;
    public AudioClip bell;
    public AudioClip pour;
    /** Hevin! You will probably want to have a boolean like this added so you 
     * cant pickup another coffee until the previous one is served in the 
     * pickUpCoffee() logic!

    private bool coffeeServed = false;*/
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        coffeeMakerCollider = GetComponent<BoxCollider>();
        thoughtBubble.active = false;
        trayCoffee.active = false;
    }

    void OnMouseDown() {
        Debug.Log("Clicking Coffee!");
        if (coffeeRange.inCoffeeRange && !thoughtBubble.active)
        {
            StartCoroutine(waitToMake());
        }
        else if (coffeeRange.inCoffeeRange && thoughtBubble.active)
        {
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
        //must be within x dist of machine to pick up
        //if gameobject with tag barrista colliding with coffee area
        
        thoughtBubble.active = false;
        trayCoffee.active = true;
        
    }
}
