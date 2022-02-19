
using System;
using System.Collections.Generic;
public static class CustomerList
{
    public static List<Customer> waitingCustomers { get; set; }
    public static List<Customer> seatedCustomers { get; set; }
       
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
