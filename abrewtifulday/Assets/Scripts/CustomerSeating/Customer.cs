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
    public bool rotate = false;
    public float drinkTime = 6f;

    public Material defaultMaterial;

    [SerializeField] GameObject order;
    [SerializeField] SeatingController controller;
    [SerializeField] Vector3 waitingArea = new Vector3(-2.5f, 0f, -3.5f);
    [SerializeField] Vector3 returnArea = new Vector3(-10f, 0f, -10f);
    [SerializeField] float stopDistance = 2.5f;

    private bool shouldDisplayOrder = false;
    private bool shouldMove = false;

    void Start()
    {
        order.SetActive(false);
    }

    public void Generate()
    {
        gameObject.SetActive(true);
        destination = waitingArea;
        shouldMove = true;
        toWaitingArea = true;
        order.SetActive(true);
    }

    public void Drink(Vector3 pos, GameObject tableCoffee)
    {
        Debug.Log("Drinking");
        //this.transform.position = pos;
        order.SetActive(false);
        // STOP CUSTOMER WAITING TIMER HERE (2)
        StartCoroutine(RemoveDrinkDelayed(tableCoffee));
    }

    IEnumerator RemoveDrinkDelayed(GameObject tableCoffee)
    {
        yield return new WaitForSeconds(drinkTime);
        tableCoffee.SetActive(false);

        seat.seatedCustomer = false;
        seat.GetComponent<NavMeshObstacle>().enabled = true;

        gameObject.SetActive(false);
    }

    public void Seat(Chair chair)
    {
        Debug.Log(transform.position);
        destination = chair.transform.position;
        Debug.Log(destination);
        atWaitingArea = false;
        // STOP CUSTOMER WAITING TIMER HERE (1)
        toSeat = true;
        seat = chair;
        controller.removeCustomerGlow(this);
        SeatingData.seatWaitingCustomer(this);
        // controller.removeArrow();
        chair.GetComponent<NavMeshObstacle>().enabled = false;
        chair.seatedCustomer = true;
        GetComponent<NavMeshAgent>().SetDestination(destination);
    }

    void Update()
    {
        // Don't do anything if the character is a default prefab
        if (!shouldMove)
        {
            return;
        }

        // Display order after customer is seated
        if (shouldDisplayOrder && atSeat)
        {
            waitingTime += Time.deltaTime;
            if (waitingTime > 3f)
            {
                order.SetActive(true);
                shouldDisplayOrder = false;
            }
        }

        // If waiting
        // Walk into the store and leave if the waiting room is full
        if (toWaitingArea)
        {
            print("To waiting area");
            bool waitingRoomFull = false;
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
                }
            });

            // Enter waiting room
            if (!waitingRoomFull && distanceToDestination() < 0.1)
            { 
                toWaitingArea = false;
                atWaitingArea = true;
                // START CUSTOMER WAITING TIMER HERE
                controller.addArrow(transform.position);
            }
        }

        // If walking to seat
        // Remove chair glow once reached
        if (toSeat)
        {
            print("To seat");
            // Check if customer reached destination
            if (distanceToDestination() < 0.1)
            {

                GetComponent<NavMeshAgent>().enabled = false;
                rotate = true;
                if (transform.position.y < 0.5)
                {
                    transform.Translate(Vector3.up * 1.5f * Time.deltaTime);
                }
                else
                {
                    Debug.Log("At seat!");
                    toSeat = false;
                    atSeat = true;
                    shouldDisplayOrder = true;
                    controller.removeChairGlow(seat);
                    seat.GetComponent<NavMeshObstacle>().enabled = true;
                }
            }
            else
            {
                GetComponent<NavMeshAgent>().SetDestination(destination);
            }
        }

        if (rotate)
        {
            Vector3 lookAt = seat.table.transform.position;
            lookAt.y = transform.position.y;
            Vector3 relPos = lookAt - transform.position;
            Quaternion toRot = Quaternion.LookRotation(relPos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRot, 1 * Time.deltaTime * 10f);
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

    float distanceToDestination()
    {
        Vector3 posdiff = transform.position - destination;
        posdiff.y = 0;
        print(posdiff.magnitude);
        return posdiff.magnitude;
    }
}
