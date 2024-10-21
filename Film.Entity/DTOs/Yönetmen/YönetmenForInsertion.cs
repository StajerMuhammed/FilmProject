using Film.Models;

namespace Film.DTOs.Yönetmen
{
    public class YönetmenForInsertion
    {
        public string Name { get; set; }
        public int BirtDay { get; set; }

        public virtual ICollection<int> FilmlerId { get; set; }
    }
}
