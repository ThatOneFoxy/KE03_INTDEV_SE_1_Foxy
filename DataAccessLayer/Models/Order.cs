using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public string DeliveryMethod { get; set; } = "Onbekend";
        public decimal ShippingCost { get; set; }
        public string PaymentMethod { get; set; } = "Onbekend";

        public string? ShippingAddress { get; set; }

        public string CustomerName { get; set; }
        public string OrderStatus { get; set; } = "In behandeling";

        public List<OrderLine> OrderLines { get; set; } = new();
    }
}
