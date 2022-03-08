using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    float killTime;
    private Camera camera;
    public GameObject tutorialArrow; 
    public GameObject HeartArrow;
    public GameObject TimerArrow;
    // Customer stuff
    public Customer firstCustomer;
    bool customerArrowAdded = false;
    bool seatArrow = false;
    [SerializeField] GameObject tutorialChair;

    // Coffee Stuff
    bool coffeeArrow = false; 

    // GUI stuff
    public TextMeshProUGUI Text;
    public GameObject DialogBox;
    public GameObject StartButton;

    // Dialog stuff
   
    public string[] sentences;
    private int dialogIndex;
    string SeatingDialog = "Click the customer and then click a chair to seat them!";
    string makingCoffeeDialog = "Walk to the coffee machine and click it to make a coffee, and once it is done brewing click again to pick it up.";
    string serveCoffeeDialog = "Walk to the customer and click on them to give them their coffee.";
    string happyBarDialog = "When you make a customer happy, your happiness quota will become more filled!";
    string timerDialog = "You must fill your happiness quota before time's up to move on to the next day!";
    private const int SEATING_CUSTOMER = 0;
    private const int PREPARING_DRINK = 1;
    private const int SERVING_DRINK = 2;
    private const int HAPPY_BAR = 3;
    private const int TIMER = 4;
    private const int STOP = 5;

    void Start()
    {
        dialogIndex = 0;
        StartButton.SetActive(false);
        HeartArrow.SetActive(false);
        TimerArrow.SetActive(false);
        camera = Camera.main;

        List<string> list = new List<string>();
        list.Add(SeatingDialog);
        list.Add(makingCoffeeDialog);
        list.Add(serveCoffeeDialog);
        list.Add(happyBarDialog);
        list.Add(timerDialog);

        sentences = list.ToArray();
        DialogBox.SetActive(false);
    }
    void Update()
    {
        if (dialogIndex == SEATING_CUSTOMER)
            SeatCustomer();
        if (dialogIndex == PREPARING_DRINK)
            PrepareDrink();
        if (dialogIndex == SERVING_DRINK)
            ServeDrink();
        if (dialogIndex == HAPPY_BAR){
            Debug.Log("Entering Happy tutorial");
            UIpointers(HAPPY_BAR, HeartArrow, 5f);
        }
        if (dialogIndex == TIMER){
            UIpointers(TIMER, TimerArrow, 5f);
        }
        if (dialogIndex == STOP){
            StartButton.SetActive(true);
        }
    }
    
    /* -------------------- S E A T   C U S T O M E R -------------------- */
    public void SeatCustomer()
    {
        if (SeatingData.waitingCustomers.Count > 0)
        {
            firstCustomer = SeatingData.waitingCustomers[0];
            // add an arrow once the character is at the waiting area
            if (firstCustomer.atWaitingArea && !customerArrowAdded)
            {
                Debug.Log("adding customer arrow");
                addArrow(firstCustomer.transform.position);
                customerArrowAdded = true;
                nextDialog(SEATING_CUSTOMER);
            }
            // add an arrow over a chair once the character is clicked
            if (Input.GetMouseButtonDown(0) && customerArrowAdded)
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    // remove arrow from customer when they are clicked, and add one over chair
                    if (objectHit.tag == "Customer" && !seatArrow)
                    {
                        Debug.Log("Customer clicked!");
                        removeArrow();
                        Debug.Log("adding seat arrow");
                        addArrow(tutorialChair.transform.position);
                        seatArrow = true;
                    }
                    // remove arrow from chair once the chair has been selected 
                    if (objectHit.tag == "Chair" && SeatingData.selectedCustomer != null)
                    {
                        removeArrow();
                        seatArrow = false;
                        DialogBox.SetActive(false);
                    }
                }
            }
        }
        // once the customer is seated you can move to next tutorial
        if (SeatingData.seatedCustomers.Count > 0)
        {
            firstCustomer = SeatingData.seatedCustomers[0];
            if (firstCustomer.atSeat)
            {
                customerArrowAdded = false;
                dialogIndex++;
            }
        }
    }
    /* -------------------- P R E P A R E   D R I N K -------------------- */
    /* points to coffee machine until its clicked. */
    public void PrepareDrink()
    {
        nextDialog(PREPARING_DRINK);
        if (!coffeeArrow)
        {
            Vector3 coffeePosition = CoffeeMaker.coffeeMakerCollider.transform.position;
            Debug.Log(coffeePosition);
            Debug.Log(coffeePosition);
            Debug.Log("Adding arrow over coffee!");
            addArrow(coffeePosition);
            coffeeArrow = true;
        }
        if (CoffeeMaker.coffeeClicked && coffeeArrow)
        {
            Debug.Log("Removing Arrow!");
            removeArrow();
        }
        if (CoffeeMaker.coffeePickedUp)
        {
            DialogBox.SetActive(false);
            dialogIndex++;
        }
    }

    /* -------------------- S E R V E   D R I N K -------------------- */
    /* puts arrow over your sitting customer until you serve them */ 
    public void ServeDrink()
    {
        nextDialog(SERVING_DRINK);
        if (SeatingData.seatedCustomers.Count > 0)
        {
            firstCustomer = SeatingData.seatedCustomers[0];
            if (firstCustomer.atSeat && !customerArrowAdded)
            {
                addArrow(firstCustomer.transform.position);
                customerArrowAdded = true;
            }
            //if customer gets drink
            //remove arrow
            if (firstCustomer.isServed)
            {
                removeArrow();
                customerArrowAdded = false;
                DialogBox.SetActive(false);
                dialogIndex++;
            }
        }
    }
    /* -------------------- U I   P O I N T E R S -------------------- */
    /* describes the two UI components and points arrows at them */
    public void UIpointers(int index, GameObject arrow, float secs){
        ArrowController a = arrow.GetComponent<ArrowController>();
        if (arrow.active == false){
            a.set_y(arrow.transform.position.y);
            tutorialArrow = a.gameObject;
            nextDialog(index);
            arrow.SetActive(true);
            killTime = Time.time + secs;
        }
        if (Time.time > killTime){
            DialogBox.SetActive(false);
            removeArrow();
            dialogIndex++;
        }
    }

    /* -------------------- N E X T   D I A L O G -------------------- */
    /* displays the next sentence and puts the dialog box back on screen*/
    public void nextDialog(int index)
    {
        Text.text = sentences[index];
        DialogBox.SetActive(true);
    }

    /* -------------------- A D D   A R R O W -------------------- */
    /* adds an arrow at a position, and calls a function in SeatingData to make it bob */
    public void addArrow(Vector3 pos)
    {
        ArrowController arrow = GameObject.FindGameObjectWithTag("Arrow").GetComponent<ArrowController>();
        //make new arrow in area 
        //pos.y += arrow.GetComponent<ArrowController>().calcY();
        Debug.Log(pos);
        var new_arrow = Instantiate(arrow, pos, Quaternion.identity);
        new_arrow.set_y(pos.y + 1.4f);
        tutorialArrow = new_arrow.gameObject;
        
    }
    /* -------------------- R E M O V E   A R R O W -------------------- */
    /* Removes the last arrow added */
    public void removeArrow()
    {
        Debug.Log("removing arrow");
        Debug.Log(tutorialArrow);
        Destroy(tutorialArrow);
        tutorialArrow = null;
        //SeatingData.showArrow = showAgain;
        
    }
}
