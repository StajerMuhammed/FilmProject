using Film.Entity.Enum.OrderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Entity.DTOs.Order
{
    public class OrderForUpdate
    {
        public int Id { get; set; } // Sipariş ID
        public bool IsDeleted { get; set; }
    }
}
