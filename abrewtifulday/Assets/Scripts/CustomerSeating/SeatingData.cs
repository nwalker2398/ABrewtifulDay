
using System;
using System.Collections.Generic;
using UnityEngine;
public static class SeatingData
{
    public static List<Customer> waitingCustomers { get; set; }
    public static List<Customer> seatedCustomers { get; set; }
    public static Chair selectedChair { get; set; }
    public static Customer selectedCustomer { get; set; }

    public static void addWaitingCustomer(Customer c)
    {
        waitingCustomers.Add(c);
    }

    public static void seatWaitingCustomer(Customer c)
    {
        waitingCustomers.Remove(c);
        seatedCustomers.Add(c);
    }

    public static void print()
    {
        Debug.Log(waitingCustomers);
        Debug.Log(seatedCustomers);
        Debug.Log(selectedCustomer);
        Debug.Log(selectedChair);
    }
}
