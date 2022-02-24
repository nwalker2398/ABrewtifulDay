using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Vector3 destination;
    public bool atDestination = false;

    public Material defaultMaterial;

    [SerializeField] GameObject order;
    [SerializeField] Vector3 waitlistArea = new Vector3(-3f, 0f, -2f);
    [SerializeField] Vector3 returnArea = new Vector3(-10f, 0f, -10f);
    [SerializeField] float stopDistance = 1.5f;

    private float displayOrderIn;
    private bool displayingOrder = false;
    private bool shouldMove = false;

    void Start()
    {
        displayOrderIn = Random.Range(3.0f, 7.0f);
        order.SetActive(false);
    }

    public void Generate()
    {
        gameObject.SetActive(true);
        displayingOrder = true;
        destination = waitlistArea;
        shouldMove = true;
    }

    void Update()
    {
        // Display order after delay
        if (!displayingOrder && Time.realtimeSinceStartup > displayOrderIn)
        {
            order.SetActive(true);
            displayingOrder = true;
        }

        // Don't move if the character is a default prefab
        if (!shouldMove)
        {
            return;
        }

        // Walk into the store and leave if the waiting room is full
        SeatingData.waitingCustomers.ForEach(delegate (Customer c)
        {
            // Stop moving if another waiting customer is in the way
            if (!atDestination &&
                !GameObject.ReferenceEquals(this, c) &&
                Vector3.Distance(transform.position, c.transform.position) < stopDistance)
            {
                // If the waiting room is full, leave the shop
                if (SeatingData.waitingCustomers.Count > 3)
                {
                    destination = returnArea;
                }
                // Else, wait to be seated
                else
                {
                    GetComponent<NavMeshAgent>().isStopped = true;
                    atDestination = true;
                }
            }
        });
        if (!atDestination) {
            GetComponent<NavMeshAgent>().SetDestination(destination);
        }
        if (Vector3.Distance(transform.position, returnArea) < 1f)
        {
            SeatingData.waitingCustomers.Remove(this);
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
