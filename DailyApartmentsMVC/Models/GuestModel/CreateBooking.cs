using System.ComponentModel.DataAnnotations;

namespace DailyApartmentsMVC.Models.GuestModel
{
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class CreateBooking
    {
        [Required]
        public int PropertyId { get; set; }

        [Required]
        public string? Date { get; set; }
    }
}
