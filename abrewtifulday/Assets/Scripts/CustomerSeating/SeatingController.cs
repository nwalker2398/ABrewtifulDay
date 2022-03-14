using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

public class SeatingController : MonoBehaviour
{
    [SerializeField] Vector3 customerStartLocation = new Vector3(-7f, 0.25f, -4f);
    [SerializeField] Material chair_glow, chair_normal;

    private Customer[] customers;
    private float timePassed;
    private float generateCustomerIn;
    private Camera camera;
    private float nextWave;
    private float waveDuration;

    void Start()
    {
        SeatingData.waitingCustomers = new List<Customer>();
        SeatingData.seatedCustomers = new List<Customer>();
        GameObject[] customerObjects = GameObject.FindGameObjectsWithTag("Customer");
        camera = Camera.main;
        customers = new Customer[customerObjects.Length];

        for (int i = 0; i < customerObjects.Length; i++)
        {
            customers[i] = customerObjects[i].GetComponent<Customer>();
        }
        timePassed = 5.0f;

        LevelController.LC.printLevel(LevelController.LC.currentLevel);
        generateCustomerIn = 0f;
        // UPDATE TO THE LEVEL DURATION
        float gameDuration = 120f;
        int numWaves = (int)LevelController.LC.currentLevelData["NumWaves"] + 1;
        waveDuration = (gameDuration / numWaves) + 1;
        nextWave = Time.time + waveDuration;
    }

    void Update()
    {
        if (GameController.GC.paused || GameController.GC.stopped)
        {
            timePassed = 0f;
            nextWave += Time.deltaTime;
            return;
        }

        timePassed += Time.deltaTime;
        if (Time.time > nextWave)
        {
            timePassed = 0f;
            nextWave += waveDuration;
            // print(LevelController.LC);
            int numCustomers = (int)LevelController.LC.currentLevelData["CustomersPerWave"];
            for (int i = 0; i < numCustomers; i++)
            {
                Vector3 location = customerStartLocation + new Vector3(-i*2, 0, i*2);
                generateCustomer(location);
            }
        }
        if (timePassed > generateCustomerIn && SeatingData.waitingCustomers.Count < 5)
        {
            timePassed = 0f;
            float startGeneration = (float)LevelController.LC.currentLevelData["GenerateCustomerIn"];
            generateCustomerIn = Random.Range(startGeneration, startGeneration + 4);
            generateCustomer(customerStartLocation);
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
                        // switching customer selected, remove the glow on currently selected customer
                        SeatingData.selectedCustomer.GetComponent<HighlightEffect>().SetHighlighted(false);
                        removeCustomerGlow(SeatingData.selectedCustomer);
                    }

                    //selected a customer from the waiting area
                    //Renderer renderer = objectHit.gameObject.GetComponent<Renderer>();
                    Customer c = objectHit.gameObject.GetComponent<Customer>();

                    // make customer glow after selection if they're in the waiting area
                    if (c.toWaitingArea || c.atWaitingArea)
                    {
                        print("Glowing customer");
                        SeatingData.selectedCustomer = c;
                        c.toWaitingArea = false;
                        c.atWaitingArea = true;
                        c.GetComponent<HighlightEffect>().SetHighlighted(true);
                    }

                    /*if (SeatingData.showArrow)
                    {
                        removeArrow(true);
                        // Can use arrow logic in tutorial scene, commenting out now!
                        // addArrow(tutorialChair.transform.position);
                    }*/
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

                        //removeArrow(false);
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
        // remove glow from customer
        customer.GetComponent<HighlightEffect>().SetHighlighted(false);
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

    /*public void addArrow(Vector3 pos)
    {
        if (SeatingData.showArrow)
        {
            var arrow = GameObject.FindGameObjectWithTag("Arrow");
            //make new arrow in area 
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
    }*/

    void generateCustomer(Vector3 location)
    {
        Customer customer = customers[Random.Range(0, customers.Length)];
        var new_customer = Instantiate(customer, location, Quaternion.identity);
        SeatingData.addWaitingCustomer(new_customer);
        new_customer.Generate();
    }
}
