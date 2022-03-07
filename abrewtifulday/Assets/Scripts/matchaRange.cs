using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matchaRange : MonoBehaviour
{
    private SphereCollider matchaArea;
    public static bool inMatchaRange = false; 
    // Start is called before the first frame update
    void Start()
    {
        matchaArea = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.tag == "Barista")
        {
            Debug.Log("Barista in Coffee Range!");
            inMatchaRange = true;
        }
    }
        private void OnTriggerExit(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.tag == "Barista")
        {
            Debug.Log("Barista leaving Coffee Range!");
            inMatchaRange = false;
        }
    }
}
