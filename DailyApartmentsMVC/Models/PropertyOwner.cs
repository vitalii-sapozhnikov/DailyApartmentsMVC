using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class PropertyOwner
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string PassportId { get; set; } = null!;

    public string TaxNumber { get; set; } = null!;

    public virtual ICollection<Chat> Chats { get; } = new List<Chat>();

    public virtual ICollection<Property> Properties { get; } = new List<Property>();
}
