using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class PropertyPriceHistory
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public DateOnly ChangeDate { get; set; }

    public decimal NewPrice { get; set; }

    public virtual Property Property { get; set; } = null!;
}
