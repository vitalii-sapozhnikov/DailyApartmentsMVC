using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.GuestModel;

public class BookingsArchive
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateOnly? Date { get; set; }

    public int? Duration { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public short? House { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerSurname { get; set; }
}
