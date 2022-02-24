using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeIndicatorController : MonoBehaviour
{
    public static bool activeBubble;
    public SphereCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        activeBubble = false;
        collider = GetComponent<SphereCollider>();
    }

    private void OnMouseDown() {
        Debug.Log("picking up order");
        activeBubble = false;
    }
}
