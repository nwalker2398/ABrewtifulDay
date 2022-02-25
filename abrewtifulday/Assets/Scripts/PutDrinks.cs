using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Work done to put coffee on the table:
- put coffee object on every table.
- raycast from the coffee tray.
- Box Collider with IsTrigger on for each table. (Collider is a must for raycast)
- assign Put layer to all tables.
- PutDrinks script for each table.
- 
*/

public class PutDrinks : MonoBehaviour
{
    [SerializeField] private GameObject hand; // in this case the hand is the coffee tray.
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange;
    [SerializeField] private GameObject trayCoffee; 
    [SerializeField] private GameObject tableCoffee;

    // Start is called before the first frame update
    void Start()
    {
        tableCoffee.active = false;
    }

    void OnMouseDown() {
        Debug.Log("Table clicked.");
        // cast a ray from the coffee tray.
        Ray putRay = new Ray(hand.transform.position, hand.transform.forward);

        if (Physics.Raycast(putRay, out RaycastHit hitInfo, pickupRange, pickupLayer)) {
            Debug.Log("Found a table");
            // if we have coffee to serve
            if (trayCoffee.active) {
                trayCoffee.active = false;
                tableCoffee.active = true;
                ScoreSystem.incrementScore(1);
            }
        }
    }
}