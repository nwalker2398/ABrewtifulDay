using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    private Camera camera;
    // Customer stuff
    public Customer firstCustomer;
    bool customerArrowAdded = false;
    bool seatArrow = false;
    [SerializeField] GameObject tutorialChair;

    // Coffee Stuff
    bool coffeeArrow = false; 
    bool pickupCoffeeArrow = false;

    // Dialog stuff
    public TextMeshProUGUI Text;
    public GameObject DialogBox;
    public string[] sentences;
    private int dialogIndex = 0;
    string SeatingDialog = "Click the customer and then click a chair to seat them!";
    string makingCoffeeDialog = "Walk to the coffee machine and click it to make a coffee, and once it is done brewing click again to pick it up.";
    string serveCoffeeDialog = "Walk to the customer and click on them to give them their coffee.";
    string happyBarDialog = "When you make a customer happy, your happiness quota will become more filled!";
    private const int SEATING_CUSTOMER = 0;
    private const int PREPARING_DRINK = 1;
    private const int SERVING_DRINK = 2;
    private const int HAPPY_BAR = 3;

    void Start()
    {
        camera = Camera.main;

        List<string> list = new List<string>();
        list.Add(SeatingDialog);
        list.Add(makingCoffeeDialog);
        list.Add(serveCoffeeDialog);
        list.Add(happyBarDialog);
        sentences = list.ToArray();
        DialogBox.SetActive(false);
    }
    void Update()
    {
        if (dialogIndex == SEATING_CUSTOMER){
            SeatCustomer();
        }
        if (dialogIndex == PREPARING_DRINK){
            PrepareDrink();
        }
        if (dialogIndex == SERVING_DRINK){
            ServeDrink();
        }
    }
    
    /* -------------------- S E A T   C U S T O M E R -------------------- */
    public void SeatCustomer()
    {
        if (SeatingData.waitingCustomers.Count > 0)
        {
            firstCustomer = SeatingData.waitingCustomers[0];
            // add an arrow once the character is at the waiting area
            if (firstCustomer.atWaitingArea && !customerArrowAdded) {
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
                        removeArrow(customerArrowAdded);
                        Debug.Log("adding seat arrow");
                        addArrow(tutorialChair.transform.position);
                        seatArrow = true;
                    }
                    // remove arrow from chair once the chair has been selected 
                    if (objectHit.tag == "Chair" && SeatingData.selectedCustomer != null)
                    {
                        removeArrow(seatArrow);
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
        if (!coffeeArrow){
            Vector3 coffeePosition = CoffeeMaker.coffeeMakerCollider.transform.position;
            Debug.Log(coffeePosition);
            coffeePosition.y += 5;
            Debug.Log(coffeePosition);
            Debug.Log("Adding arrow over coffee!");
            addArrow(coffeePosition);
            coffeeArrow = true;
        }
        if (CoffeeMaker.coffeeClicked && coffeeArrow){
            Debug.Log("Removing Arrow!");
            removeArrow(coffeeArrow);
        }
        if (CoffeeMaker.coffeePickedUp){
            DialogBox.SetActive(false);
            dialogIndex++;
        }
    }

    /* -------------------- S E R V E   D R I N K -------------------- */
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
                removeArrow(customerArrowAdded);
                customerArrowAdded = false;
                DialogBox.SetActive(false);
                dialogIndex++;
            }
        }
    }


    public void nextDialog(int index)
    {
        Text.text = sentences[index];
        DialogBox.SetActive(true);
    }

    public void addArrow(Vector3 pos)
    {
        var arrow = GameObject.FindGameObjectWithTag("Arrow");
        //make new arrow in area 
        //pos.y += arrow.GetComponent<ArrowController>().calcY();
        Debug.Log(pos);
        var new_arrow = Instantiate(arrow, pos, Quaternion.identity);
        SeatingData.customerArrow = new_arrow;
        
    }

    public void removeArrow(bool arrowShowing)
    {
        if (arrowShowing){
            Debug.Log("removing arrow");
            Destroy(SeatingData.customerArrow);
            SeatingData.customerArrow = null;
            //SeatingData.showArrow = showAgain;
        }
    }

}
