using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.OwnerModel;

public partial class MyRatesAndCommentsForOwner
{
    public int? BookingId { get; set; }

    public short? Rate { get; set; }

    public string? Comment { get; set; }
}
