using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coffeeRange : MonoBehaviour
{
    private SphereCollider coffeeArea;
    public static bool inCoffeeRange = false; 
    // Start is called before the first frame update
    void Start()
    {
        coffeeArea = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.tag == "Barista")
        {
            Debug.Log("Barista in Coffee Range!");
            inCoffeeRange = true;
        }
    }
        private void OnTriggerExit(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.tag == "Barista")
        {
            Debug.Log("Barista leaving Coffee Range!");
            inCoffeeRange = false;
        }
    }
}
