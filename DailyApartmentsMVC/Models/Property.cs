using System;
using System.Collections.Generic;

namespace DailyApartmentsMVC.Models;

public partial class Property
{
    public int Id { get; set; }

    public int PropertyOwnerId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public short RoomNumber { get; set; }

    public short SleepingPlaceNumber { get; set; }

    public string[] PhotoLinks { get; set; } = null!;

    public decimal Price { get; set; }

    public bool Verified { get; set; }

    public bool Active { get; set; }

    public string? Type { get; set; }

    public short? MinRentalDays { get; set; }

    public DateOnly PublicationDate { get; set; }

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public short House { get; set; }

    public short? Flat { get; set; }

    public virtual ICollection<AdditionalTerm> AdditionalTerms { get; } = new List<AdditionalTerm>();

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual ICollection<CheckIn> CheckIns { get; } = new List<CheckIn>();

    public virtual PropertyOwner PropertyOwner { get; set; } = null!;

    public virtual ICollection<PropertyPriceHistory> PropertyPriceHistories { get; } = new List<PropertyPriceHistory>();
}
