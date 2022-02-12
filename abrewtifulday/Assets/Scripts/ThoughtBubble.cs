using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{
    private SphereCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();

    }

    void OnMouseDown()
    {
        Debug.Log("Bubble being clicked!");
        Destroy(gameObject);
    }

}
