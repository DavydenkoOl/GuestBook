using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
    public class Messages
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(500, MinimumLength = 20, ErrorMessage = "Длина строки должна быть от 20 до 500 символов")]
        public string? Message { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? Id_user { get; set; }

        public Users? Owner { get; set; }


    }
}
