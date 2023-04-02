using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DailyApartmentsMVC.Models.GuestModel;

public partial class GuestContext : DbContext
{
    public GuestContext()
    {
    }

    public GuestContext(DbContextOptions<GuestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PropertyList> PropertyLists { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("property_list");

            entity.Property(e => e.City)
                .HasColumnType("character varying")
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Flat).HasColumnName("flat");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MinRentalDays).HasColumnName("min_rental_days");
            entity.Property(e => e.PhotoLinks)
                .HasColumnType("character varying[]")
                .HasColumnName("photo_links");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PropertyOwnerId).HasColumnName("property_owner_id");
            entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.SleepingPlaceNumber).HasColumnName("sleeping_place_number");
            entity.Property(e => e.Street)
                .HasColumnType("character varying")
                .HasColumnName("street");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
