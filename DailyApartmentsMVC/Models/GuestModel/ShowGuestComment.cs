using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.GuestModel;

public partial class ShowGuestComment
{
    public int? BookingId { get; set; }

    public string? Comment { get; set; }
}
