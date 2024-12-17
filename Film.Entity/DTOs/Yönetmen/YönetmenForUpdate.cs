using Film.Models;

namespace Film.DTOs.Yönetmen
{
    public class YönetmenForUpdate
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime BirtDay { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<int>? FilmlerId { get; set; } 
    }
}
