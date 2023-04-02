using System.ComponentModel.DataAnnotations;

namespace DailyApartmentsMVC.Models.RegistrationModel
{
    public class GuestRegistration
    {
        public string? UserName { get; set; }


        [EmailAddress(ErrorMessage = "Введіть справжню електронну адресу")]
        public string? Email { get; set; }


        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
