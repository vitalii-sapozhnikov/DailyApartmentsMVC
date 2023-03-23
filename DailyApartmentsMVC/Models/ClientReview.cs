using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class ClientReview
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public short Rate { get; set; }

    public string Comment { get; set; } = null!;

    public virtual Booking Booking { get; set; } = null!;
}
