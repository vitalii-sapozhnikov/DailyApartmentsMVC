using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DailyApartmentsMVC.Models;

public partial class AirbnbContext : DbContext
{
    public AirbnbContext()
    {
    }

    public AirbnbContext(DbContextOptions<AirbnbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalTerm> AdditionalTerms { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingsCount> BookingsCounts { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<CheckIn> CheckIns { get; set; }

    public virtual DbSet<ClientReview> ClientReviews { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<OwnerCityBooking> OwnerCityBookings { get; set; }

    public virtual DbSet<OwnersWithLowestBookingAmount> OwnersWithLowestBookingAmounts { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyComment> PropertyComments { get; set; }

    public virtual DbSet<PropertyOwner> PropertyOwners { get; set; }

    public virtual DbSet<PropertyPriceHistory> PropertyPriceHistories { get; set; }

    public virtual DbSet<PropertyReview> PropertyReviews { get; set; }

    public virtual DbSet<ReviewAttribute> ReviewAttributes { get; set; }

    public virtual DbSet<TermsAttribute> TermsAttributes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("additional_terms_pkey");

            entity.ToTable("additional_terms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Attribute).WithMany(p => p.AdditionalTerms)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("additional_terms_attribute_id_fkey");

            entity.HasOne(d => d.Property).WithMany(p => p.AdditionalTerms)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("additional_terms_property_id_fkey");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("booking_pkey");

            entity.ToTable("booking");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Guest).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_guest_id_fkey");

            entity.HasOne(d => d.Property).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_property_id_fkey");
        });

        modelBuilder.Entity<BookingsCount>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("bookings_count");

            entity.Property(e => e.Count).HasColumnName("count");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chat_pkey");

            entity.ToTable("chat");

            entity.HasIndex(e => e.Time, "chat_time_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.PropertyOwnerId).HasColumnName("property_owner_id");
            entity.Property(e => e.Time)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("time");

            entity.HasOne(d => d.Guest).WithMany(p => p.Chats)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chat_guest_id_fkey");

            entity.HasOne(d => d.PropertyOwner).WithMany(p => p.Chats)
                .HasForeignKey(d => d.PropertyOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chat_property_owner_id_fkey");
        });

        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("check_in_pkey");

            entity.ToTable("check_in");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.Property).WithMany(p => p.CheckIns)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("check_in_property_id_fkey");
        });

        modelBuilder.Entity<ClientReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_review_pkey");

            entity.ToTable("client_review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.Rate).HasColumnName("rate");

            entity.HasOne(d => d.Booking).WithMany(p => p.ClientReviews)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("client_review_booking_id_fkey");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guest_pkey");

            entity.ToTable("guest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.SuccessfulDeals).HasColumnName("successful_deals");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_pkey");

            entity.ToTable("message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Message1)
                .HasColumnType("character varying")
                .HasColumnName("message");
            entity.Property(e => e.Time)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("time");

            entity.HasOne(d => d.TimeNavigation).WithMany(p => p.Messages)
                .HasPrincipalKey(p => p.Time)
                .HasForeignKey(d => d.Time)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("message_time_fkey");
        });

        modelBuilder.Entity<OwnerCityBooking>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("owner_city_bookings");

            entity.Property(e => e.City)
                .HasColumnType("character varying")
                .HasColumnName("city");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");
        });

        modelBuilder.Entity<OwnersWithLowestBookingAmount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("owners_with_lowest_booking_amount");

            entity.Property(e => e.City)
                .HasColumnType("character varying")
                .HasColumnName("city");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("property_pkey");

            entity.ToTable("property");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.City)
                .HasColumnType("character varying")
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Flat).HasColumnName("flat");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.MinRentalDays).HasColumnName("min_rental_days");
            entity.Property(e => e.PhotoLinks)
                .HasColumnType("character varying[]")
                .HasColumnName("photo_links");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PropertyOwnerId).HasColumnName("property_owner_id");
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

            entity.HasOne(d => d.PropertyOwner).WithMany(p => p.Properties)
                .HasForeignKey(d => d.PropertyOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("property_property_owner_id_fkey");
        });

        modelBuilder.Entity<PropertyComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("proprety_comment_pkey");

            entity.ToTable("property_comment");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('proprety_comment_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");

            entity.HasOne(d => d.Booking).WithMany(p => p.PropertyComments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("proprety_comment_booking_id_fkey");
        });

        modelBuilder.Entity<PropertyOwner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("property_owner_pkey");

            entity.ToTable("property_owner");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PassportId)
                .HasColumnType("character varying")
                .HasColumnName("passport_id");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");
            entity.Property(e => e.TaxNumber)
                .HasColumnType("character varying")
                .HasColumnName("tax_number");
        });

        modelBuilder.Entity<PropertyPriceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("property_price_history_pkey");

            entity.ToTable("property_price_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChangeDate).HasColumnName("change_date");
            entity.Property(e => e.NewPrice).HasColumnName("new_price");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyPriceHistories)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("property_price_history_property_id_fkey");
        });

        modelBuilder.Entity<PropertyReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("proprety_review_pkey");

            entity.ToTable("property_review");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('proprety_review_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.ReviewAttributeId).HasColumnName("review_attribute_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Booking).WithMany(p => p.PropertyReviews)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("proprety_review_booking_id_fkey");

            entity.HasOne(d => d.ReviewAttribute).WithMany(p => p.PropertyReviews)
                .HasForeignKey(d => d.ReviewAttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("proprety_review_review_attribute_id_fkey");
        });

        modelBuilder.Entity<ReviewAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("review_attribute_pkey");

            entity.ToTable("review_attribute");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
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
