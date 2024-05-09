using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class KitapTuru
    {
        [Key] // Primary key Id
        public int Id { get; set;  }
        [Required(ErrorMessage = "Kitap türü Adı boş bırakılamaz!")] //Not null
        [MaxLength(25)]
        [DisplayName("Kitap Türü Adı")]
      public string Ad { get; set; }  
    }
}
