using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camera;

    private void Start()
    {
        camera = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward);
    }
}
