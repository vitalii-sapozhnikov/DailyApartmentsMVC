using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class Message
{
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public string Message1 { get; set; } = null!;

    public virtual Chat TimeNavigation { get; set; } = null!;
}
