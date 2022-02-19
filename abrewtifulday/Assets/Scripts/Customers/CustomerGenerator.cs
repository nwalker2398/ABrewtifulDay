using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    [SerializeField] Vector3 customerStartLocation = new Vector3(-10f, 0.25f, -5f);
    private Customer[] customers;
    private float timePassed;

    void Start()
    {
        CustomerList.waitingCustomers = new List<Customer>();
        CustomerList.seatedCustomers = new List<Customer>();
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
    }

    void generateCustomer()
    {
        Customer customer = customers[Random.Range(0, customers.Length)];
        print("Generating new customer");

        var new_customer = Instantiate(customer, customerStartLocation, Quaternion.identity);
        CustomerList.addWaitingCustomer(new_customer);
        new_customer.Generate();
    }
}
