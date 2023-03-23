using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class CheckIn
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public TimeOnly Time { get; set; }

    public virtual Property Property { get; set; } = null!;
}
