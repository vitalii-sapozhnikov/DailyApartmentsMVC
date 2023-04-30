using DailyApartmentsMVC.Models.ModeratorModel;
using Microsoft.EntityFrameworkCore;

namespace DailyApartmentsMVC.Models.ModeratorModel
{
    public partial class ModeratorContext: DbContext
    {
        public ModeratorContext()
        {
        }

        public ModeratorContext(DbContextOptions<ModeratorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PropertiesForModerator> PropertiesForModerators { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertiesForModerator>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("properties_for_moderator");

                entity.Property(e => e.City)
                    .HasColumnType("character varying")
                    .HasColumnName("city");
                entity.Property(e => e.Country)
                    .HasColumnType("character varying")
                    .HasColumnName("country");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("email");
                entity.Property(e => e.Flat).HasColumnName("flat");
                entity.Property(e => e.House).HasColumnName("house");
                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");
                entity.Property(e => e.PassportId)
                    .HasColumnType("character varying")
                    .HasColumnName("passport_id");
                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("character varying")
                    .HasColumnName("phone_number");
                entity.Property(e => e.PhotoLinks)
                    .HasColumnType("character varying[]")
                    .HasColumnName("photo_links");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.PropertyId).HasColumnName("property_id");
                entity.Property(e => e.PropertyOwnerId).HasColumnName("property_owner_id");
                entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
                entity.Property(e => e.RoomNumber).HasColumnName("room_number");
                entity.Property(e => e.SleepingPlaceNumber).HasColumnName("sleeping_place_number");
                entity.Property(e => e.Street)
                    .HasColumnType("character varying")
                    .HasColumnName("street");
                entity.Property(e => e.Surname)
                    .HasColumnType("character varying")
                    .HasColumnName("surname");
                entity.Property(e => e.TaxNumber)
                    .HasColumnType("character varying")
                    .HasColumnName("tax_number");
                entity.Property(e => e.Title)
                    .HasColumnType("character varying")
                    .HasColumnName("title");
                entity.Property(e => e.Type)
                    .HasColumnType("character varying")
                    .HasColumnName("type");
                entity.Property(e => e.Verified).HasColumnName("verified");
                entity.Property(e => e.CertificateLink).HasColumnName("certificate_link");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
