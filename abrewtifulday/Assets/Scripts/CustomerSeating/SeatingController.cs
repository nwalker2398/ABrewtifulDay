using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingController : MonoBehaviour
{
    [SerializeField] Vector3 customerStartLocation = new Vector3(-10f, 0.25f, -5f);
    [SerializeField] Material customer_glow, chair_glow, chair_normal;
    [SerializeField] Camera camera;
    private Customer[] customers;
    private float timePassed;

    void Start()
    {
        SeatingData.waitingCustomers = new List<Customer>();
        SeatingData.seatedCustomers = new List<Customer>();
        GameObject[] customerObjects = GameObject.FindGameObjectsWithTag("Customer");
        customers = new Customer[customerObjects.Length];

        for (int i = 0; i < customerObjects.Length; i++)
        {
            customers[i] = customerObjects[i].GetComponent<Customer>();
        }
        timePassed = 0;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 5.0f)
        {
            generateCustomer();
            timePassed = 0f;
        }



        // Glow customer if selected
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                print(objectHit.tag);

                if (objectHit.tag == "Customer")
                {
                    print(SeatingData.selectedCustomer);
                    if (SeatingData.selectedCustomer != null)
                    {
                        removeCustomerGlow();
                    }

                    Renderer renderer = objectHit.gameObject.GetComponent<Renderer>();
                    SeatingData.selectedCustomer = objectHit.gameObject.GetComponent<Customer>();
                    renderer.material = customer_glow;
                }

                // Select a chair if a customer is selected
                if (objectHit.tag == "Chair" && SeatingData.selectedCustomer != null)
                {
                    print(SeatingData.selectedChair); ;
                    if (SeatingData.selectedChair != null)
                    {
                        Renderer oldRenderer = SeatingData.selectedChair.gameObject.GetComponent<Renderer>();
                        oldRenderer.material = chair_normal;
                    }

                    Renderer renderer = objectHit.gameObject.GetComponent<Renderer>();
                    SeatingData.selectedChair = objectHit.gameObject.GetComponent<Chair>();
                    renderer.material = chair_glow;

                    seatCustomer();
                }
            }
        }
    }

    void seatCustomer()
    {
        Chair chair = SeatingData.selectedChair;
        Customer customer = SeatingData.selectedCustomer;
        removeCustomerGlow();

        customer.destination = chair.transform.position;
        customer.atDestination = false;
    }

    void removeCustomerGlow() {
        Renderer oldRenderer = SeatingData.selectedCustomer.gameObject.GetComponent<Renderer>();
        oldRenderer.material = SeatingData.selectedCustomer.defaultMaterial;
    }

    void generateCustomer()
    {
        Customer customer = customers[Random.Range(0, customers.Length)];
        print("Generating new customer");

        var new_customer = Instantiate(customer, customerStartLocation, Quaternion.identity);
        SeatingData.addWaitingCustomer(new_customer);
        new_customer.Generate();
    }
}
