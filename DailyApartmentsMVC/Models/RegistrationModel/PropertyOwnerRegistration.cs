using System.ComponentModel.DataAnnotations;

namespace DailyApartmentsMVC.Models.RegistrationModel
{
    public class PropertyOwnerRegistration
    {
        public string? OwnerUserName { get; set; }


        [EmailAddress(ErrorMessage = "Введіть справжню електронну адресу")]
        public string? OwnerEmail { get; set; }

        [DataType(DataType.Password)]
        public string? OwnerPassword { get; set; }

        public string? OwnerName { get; set; }

        public string? OwnerSurname { get; set; }

        public string? OwnerPassportID { get; set;}

        public string? OwnerTaxNumber { get; set; }

        public string? OwnerPhoneNumber { get; set; }
    }
}
