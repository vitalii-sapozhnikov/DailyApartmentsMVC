using DailyApartmentsMVC.Models.GuestModel;
using DailyApartmentsMVC.Models.ModeratorModel;
using DailyApartmentsMVC.Models.OwnerModel;

namespace DailyApartmentsMVC.AppSettings
{
    public static class AppSettings
    {
        public static GuestContext? guestContext { get; set; }
        public static OwnerContext? ownerContext { get; set; }
        public static ModeratorContext? moderatorContext { get; set; }
    }
}
