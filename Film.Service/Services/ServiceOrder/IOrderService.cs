using Film.Entity.DTOs.Order;
using Film.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Service.Services.ServiceOrder
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders(); // Tüm Order DTO'larını getir
        Order GetOrderById(int id); // ID'ye göre Order Getir
        Order CreateOrder(OrderForInsertion Order); // Yeni ORder oluşturur
        Order UpdateOrder(OrderForUpdate Order); // Order güncelle
        void DeleteOrder(int id); // Order sil
    }
}
