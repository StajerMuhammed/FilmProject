using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Entity.Enum.OrderStatus
{
    public enum OrderStatus
    {
        Sepette = 1,     // Sipariş Sepette
        TeslimEdildi = 2,   // Sipariş Onaylandı
        IptalEdildi = 3, // Sipariş İptal Edildi
    }
}
