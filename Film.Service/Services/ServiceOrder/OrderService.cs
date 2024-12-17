using Film.Datas;
using Film.DTOs.Movies;
using Film.Entity.DTOs.Order;
using Film.Entity.Enum.OrderStatus;
using Film.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Service.Services.ServiceOrder
{
    public class OrderService : IOrderService
    {
        private readonly SampleDBContext _context;

        public OrderService(SampleDBContext context)
        {
            _context = context;
        }
        public Order CreateOrder(OrderForInsertion orderForInsertion)
        {
            // FilmId'ye göre ilgili Film nesnesini al
            var film =  _context.Films
                .Include(x => x.Kategori)
                .ThenInclude(y => y.Films)
                .Include(x => x.Yönetmen)
                .ThenInclude(x => x.Filmler)
                .FirstOrDefault();

            if (film == null)
            {
                throw new Exception("Film bulunamadı");
            }

            // Film nesnesini Order nesnesine dahil et
            var order = new Order
            {
                FilmId = orderForInsertion.FilmId,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                Film = film,
                Status = OrderStatus.Sepette
            };
            

            // Order'ı kaydet
            _context.Orders.Add(order);
            _context.SaveChanges();

            return order; // order nesnesini geri döndür

        }

        public void DeleteOrder(int id)
        {
            var order = GetOrderById(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"OrderID {id} bulunamadı."); // Hata kontrolü
            }

            order.IsDeleted = true; // Silinmiş olarak işaretle

            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        public IEnumerable<Order> GetAllOrders()
        {
            var order = _context.Orders
                .Include(x =>x.Film)
                .ThenInclude(y => y.Yönetmen)
                .Include(x => x.Film)
                .ThenInclude(y => y.Kategori)
                .Where(c => !c.IsDeleted)
                .ToList();
            return order;
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders
                .Include(x => x.Film)
                .ThenInclude(y => y.Yönetmen)
                .Include(x => x.Film)
                .ThenInclude(y => y.Kategori)
                .Where(c => !c.IsDeleted)
                .FirstOrDefault(x => x.Id == id);
       //     var order = _context.Orders
       //.Include(film => order.Film)  // Film'i dahil et
       //.ThenInclude(category =)
       //.FirstOrDefault(order => order.Id == id && !order.IsDeleted);  // Silinmemiş Order'ı bul

            if (order == null || order.Film == null)
            {
                throw new Exception("Order veya ilişkili veriler bulunamadı");
            }

            return order;  // Order nesnesini geri döndür

        }

        public Order UpdateOrder(OrderForUpdate orderForUpdate)
        {
            // Mevcut Order'ı getir
            var existingOrder = GetOrderById(orderForUpdate.Id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order ID {orderForUpdate.Id} bulunamadı.");
            }

            // 'IsDeleted' durumunu güncelle
            existingOrder.IsDeleted = orderForUpdate.IsDeleted;

            // Eğer sipariş silindiyse, durumu 'IptalEdildi' olarak değiştir
            if (existingOrder.IsDeleted)
            {
                existingOrder.Status = OrderStatus.IptalEdildi;
            }

            _context.SaveChanges();

            return existingOrder;
        }
    }
}
