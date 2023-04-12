using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.OwnerModel;

public partial class PropertyListOwner
{
    public int? Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public short? RoomNumber { get; set; }

    public short? SleepingPlaceNumber { get; set; }

    public string[]? PhotoLinks { get; set; }

    public decimal? Price { get; set; }

    public bool? Verified { get; set; }

    public bool? Active { get; set; }

    public string? Type { get; set; }

    public short? MinRentalDays { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public short? House { get; set; }

    public short? Flat { get; set; }

    public long? BookingsCount { get; set; }
}
