using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Backstory : MonoBehaviour
{
        public TextMeshProUGUI Text;
        public GameObject DialogBox;
        public GameObject continueButton;
        string[] dialog = {"Wow, welcome back to town!",
                            "I heard from your dad that you are starting your own cafe!",
                            "I remember when you were little you used to always make me imaginary coffees (chuckles)",
                            "Well let me know when it opens, and good luck with your shop"};
        const int END_OF_DIALOG = 4;
        int dialogIndex;

    // Start is called before the first frame update
    void Start()
    {
        dialogIndex = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogIndex == END_OF_DIALOG){
            SceneManager.LoadScene("Tutorial");
        }
        Text.text = dialog[dialogIndex];
        //if button clicked
        //increment dialog 
    }
    public void nextDialog()
    {
        //Text.text = dialog[index];
        dialogIndex++;
    }


}
