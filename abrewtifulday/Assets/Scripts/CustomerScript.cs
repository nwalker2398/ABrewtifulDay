using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    [SerializeField] GameObject order;
    private float displayOrderIn;
    private bool displayingOrder = false;
    void Start()
    {
        displayOrderIn = Random.Range(3.0f, 7.0f);
        order.SetActive(false);
    }

    void Update()
    {
        if (!displayingOrder && Time.realtimeSinceStartup > displayOrderIn)
        {
            order.SetActive(true);
            displayingOrder = true;
        }

    }
}
