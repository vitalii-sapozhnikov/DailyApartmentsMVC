using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class Guest
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public short Age { get; set; }

    public short SuccessfulDeals { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual ICollection<Chat> Chats { get; } = new List<Chat>();
}
