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
    [SerializeField] private GameObject hand; // in this case the hand is the coffee tray.
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange;
    [SerializeField] private GameObject objectCoffee;
    [SerializeField] private GameObject objectMatcha;
    [SerializeField] private GameObject objectBoba;


    [SerializeField] GameObject timerIcon;
    [SerializeField] GameObject heartIcon;

    void Start()
    {
        objectCoffee.active = false;
        objectMatcha.active = false;
        objectBoba.active = false;
        heartIcon.active = false;
    }

    void OnMouseDown() {
        Debug.Log("Table clicked.");
        // cast a ray from the coffee tray.
        Ray putRay = new Ray(hand.transform.position, hand.transform.forward);
        if (Physics.Raycast(putRay, out RaycastHit hitInfo, pickupRange, pickupLayer)) {
            // if we have a drink to serve, put the drink onto the object
            if (Tray.instance.curDrink.active && objectCoffee.transform.parent.name == hitInfo.transform.gameObject.name) {
                Debug.Log("Object: " + objectCoffee.transform.parent.name + ", Hit Info: " + hitInfo.transform.gameObject.name);
                Tray.instance.curDrink.active = false;
                GameObject drink = null;

                if(Tray.instance.curDrink == Tray.instance.trayCoffee)
                {
                    objectCoffee.active = true;
                    drink = objectCoffee;
                }
                else if (Tray.instance.curDrink == Tray.instance.trayMatcha)
                {
                    objectMatcha.active = true;
                    drink = objectMatcha;
                }
                else if (Tray.instance.curDrink == Tray.instance.trayBoba)
                {
                    objectBoba.active = true;
                    drink = objectBoba;
                }
                timerIcon.active = false;
                heartIcon.active = true;

                //TODO: check if Tray.instance.curDrink == the ordered drink to see if score should be higher than 1

                ScoreSystem.incrementScore(1);
                hitInfo.transform.gameObject.GetComponent<Customer>().Drink(drink.transform.position, drink);
            }
            
            // pick the coffee from the object
            // else if (trayCoffee.active == false && objectCoffee.active == true) {
            //     trayCoffee.active = true;
            //     objectCoffee.active = false;
            // }
        }
    }
}