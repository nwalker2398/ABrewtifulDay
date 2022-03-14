using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Vector3 destination;
    public float stopDistance = 1.0f;

    [SerializeField] GameObject order;
    [SerializeField] SeatingController controller;
    [SerializeField] Vector3 waitingArea = new Vector3(-2.5f, 0f, -3.5f);
    [SerializeField] Vector3 returnArea = new Vector3(-10f, 0f, -10f);

    public bool toWaitingArea = false;
    public bool atWaitingArea = false;

    public bool toWaitingLine = false;
    public bool atWaitingLine = false;

    private Chair seat;
    private bool toSeat = false;
    public bool atSeat = false;
    private bool leaveIfFull = true;
    private bool shouldDisplayOrder = false;
    private bool shouldMove = false;
    [SerializeField] float waitingSeatTime = 60f;
    [SerializeField] float waitingRoomTime = 20f;
    private bool rotate = false;
    private float drinkTime = 6f;

    [SerializeField] CustomerTimer timer;
    public bool isServed = false;
    [SerializeField] GameObject angryUI;

    // For Ordering Drinks
    [SerializeField] GameObject customerCanvas;
    // Option 1: Using 2d sprites
    public SpriteRenderer spriteRenderer;
    public Sprite coffeeSprite;
    public Sprite bobaSprite;
    public Sprite matchaSprite;
    // Option 2: Using 3d prefabs
    public GameObject coffeePrefab;
    public GameObject bobaPrefab;
    public GameObject matchaPrefab;

    private int randomOrder;

    CanvasRenderer timerRenderer;

    void Start()
    {
        timerRenderer = timer.GetComponent<CanvasRenderer>();

        order.SetActive(false);

        coffeePrefab.SetActive(false);
        bobaPrefab.SetActive(false);
        matchaPrefab.SetActive(false);
        angryUI.SetActive(false);

        randomOrder = randomizeOrder();

        // timerRenderer.enabled = false;
        // waitingRoomTimerRenderer.enabled = false;

        //customerCanvas.SetActive(false);
    }

    public void Generate()
    {
        gameObject.SetActive(true);
        destination = waitingArea;
        shouldMove = true;
        toWaitingArea = true;
        // order.SetActive(true);

        timer.startTimer(waitingRoomTime);
        hideOrder();
    }

    private int randomizeOrder() {
        Scene scene = SceneManager.GetActiveScene();
        int n;
        if (scene.name == "Tutorial" || scene.name == "Cafe1") {
            n = 1;

            spriteRenderer.sprite = coffeeSprite;
            float scale = 0.3f;
            spriteRenderer.transform.localScale = new Vector3(scale, scale, scale);

            //coffeePrefab.SetActive(true);
            order = coffeePrefab;
        }
        else {
            // Randomize order
            n = (int)Random.Range(0,4);

            if (n == 1) {
                spriteRenderer.sprite = coffeeSprite;
                float scale = 0.3f;
                spriteRenderer.transform.localScale = new Vector3(scale, scale, scale);

                //coffeePrefab.SetActive(true);
                order = coffeePrefab;
            }
            else if (n == 2) {
                spriteRenderer.sprite = bobaSprite;
                float scale = 0.2f;
                spriteRenderer.transform.localScale = new Vector3(scale, scale, scale);

                //bobaPrefab.SetActive(true);
                order = bobaPrefab;
            }
            else {
                spriteRenderer.sprite = matchaSprite;
                float scale = 0.2f;
                spriteRenderer.transform.localScale = new Vector3(scale, scale, scale);

                //matchaPrefab.SetActive(true);
                order = matchaPrefab;
            }
        }
        return n;
    }

    private float calculateScore(GameObject drink) {
        // check if the order match the given drink
        float rawScore;
        float finalScore;
        if (randomOrder == 1 && drink.transform.name == "objectCoffee") {
            rawScore = 3;
        }
        else if (randomOrder == 2 && drink.transform.name == "objectBoba") {
            rawScore = 3;
        }
        else if (randomOrder == 3 && drink.transform.name == "objectMatcha") {
            rawScore = 3;
        }
        else {
            rawScore = 1;
        }
        
        float ratio = timer.getRemainingTimeRatio();

        if (ratio < 0.34) {
            finalScore = rawScore - 2;
        }
        else if (ratio >= 0.34 && ratio < 0.68) {
            finalScore = rawScore - 1;
        }
        else {
            finalScore = rawScore;
        }

        if (finalScore < 0) {
            return 0;
        }
        return finalScore;
    }

    public void Drink(Vector3 pos, GameObject drink)
    {
        isServed = true;

        ScoreSystem.incrementScore(calculateScore(drink));
        ScoreSystem.incrementCustomer();
        
        Debug.Log("Drinking");
        order.SetActive(false);
        StartCoroutine(RemoveDrinkDelayed(drink));
    }

    IEnumerator RemoveDrinkDelayed(GameObject drink)
    {
        yield return new WaitForSeconds(drinkTime);
        drink.SetActive(false);

        seat.seatedCustomer = false;
        seat.GetComponent<NavMeshObstacle>().enabled = true;

        leaveCafe(false);
    }

    public void Seat(Chair chair)
    {
        print("Seating customer");
        destination = chair.transform.position;
        atWaitingArea = false;
        toSeat = true;
        angryUI.SetActive(false);
        seat = chair;
        controller.removeCustomerGlow(this);
        SeatingData.seatWaitingCustomer(this);
        GetComponent<NavMeshAgent>().isStopped = false;
        chair.GetComponent<NavMeshObstacle>().enabled = false;
        chair.seatedCustomer = true;
        GetComponent<NavMeshAgent>().SetDestination(destination);
    }

    void Update()
    {
        if (GameController.GC.paused || GameController.GC.stopped)
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            return;
        }

        // If character is atSeat or atWaitingArea but is not being served or seated, and the time is up, then leave
        if (timer.timeHasEnd() && !isServed) {
            // if (atWaitingArea) {
            //     atWaitingArea = false;
            //     SeatingData.waitingCustomers.Remove(this);
            // }
            if (atSeat)
            {
                seat.seatedCustomer = false;
                seat.GetComponent<NavMeshObstacle>().enabled = true;
            }
            order.SetActive(false);

            angryUI.SetActive(true);
            leaveCafe(true); 
        }

        // Don't do anything if the character is a default prefab
        if (!shouldMove)
        {
            return;
        }

        // Display order after customer is seated
        // if (shouldDisplayOrder && atSeat)
        // {
        //     waitingSeatTime += Time.deltaTime;
        //     if (waitingSeatTime > 3f)
        //     {
        //         //order.SetActive(true);
        //         shouldDisplayOrder = false;
        //     }
        // }

        // If waiting
        // Walk into the store and leave if the waiting room is full
        if (toWaitingArea)
        {
            bool waitingRoomFull = false;
            bool setStopped = false;
            SeatingData.waitingCustomers.ForEach(delegate (Customer c)
            {
                // Stop moving if another waiting customer is in the way
                if (!atWaitingArea &&
                    !GameObject.ReferenceEquals(this, c) &&
                    Vector3.Distance(transform.position, c.transform.position) < stopDistance)
                {
                    // If the waiting room is full, leave the shop
                    if (leaveIfFull && SeatingData.waitingCustomers.Count > 4)
                    {
                        destination = returnArea;
                    }
                    // If another customer is between the current customer and the ideal waiting area, stop moving
                    else if (distanceToDestination() > Vector3.Distance(destination, c.transform.position))
                    {
                        GetComponent<NavMeshAgent>().isStopped = true;
                        setStopped = true;
                        leaveIfFull = false;
                    }
                }

                if (timer.timeHasEnd())
                {
                    leaveCafe(false);
                    angryUI.SetActive(true);
                }
            });


            if (setStopped == false)
            {
                GetComponent<NavMeshAgent>().isStopped = false;
            }

            // Enter waiting room
            if (!waitingRoomFull && distanceToDestination() < 0.1)
            {
                toWaitingArea = false;
                atWaitingArea = true;

                // timer.startTimer(waitingRoomTime);
            }
        }

        // If walking to seat
        // Remove chair glow once reached
        if (toSeat)
        {
            timer.pauseTimer();
            // Check if customer reached destination
            if (distanceToDestination() < 0.1)
            {

                GetComponent<NavMeshAgent>().enabled = false;
                rotate = true;
                if (transform.position.y <= 0.66)
                {
                    transform.Translate(Vector3.up * 0.11f);
                }
                else
                {
                    //Debug.Log("At seat!");
                    Vector3 pos = transform.position;
                    transform.position = new Vector3(pos.x, 0.66f, pos.z);
                    transform.position = new Vector3(pos.x, 0.66f, pos.z);
                    toSeat = false;
                    atSeat = true;
                    shouldDisplayOrder = true;
                    controller.removeChairGlow(seat);
                    seat.GetComponent<NavMeshObstacle>().enabled = true;
                    
                    //timerRenderer.enabled = true;
                    timer.startTimer(waitingSeatTime);
                    timer.GetComponent<Image>().color = new Color32(181, 190, 148, 255);
                    timer.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(60, 179, 113, 160);

                    showOrder();
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
            //waitingRoomTime += Time.deltaTime;

            if (timer.timeHasEnd())
            {
                leaveCafe(false);
                angryUI.SetActive(true);
            }
        }

        // Remove customer once they leave the cafe
        if (Vector3.Distance(transform.position, returnArea) < 2f)
        {
            SeatingData.waitingCustomers.Remove(this);
            Destroy(this);
        }

        //hideImage(customerCanvas);
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

    void leaveCafe(bool decreaseScore)
    {
        hideOrder();

        hideTimer();

        atWaitingArea = false;

        destination = returnArea;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<NavMeshAgent>().SetDestination(destination);
        GetComponent<Rigidbody>().useGravity = true;
        rotate = false;

        /*if (SeatingData.showArrow)
        {
            controller.removeArrow(true);
        }*/

        if (decreaseScore && ScoreSystem.getCurrentScore() > 0) {
            ScoreSystem.decrementScore(1);
        }
    }

    private void hideOrder()
    {
        order.SetActive(false);
        // coffeePrefab.SetActive(false);
        // bobaPrefab.SetActive(false);
        // matchaPrefab.SetActive(false);
    }

    private void showOrder() {
        order.SetActive(true);
    }

    private void hideImage(GameObject gameObject) {
        //gameObject.GetComponent<Image>().enabled = false;
        //timer.transform.GetChild(0).gameObject.active = false;

        Image[] childrenImages = gameObject.GetComponentsInChildren<Image>();
        foreach(Image img in childrenImages) {
            img.enabled = false;
        }
    }

    private void hideTimer() {
        timer.GetComponent<Image>().enabled = false;
        timer.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
    }
}
