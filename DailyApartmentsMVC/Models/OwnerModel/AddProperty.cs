using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DailyApartmentsMVC.Models.OwnerModel
{
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class AddProperty
    {
        [Required]
        public string? Title { get; set; }


        [Required]
        public string? Description { get; set; }


        [Required]
        [Range(1, 100, ErrorMessage = "Номер кімнати має бути від 1 до 100")]
        public short RoomNumber { get; set; }


        [Required]
        [Range(1, 100, ErrorMessage = "Кількість спальних місць має бути від 1 до 100")]
        public short SleepingPlaceNumber { get; set; }


        [Required]
        [Display(Name = "Оберіть фотографії")]
        public IFormFile[]? Photo { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Назва країни може містити лише літери")]
        public string Country { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Назва міста може містити лише літери")]

        public string City { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Назва вулиці може містити лише літери")]
        public string Street { get; set; }

        [Required]
        public short House { get; set; }


        public string Type { get; set; }
        public int MinRentalDays { get; set; }
    }
}
