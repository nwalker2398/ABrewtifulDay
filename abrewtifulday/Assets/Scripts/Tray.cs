using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    public GameObject trayCoffee;
    public GameObject trayMatcha;
    public GameObject trayBoba;
    public GameObject curDrink;

    public static Tray instance;

    // Start is called before the first frame update
    void Start()
    {
        trayCoffee.active = false;
        trayMatcha.active = false;
        trayBoba.active = false;
        curDrink = null;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
