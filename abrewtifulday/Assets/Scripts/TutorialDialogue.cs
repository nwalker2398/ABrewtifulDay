using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    bool FirstCustomerClicked = false;
    public string[] sentences;
    public TextMeshProUGUI Text;
    private int dialogIndex = 0;
    public GameObject DialogBox;
    public GameObject firstCustomer;

    string SeatingDialog = "Click the customer and then click a chair to seat them!";
    string makingCoffeeDialog = "Walk to the coffee machine and click it to make a coffee, and once it is done brewing click again to pick it up.";
    string serveCoffeeDialog = "Walk to the customer and click on them to give them their coffee.";
    string happyBarDialog = "When you make a customer happy, your happiness quota will become more filled!";
  
    private const int SEATING_CUSTOMER = 0;
    private const int PREPARING_DRINK = 1;
    private const int SERVING_DRINK = 2;
    private const int HAPPY_BAR = 3;

    //public TMP_InputField tMP_Input;
    // Start is called before the first frame update
    void Start()
    {
        List<string> list = new List<string>();

        list.Add(SeatingDialog);
        list.Add(makingCoffeeDialog);
        list.Add(serveCoffeeDialog);
        list.Add(happyBarDialog);
        sentences = list.ToArray();
        DialogBox.SetActive(false);
    }
    void Update() {
        if (dialogIndex == SEATING_CUSTOMER){
            SeatCustomer();
        }

    }
    
    public void SeatCustomer(){
        Vector3 targetPos = new Vector3(-12.0f, .3f, -6.8f);
        //if mouse down on tag chair, index++
        Text.text = sentences[SEATING_CUSTOMER];
        if (!FirstCustomerClicked){
            FirstCustomerClicked = true;
            if (firstCustomer.transform.position == targetPos )
            {
                addArrow(firstCustomer.transform.position);
                Debug.Log("first customer at position!");
                DialogBox.SetActive(false);
            }
            
        }
        
    }

    public void addArrow(Vector3 pos)
    {
        if (SeatingData.showArrow)
        {
            var arrow = GameObject.FindGameObjectWithTag("Arrow");
            //make new arrow in area 
            pos.y = arrow.GetComponent<ArrowController>().calcY();
            var new_arrow = Instantiate(arrow, pos, Quaternion.identity);
            SeatingData.customerArrow = new_arrow;
        }
    }

    public void removeArrow(bool showAgain)
    {
        if (SeatingData.showArrow)
        {
            Destroy(SeatingData.customerArrow);
            SeatingData.customerArrow = null;
            SeatingData.showArrow = showAgain;
        }
    }

}
