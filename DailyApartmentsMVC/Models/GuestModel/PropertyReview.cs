using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models.GuestModel;

public partial class PropertyReview
{
    public int? BookingId { get; set; }

    public int? ReviewAttributeId { get; set; }

    public short? Value { get; set; }
}
