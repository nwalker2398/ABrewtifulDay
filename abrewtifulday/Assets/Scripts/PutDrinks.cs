using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Work done to serve coffee to customers:
- put coffee object on every customer object.
- raycast from the coffee tray.
- Box Collider for each customer object. (Collider is a must for raycast)
- assign Put layer to customer object.
- PutDrinks script for each customer.
*/

public class PutDrinks : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange;
    [SerializeField] private GameObject objectCoffee;

    [SerializeField] GameObject timerIcon;
    [SerializeField] GameObject heartIcon;

    private GameObject tray; 
    private GameObject trayCoffee;

    void Start()
    {
        objectCoffee.SetActive(false);
        heartIcon.SetActive(false);
        tray = GameObject.FindGameObjectWithTag("Tray");
        trayCoffee = GameObject.FindGameObjectWithTag("TrayCoffee");
    }

    void OnMouseDown() {
        Debug.Log("Table clicked.");
        // cast a ray from the coffee tray.
        Ray putRay = new Ray(tray.transform.position, tray.transform.forward);

        if (Physics.Raycast(putRay, out RaycastHit hitInfo, pickupRange, pickupLayer)) {
            // if we have coffee to serve, put the coffee onto the object
            if (trayCoffee.active && objectCoffee.transform.parent.name == hitInfo.transform.gameObject.name) {
                Debug.Log("Object: " + objectCoffee.transform.parent.name + ", Hit Info: " + hitInfo.transform.gameObject.name);
                trayCoffee.SetActive(false);
                objectCoffee.SetActive(true);
                timerIcon.SetActive(false);
                heartIcon.SetActive(true);
                ScoreSystem.incrementScore(1);
                hitInfo.transform.gameObject.GetComponent<Customer>().Drink(objectCoffee.transform.position, objectCoffee);
            }
            
            // pick the coffee from the object
            // else if (trayCoffee.active == false && objectCoffee.active == true) {
            //     trayCoffee.active = true;
            //     objectCoffee.active = false;
            // }
        }
    }
}