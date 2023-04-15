using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.GuestModel;

public partial class PropertyComment
{
    public int? BookingId { get; set; }

    public string? Comment { get; set; }
}
