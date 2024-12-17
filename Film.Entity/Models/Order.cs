using Film.Entity.Enum.OrderStatus;
using Film.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Entity.Models
{
     public class Order
    {
        public int Id { get; set; }
        public int FilmId { get; set; } // FilmId
        public FilmModel Film { get; set; } = null!; // FilmModel ile ilişki kurduk


        public OrderStatus Status { get; set; } = OrderStatus.Sepette; // Enum tipi
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Sipariş tarihi
        public bool IsDeleted { get; set; }
    }

}
