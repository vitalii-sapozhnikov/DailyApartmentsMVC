using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class ReviewAttribute
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PropertyReview> PropertyReviews { get; } = new List<PropertyReview>();
}
