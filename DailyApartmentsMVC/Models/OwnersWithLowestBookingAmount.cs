using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class OwnersWithLowestBookingAmount
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? City { get; set; }

    public long? Count { get; set; }
}
