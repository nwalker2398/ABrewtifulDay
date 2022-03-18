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
    [SerializeField] private GameObject objectMatcha;
    [SerializeField] private GameObject objectBoba;


    [SerializeField] GameObject timerIcon;
    [SerializeField] GameObject heartIcon;

    private GameObject tray;
    private GameObject trayCoffee;
    private Camera camera;

    void Start()
    {
        objectCoffee.SetActive(false);
        objectMatcha.SetActive(false);
        objectBoba.SetActive(false);
        heartIcon.SetActive(false);
        tray = GameObject.FindGameObjectWithTag("Tray");
        trayCoffee = GameObject.FindGameObjectWithTag("TrayCoffee");
        camera = Camera.main;
    }

    void OnMouseDown()
    {
        Debug.Log("Table clicked.");
        // cast a ray from the coffee tray.
        Ray putRay = new Ray(tray.transform.position, tray.transform.forward);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Only deliver drinks to customers
            if (hitInfo.transform.tag != "Customer")
            {
                return;
            }

            // Only deliver drinks if barista is close to the customer
            GameObject barista = trayCoffee = GameObject.FindGameObjectWithTag("Barista");
            if (Vector3.Distance(barista.transform.position, hitInfo.transform.gameObject.transform.position) > 1.5f)
            {
                return;
            }

            // if we have a drink to serve, put the drink onto the object
            if (Tray.instance.curDrink != null && Tray.instance.curDrink.active && objectCoffee.transform.parent.name == hitInfo.transform.gameObject.name && hitInfo.transform.gameObject.GetComponent<Customer>().atSeat == true)
            {
                Debug.Log("Object: " + objectCoffee.transform.parent.name + ", Hit Info: " + hitInfo.transform.gameObject.name);
                Tray.instance.curDrink.SetActive(false);
                //trayCoffee.SetActive(false);
                GameObject drink = null;

                if (Tray.instance.curDrink == Tray.instance.trayCoffee)
                {
                    objectCoffee.SetActive(true);
                    drink = objectCoffee;
                }
                else if (Tray.instance.curDrink == Tray.instance.trayMatcha)
                {
                    objectMatcha.SetActive(true);
                    drink = objectMatcha;
                }
                else if (Tray.instance.curDrink == Tray.instance.trayBoba)
                {
                    objectBoba.SetActive(true);
                    drink = objectBoba;
                }
                timerIcon.SetActive(false);
                heartIcon.SetActive(true);

                //TODO: check if Tray.instance.curDrink == the ordered drink to see if score should be higher than 1

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