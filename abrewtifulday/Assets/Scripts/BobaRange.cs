using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaRange : MonoBehaviour
{
    private SphereCollider bobaArea;
    public static bool inBobaRange = false; 
    // Start is called before the first frame update
    void Start()
    {
        bobaArea = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.tag == "Barista")
        {
            Debug.Log("Barista in Boba Range!");
            inBobaRange = true;
        }
    }
        private void OnTriggerExit(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.tag == "Barista")
        {
            Debug.Log("Barista leaving Boba Range!");
            inBobaRange = false;
        }
    }
}
