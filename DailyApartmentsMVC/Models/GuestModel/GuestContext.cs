using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
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
    public DbSet<PropertyDetails> PropertyDetails { get; set; }
    public virtual DbSet<BookingsArchive> BookingsArchives { get; set; }
    public virtual DbSet<ReviewAttribute> ReviewAttributes { get; set; }
    public virtual DbSet<ShowGuestComment> ShowGuestComments { get; set; }
    public virtual DbSet<ShowGuestReview> ShowGuestReviews { get; set; }






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




        modelBuilder
        .Entity<PropertyDetails>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.SleepingPlaceNumber).HasColumnName("sleeping_place_number");
            entity.Property(e => e.PhotoLinks).HasColumnName("photo_links");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Street).HasColumnName("street");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Flat).HasColumnName("flat");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RatingAttribute).HasColumnName("rating_attribute");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.ReviewGuestName).HasColumnName("review_guest_name");
            entity.Property(e => e.ReviewGuestSurname).HasColumnName("review_guest_surname");
            entity.Property(e => e.OwnerName).HasColumnName("owner_name");
            entity.Property(e => e.OwnerSurname).HasColumnName("owner_surname");
            entity.Property(e => e.Deals).HasColumnName("deals");
        });


        modelBuilder.Entity<BookingsArchive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("bookings_archive");

            entity.Property(e => e.City)
                .HasColumnType("character varying")
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.OwnerName)
                .HasColumnType("character varying")
                .HasColumnName("owner_name");
            entity.Property(e => e.OwnerSurname)
                .HasColumnType("character varying")
                .HasColumnName("owner_surname");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Street)
                .HasColumnType("character varying")
                .HasColumnName("street");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<ReviewAttribute>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("review_attributes");

            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ShowGuestComment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("show_guest_comments");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
        });

        modelBuilder.Entity<ShowGuestReview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("show_guest_reviews");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.ReviewAttributeId).HasColumnName("review_attribute_id");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
