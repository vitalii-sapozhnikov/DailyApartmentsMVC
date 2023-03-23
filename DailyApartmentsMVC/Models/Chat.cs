using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int PropertyOwnerId { get; set; }

    public int GuestId { get; set; }

    public DateTime Time { get; set; }

    public virtual Guest Guest { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; } = new List<Message>();

    public virtual PropertyOwner PropertyOwner { get; set; } = null!;
}
