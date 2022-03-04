using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Vector3 destination;
    public Material defaultMaterial;

    [SerializeField] GameObject order;
    [SerializeField] SeatingController controller;
    [SerializeField] Vector3 waitingArea = new Vector3(-2.5f, 0f, -3.5f);
    [SerializeField] Vector3 returnArea = new Vector3(-10f, 0f, -10f);
    [SerializeField] Chair seat;

    private bool toWaitingArea = false;
    private bool atWaitingArea = false;
    private bool toSeat = false;
    private bool atSeat = false;
    private float stopDistance = 2.5f;
    private bool shouldDisplayOrder = false;
    private bool shouldMove = false;
    private float waitingSeatTime = 0f;
    private float waitingRoomTime = 0f;
    private bool rotate = false;
    private float drinkTime = 6f;

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
        order.SetActive(false);
        StartCoroutine(RemoveDrinkDelayed(tableCoffee));
    }

    IEnumerator RemoveDrinkDelayed(GameObject tableCoffee)
    {
        yield return new WaitForSeconds(drinkTime);
        tableCoffee.SetActive(false);

        seat.seatedCustomer = false;
        seat.GetComponent<NavMeshObstacle>().enabled = true;

        destination = returnArea;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<NavMeshAgent>().SetDestination(destination);
        GetComponent<Rigidbody>().useGravity = true;
        rotate = false;
    }

    public void Seat(Chair chair)
    {
        destination = chair.transform.position;
        atWaitingArea = false;
        toSeat = true;
        seat = chair;
        controller.removeCustomerGlow(this);
        SeatingData.seatWaitingCustomer(this);
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
            waitingSeatTime += Time.deltaTime;
            if (waitingSeatTime > 3f)
            {
                order.SetActive(true);
                shouldDisplayOrder = false;
            }
        }

        // If waiting
        // Walk into the store and leave if the waiting room is full
        if (toWaitingArea)
        {
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
                controller.addArrow(transform.position);
            }
        }

        // If walking to seat
        // Remove chair glow once reached
        if (toSeat)
        {
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

        // Rotate to face the table
        if (rotate)
        {
            faceTable();
        }

        // Move to destination
        if (toWaitingArea ) {
            GetComponent<NavMeshAgent>().SetDestination(destination);
        }

        // Leave cafe if waiting for more than 20 seconds in the waiting area
        if (atWaitingArea)
        {
            waitingRoomTime += Time.deltaTime;
            if (waitingRoomTime > 20f)
            {
                leaveCafe();
            }
        }

        // Remove customer once they leave the cafe
        if (Vector3.Distance(transform.position, returnArea) < 1f)
        {
            SeatingData.waitingCustomers.Remove(this);
            Destroy(this);
        }
    }

    float distanceToDestination()
    {
        Vector3 posdiff = transform.position - destination;
        posdiff.y = 0;
        return posdiff.magnitude;
    }

    void faceTable()
    {
        Vector3 lookAt = seat.table.transform.position;
        lookAt.y = transform.position.y;
        Vector3 relPos = lookAt - transform.position;
        Quaternion toRot = Quaternion.LookRotation(relPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRot, 1 * Time.deltaTime * 10f);
    }

    void leaveCafe()
    {
        print("Waiting time exceeded");
        atWaitingArea = false;
        destination = returnArea;
        GetComponent<NavMeshAgent>().SetDestination(destination);
        if (SeatingData.showArrow)
        {
            controller.removeArrow(true);
        }
    }
}
