using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubbleScript : MonoBehaviour
{
    public GameObject customer;
    public GameObject camera;
    void Update()
    {
        transform.position = customer.transform.position + new Vector3(0, 1.5f, 0);
        transform.LookAt(camera.transform);
    }
}
