using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubbleScript : MonoBehaviour
{
    public GameObject customer;
    void Update()
    {
        transform.position = customer.transform.position + new Vector3(0, 0.75f, 0);
    }
}
