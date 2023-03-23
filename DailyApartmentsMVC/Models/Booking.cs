using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public int GuestId { get; set; }

    public DateOnly Date { get; set; }

    public bool Status { get; set; }

    public int? Duration { get; set; }

    public virtual ICollection<ClientReview> ClientReviews { get; } = new List<ClientReview>();

    public virtual Guest Guest { get; set; } = null!;

    public virtual Property Property { get; set; } = null!;

    public virtual ICollection<PropertyComment> PropertyComments { get; } = new List<PropertyComment>();

    public virtual ICollection<PropertyReview> PropertyReviews { get; } = new List<PropertyReview>();
}
