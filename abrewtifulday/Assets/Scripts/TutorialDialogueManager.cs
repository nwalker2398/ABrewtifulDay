using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueManager : MonoBehaviour
{
    string SeatingDialog = "Click the customer and then click a chair to seat them!";
    string makingCoffeeDialog = "Walk to the coffee machine and click it to make a coffee, and once it is done brewing click again to pick it up.";
    string serveCoffeeDialog = "Walk to the customer and click on them to give them their coffee.";
    string happyBarDialog = "When you make a customer happy, your happiness quota will become more filled!";
  
    Queue<string> advice;
    //public TMP_InputField tMP_Input;
    // Start is called before the first frame update
    void Start()
    {
        advice = new Queue<string>();
        //tMP_Input = gameObject.GetComponent<TMP_InputField>();
        //tMP_Input.text = SeatingDialog;
        
    }

    // Update is called once per frame
    void Update()
    {
        firstCustomerSeating();
    }
    void firstCustomerSeating()
    {
        //when first customer walks in
            //trigger seatingDialog
            // arrows

    }
    //public void StartDialog(TutorialDialogue )

}
