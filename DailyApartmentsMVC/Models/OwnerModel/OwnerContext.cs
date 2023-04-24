using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DailyApartmentsMVC.Models.OwnerModel;

public partial class OwnerContext : DbContext
{
    public OwnerContext()
    {
    }

    public OwnerContext(DbContextOptions<OwnerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PropertyListOwner> PropertyListOwners { get; set; }
    public virtual DbSet<BookingsForOwner> BookingsForOwners { get; set; }
    public virtual DbSet<MyRatesAndCommentsForOwner> MyRatesAndCommentsForOwners { get; set; }
    public virtual DbSet<OwnerBookingsStatistic> OwnerBookingsStatistics { get; set; }
    public virtual DbSet<TermsAttribute> TermsAttributes { get; set; }
    public DbSet<MonthlyIncome> MonthlyIncomes { get; set; }
    public DbSet<MonthlyBooking> MonthlyBookings { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyListOwner>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("property_list_owner");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.BookingsCount).HasColumnName("bookings_count");
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
            entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
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
            entity.Property(e => e.Verified).HasColumnName("verified");
        });

        modelBuilder.Entity<BookingsForOwner>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("bookings_for_owner");

            entity.Property(e => e.Bookingids).HasColumnName("bookingids");
            entity.Property(e => e.City)
                .HasColumnType("character varying")
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");
            entity.Property(e => e.Dates).HasColumnName("dates");
            entity.Property(e => e.Durations).HasColumnName("durations");
            entity.Property(e => e.Flat).HasColumnName("flat");
            entity.Property(e => e.Guestemails)
                .HasColumnType("character varying[]")
                .HasColumnName("guestemails");
            entity.Property(e => e.Guestnames).HasColumnName("guestnames");
            entity.Property(e => e.GuestReviews).HasColumnName("guest_rates");
            entity.Property(e => e.GuestComments).HasColumnName("guest_comments");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Statuses).HasColumnName("statuses");
            entity.Property(e => e.Street)
                .HasColumnType("character varying")
                .HasColumnName("street");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<MyRatesAndCommentsForOwner>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("my_rates_and_comments_for_owner");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.Rate).HasColumnName("rate");
        });

        modelBuilder.Entity<OwnerBookingsStatistic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("owner_bookings_statistics");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.CurrentPrice).HasColumnName("current_price");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<MonthlyIncome>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.MonthIndex).HasColumnName("month");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<MonthlyBooking>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.MonthIndex).HasColumnName("month");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Year).HasColumnName("year");
        });


        modelBuilder.Entity<TermsAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("terms_attribute_pkey");

            entity.ToTable("terms_attribute");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);


    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
