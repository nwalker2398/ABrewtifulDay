using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Material glow, normal;

    private void Start()
    {
        print("Starting chair controller");
    }

    void Update()
    {
    }
}
