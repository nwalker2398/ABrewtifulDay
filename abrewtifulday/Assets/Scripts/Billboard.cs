using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform camera;

    void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward);
    }
}
