using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingController : MonoBehaviour
{
    [SerializeField] Vector3 customerStartLocation = new Vector3(-7f, 0.25f, -4f);
    [SerializeField] Material customer_glow, chair_glow, chair_normal;
    [SerializeField] GameObject tutorialChair;
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
        timePassed = 5.0f;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 5.0f)
        {
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

                if (objectHit.tag == "Customer")
                {
                    if (SeatingData.selectedCustomer != null)
                    {
                        removeCustomerGlow(SeatingData.selectedCustomer);
                    }

                    //Renderer renderer = objectHit.gameObject.GetComponent<Renderer>();
                    SeatingData.selectedCustomer = objectHit.gameObject.GetComponent<Customer>();
                    if (SeatingData.showArrow)
                    {
                        removeArrow(true);
                        addArrow(tutorialChair.transform.position);
                    }
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

                        removeArrow(false);
                        print("Seating customer");
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

    public void addArrow(Vector3 pos)
    {
        if (SeatingData.showArrow)
        {
            var arrow = GameObject.FindGameObjectWithTag("Arrow");
            pos.y = arrow.GetComponent<ArrowController>().calcY();
            var new_arrow = Instantiate(arrow, pos, Quaternion.identity);
            SeatingData.customerArrow = new_arrow;
        }
    }

    public void removeArrow(bool showAgain)
    {
        if (SeatingData.showArrow)
        {
            Destroy(SeatingData.customerArrow);
            SeatingData.customerArrow = null;
            SeatingData.showArrow = showAgain;
        }
    }

    void generateCustomer()
    {
        Customer customer = customers[Random.Range(0, customers.Length)];
        var new_customer = Instantiate(customer, customerStartLocation, Quaternion.identity);
        SeatingData.addWaitingCustomer(new_customer);
        new_customer.Generate();
    }
}
