using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class PropertyReview
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int ReviewAttributeId { get; set; }

    public short? Value { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual ReviewAttribute ReviewAttribute { get; set; } = null!;
}
