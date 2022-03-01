using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 destination = new Vector3(0, 0.5f, 3f);
    public Vector3 lookAt = new Vector3(0, 0f, 0f);
    public Vector3 force = new Vector3(0, 5, 0);
    public float speed = 0.05f;
    public float rotSpeed = 3f;
    public bool move = true;
    public bool rotate = false;
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(destination);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 posdiff = transform.position - destination;
        posdiff.y = 0;
        // Check if customer reached destination
        if (posdiff.magnitude < 0.1 && move)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            rotate = true;
            if (transform.position.y < 0.5)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            else
            {
                move = false;
            }
        }
        if (rotate)
        {
            Vector3 relPos = lookAt - transform.position;
            Quaternion toRot = Quaternion.LookRotation(relPos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRot, 1 * Time.deltaTime * rotSpeed);
        }
    }
}
