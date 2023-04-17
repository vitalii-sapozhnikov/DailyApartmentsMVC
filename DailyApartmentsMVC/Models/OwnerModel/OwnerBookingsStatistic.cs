using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.OwnerModel;

public partial class OwnerBookingsStatistic
{
    public int? PropertyId { get; set; }

    public string? Title { get; set; }

    public int? BookingId { get; set; }

    public int? GuestId { get; set; }

    public DateOnly? Date { get; set; }

    public bool? Status { get; set; }

    public int? Duration { get; set; }

    public decimal? CurrentPrice { get; set; }
}

