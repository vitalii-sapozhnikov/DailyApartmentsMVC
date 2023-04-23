using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.ModeratorModel;

public partial class PropertiesForModerator
{
    public int? PropertyId { get; set; }

    public int? PropertyOwnerId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public short? RoomNumber { get; set; }

    public short? SleepingPlaceNumber { get; set; }

    public string[]? PhotoLinks { get; set; }

    public decimal? Price { get; set; }

    public bool? Verified { get; set; }

    public string? Type { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public short? House { get; set; }

    public short? Flat { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PassportId { get; set; }

    public string? TaxNumber { get; set; }
}
