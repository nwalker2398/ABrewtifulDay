using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDrinks : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange;
    [SerializeField] private GameObject trayCoffee; 
    [SerializeField] private GameObject tableCoffee;

    // Start is called before the first frame update
    void Start()
    {
        tableCoffee.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("E");
            Ray putRay = new Ray(hand.transform.position, hand.transform.forward);

            if (Physics.Raycast(putRay, out RaycastHit hitInfo, pickupRange, pickupLayer)) {
                Debug.Log("Found a table");
                // if we have coffee to serve
                if (trayCoffee.active) {
                    trayCoffee.active = false;
                    tableCoffee.active = true;
                }
            }
        }
    }
}
