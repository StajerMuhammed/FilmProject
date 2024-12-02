using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Film.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tür alanı zorunludur.")]
        public  string Tür { get; set; } = null!;
        public bool IsDeleted { get; set; }

    }
}
