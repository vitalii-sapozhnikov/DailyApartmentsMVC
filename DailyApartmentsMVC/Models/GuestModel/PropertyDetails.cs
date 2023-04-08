namespace DailyApartmentsMVC.Models.GuestModel
{
    public class PropertyDetails
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public short RoomNumber { get; set; }
        public short SleepingPlaceNumber { get; set; }
        public string[]? PhotoLinks { get; set; }
        public decimal Price { get; set; }
        public string? Type { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public short House { get; set; }
        public short Flat { get; set; }
        public short? Rating { get; set; }
        public string? RatingAttribute { get; set; }
        public string? Comment { get; set; }
        public string? ReviewGuestName { get; set; }
        public string? ReviewGuestSurname { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerSurname { get; set; }
    }

}
