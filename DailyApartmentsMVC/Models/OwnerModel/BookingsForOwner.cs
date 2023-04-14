using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.OwnerModel;

public partial class BookingsForOwner
{
    public int? PropertyId { get; set; }

    public string? Title { get; set; }

    public decimal? Price { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public short? House { get; set; }

    public short? Flat { get; set; }

    public int?[]? Bookingids { get; set; }

    public DateOnly?[]? Dates { get; set; }

    public bool?[]? Statuses { get; set; }

    public int?[]? Durations { get; set; }

    public string?[]? Guestnames { get; set; }

    public string?[]? Guestemails { get; set; }
}
