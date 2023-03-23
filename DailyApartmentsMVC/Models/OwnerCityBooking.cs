using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class OwnerCityBooking
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? City { get; set; }

    public long? Count { get; set; }
}
