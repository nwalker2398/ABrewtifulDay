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
    [SerializeField] private float pickupRange = 10;
    [SerializeField] private GameObject trayCoffee; 
    [SerializeField] private GameObject tableCoffee;
    [SerializeField] private float serveRadius = 2; 

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
            Debug.Log("Raycast hit:" + hitInfo.transform.gameObject.name);
            // if we have coffee to serve, put the coffee onto the table
            //if (trayCoffee.active) {
            if (trayCoffee.active && tableCoffee.transform.parent.name == hitInfo.transform.gameObject.name) {
                trayCoffee.active = false;
                tableCoffee.active = true;
                ScoreSystem.incrementScore(1);

                Collider[] hitColliders = Physics.OverlapSphere(tableCoffee.transform.position, serveRadius);
                foreach (var hitCollider in hitColliders)
                {
                   if (hitCollider.ToString().Contains("ustomer"))
                    {
                        Debug.Log("customer");
                        Debug.Log(hitCollider.gameObject.GetComponent<Customer>());
                        Customer c = hitCollider.gameObject.GetComponent<Customer>();
                        c.Drink(tableCoffee.transform.position, tableCoffee);
                        break;
                        //((Customer)hitCollider.gameObject).Drink(tableCoffee.transform.position);
                    }
                }
            }
            // pick the coffee from the table
            else if (trayCoffee.active == false && tableCoffee.active == true) {
                trayCoffee.active = true;
                tableCoffee.active = false;
            }
        }
    }
}