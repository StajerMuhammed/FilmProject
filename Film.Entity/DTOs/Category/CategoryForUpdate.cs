using System.ComponentModel.DataAnnotations;

namespace Film.DTOs.Category
{
    public class CategoryForUpdate
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tür alanı zorunludur.")]
        public string Tür { get; set; }
        public bool IsDeleted { get; set; }
    }
}
