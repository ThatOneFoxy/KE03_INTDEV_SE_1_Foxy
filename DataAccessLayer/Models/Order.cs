using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public List<OrderLine> OrderLines { get; set; } = new();
    }

}
