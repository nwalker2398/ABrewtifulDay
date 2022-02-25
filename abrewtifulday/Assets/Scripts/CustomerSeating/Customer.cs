using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Vector3 destination;
    public bool toWaitingArea = false;
    public bool atWaitingArea = false;
    public bool toSeat = false;
    public bool atSeat = false;
    public Chair seat;
    public float waitingTime = 0f;

    public Material defaultMaterial;

    [SerializeField] GameObject order;
    [SerializeField] SeatingController controller;
    [SerializeField] Vector3 waitingArea = new Vector3(-1.5f, 0f, -2f);
    [SerializeField] Vector3 returnArea = new Vector3(-10f, 0f, -10f);
    [SerializeField] float stopDistance = 2.5f;

    private bool displayingOrder = false;
    private bool shouldMove = false;

    void Start()
    {
        order.SetActive(false);
    }

    public void Generate()
    {
        gameObject.SetActive(true);
        displayingOrder = true;
        destination = waitingArea;
        shouldMove = true;
        toWaitingArea = true;
    }

    public void Seat(Chair chair)
    {
        Debug.Log(destination);
        Debug.Log(transform.position);
        destination = chair.transform.position;
        atWaitingArea = false;
        toSeat = true;
        seat = chair;
        controller.removeCustomerGlow(this);
        SeatingData.seatWaitingCustomer(this);
        GetComponent<NavMeshAgent>().SetDestination(chair.transform.position);
    }

    void Update()
    {
        //Debug.Log("toWaiting: " + toWaitingArea);
        //Debug.Log("atWaiting: " + atWaitingArea);
        //Debug.Log("toSeat: " + toSeat);
        //Debug.Log("atSeat: " + atSeat);
        // Don't do anything if the character is a default prefab
        if (!shouldMove)
        {
            return;
        }

        // Display order after customer is seated
        if (!displayingOrder && atSeat)
        {
            waitingTime += Time.deltaTime;
            if (waitingTime > 3f)
            {
                order.SetActive(true);
                displayingOrder = true;
            }
        }

        // If waiting
        // Walk into the store and leave if the waiting room is full
        if (toWaitingArea)
        {
            SeatingData.waitingCustomers.ForEach(delegate (Customer c)
            {
                // Stop moving if another waiting customer is in the way
                if (!atWaitingArea &&
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
                        toWaitingArea = false;
                        atWaitingArea = true;
                    }
                }
            });
        }

        // If walking to seat
        // Remove chair glow once reached
        if (toSeat)
        {
            // Debug.Log("Walking to seat: ");
            // Check if customer reached destination
            if (Vector3.Distance(transform.position, destination) < 0.1)
            {
                Debug.Log("At seat!");
                GetComponent<NavMeshAgent>().isStopped = true;
                toSeat = false;
                atSeat = true;
                controller.removeChairGlow(seat);
                this.transform.position += new Vector3(0f, 0.5f, 0f);
                this.transform.LookAt(seat.table.transform);
            }
        }

        // Move to destination
        if (toWaitingArea) {
            //Debug.Log("Walking to destination");
            GetComponent<NavMeshAgent>().SetDestination(destination);
        }

        // Remove customer once they leave the cafe
        if (Vector3.Distance(transform.position, returnArea) < 1f)
        {
            SeatingData.waitingCustomers.Remove(this);
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}
