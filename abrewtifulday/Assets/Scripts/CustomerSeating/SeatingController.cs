using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingController : MonoBehaviour
{
    [SerializeField] Vector3 customerStartLocation = new Vector3(-3f, 0.25f, -5f);
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
            print(customers[i]);
        }
        timePassed = 5.0f;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 5.0f)
        {
            //SeatingData.print();
            timePassed = 0f;
        }
        if (SeatingData.waitingCustomers.Count == 0)
        {
            generateCustomer();
        }

        // Glow customer if selected
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Debug.Log(objectHit);

                if (objectHit.tag == "Customer")
                {
                    if (SeatingData.selectedCustomer != null)
                    {
                        removeCustomerGlow(SeatingData.selectedCustomer);
                    }

                    //Renderer renderer = objectHit.gameObject.GetComponent<Renderer>();
                    SeatingData.selectedCustomer = objectHit.gameObject.GetComponent<Customer>();
                    //renderer.material = customer_glow;
                }

                // Select a chair if a customer is selected
                if (objectHit.tag == "Chair" && SeatingData.selectedCustomer != null)
                {
                    Chair chair = objectHit.gameObject.GetComponent<Chair>();
                    Renderer renderer = objectHit.gameObject.GetComponent<Renderer>();
                    if (!chair.seatedCustomer)
                    {
                        SeatingData.selectedChair = chair;
                        renderer.material = chair_glow;

                        seatCustomer();
                    }

                }
            }
        }
    }

    void seatCustomer()
    {
        Chair chair = SeatingData.selectedChair;
        Customer customer = SeatingData.selectedCustomer;
        customer.Seat(chair);
        SeatingData.selectedChair = null;
        SeatingData.selectedCustomer = null;
    }

    public void removeCustomerGlow(Customer c) {
        //Renderer oldRenderer = c.gameObject.GetComponent<Renderer>();
        //oldRenderer.material = c.defaultMaterial;
        //oldRenderer.material = c.defaultMaterial;
    }
    public void removeChairGlow(Chair chair)
    {
        Renderer oldRenderer = chair.GetComponent<Renderer>();
        oldRenderer.material = chair_normal;
    }

    void generateCustomer()
    {
        Customer customer = customers[Random.Range(0, customers.Length)];
        Debug.Log(customer);
        var new_customer = Instantiate(customer, customerStartLocation, Quaternion.identity);
        SeatingData.addWaitingCustomer(new_customer);
        new_customer.Generate();
    }
}
