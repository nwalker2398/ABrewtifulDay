using System.Collections.Generic;
using UnityEngine;
public static class SeatingData
{
    public static List<Customer> waitingCustomers { get; set; }
    public static List<Customer> seatedCustomers { get; set; }
    public static Chair selectedChair { get; set; }
    public static Customer selectedCustomer { get; set; }
    public static GameObject customerArrow { get; set; }

    public static bool showArrow = true;

    public static void addWaitingCustomer(Customer c)
    {
        waitingCustomers.Add(c);
    }

    public static void seatWaitingCustomer(Customer c)
    {
        waitingCustomers.Remove(c);
        seatedCustomers.Add(c);
    }
}
