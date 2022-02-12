using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMaker : MonoBehaviour
{
    private BoxCollider collider;
    private const int COFFEE_WAIT_TIME = 5;
    public GameObject thoughtBubble;
    private AudioSource audioSource;
    public AudioClip bell;
    public AudioClip pour;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        collider = GetComponent<BoxCollider>();
        thoughtBubble.active = false;
    }

    // Makes coffee when clicked 
    void OnMouseDown() {
        StartCoroutine(waitToMake());
    }

    // waits COFFEE_WAIT_TIME amount of seconds before displaying coffee bubble
    IEnumerator waitToMake()
    {
        // delay 
        Debug.Log("Started Coffee at timestamp : " + Time.time);
        audioSource.PlayOneShot(pour);
        yield return new WaitForSeconds(COFFEE_WAIT_TIME);
        Debug.Log("Finished Coffee at timestamp : " + Time.time);
        Debug.Log("coffee is ready!");
        audioSource.PlayOneShot(bell);
        thoughtBubble.active = true;


        // coffee ready sign
    }
}
