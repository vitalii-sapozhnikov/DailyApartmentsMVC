using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DailyApartmentsMVC.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookings_count",
                columns: table => new
                {
                    count = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "guest",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    surname = table.Column<string>(type: "character varying", nullable: false),
                    age = table.Column<short>(type: "smallint", nullable: false),
                    successful_deals = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("guest_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "property_owner",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    surname = table.Column<string>(type: "character varying", nullable: false),
                    email = table.Column<string>(type: "character varying", nullable: false),
                    phone_number = table.Column<string>(type: "character varying", nullable: false),
                    passport_id = table.Column<string>(type: "character varying", nullable: false),
                    tax_number = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("property_owner_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "review_attribute",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("review_attribute_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "terms_attribute",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("terms_attribute_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_owner_id = table.Column<int>(type: "integer", nullable: false),
                    guest_id = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("chat_pkey", x => x.id);
                    table.UniqueConstraint("AK_chat_time", x => x.time);
                    table.ForeignKey(
                        name: "chat_guest_id_fkey",
                        column: x => x.guest_id,
                        principalTable: "guest",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "chat_property_owner_id_fkey",
                        column: x => x.property_owner_id,
                        principalTable: "property_owner",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "property",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_owner_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    room_number = table.Column<short>(type: "smallint", nullable: false),
                    sleeping_place_number = table.Column<short>(type: "smallint", nullable: false),
                    photo_links = table.Column<string[]>(type: "character varying[]", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<string>(type: "character varying", nullable: true),
                    min_rental_days = table.Column<short>(type: "smallint", nullable: true),
                    publication_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("property_pkey", x => x.id);
                    table.ForeignKey(
                        name: "property_property_owner_id_fkey",
                        column: x => x.property_owner_id,
                        principalTable: "property_owner",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    message = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pkey", x => x.id);
                    table.ForeignKey(
                        name: "message_time_fkey",
                        column: x => x.time,
                        principalTable: "chat",
                        principalColumn: "time");
                });

            migrationBuilder.CreateTable(
                name: "additional_terms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_id = table.Column<int>(type: "integer", nullable: false),
                    attribute_id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("additional_terms_pkey", x => x.id);
                    table.ForeignKey(
                        name: "additional_terms_attribute_id_fkey",
                        column: x => x.attribute_id,
                        principalTable: "terms_attribute",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "additional_terms_property_id_fkey",
                        column: x => x.property_id,
                        principalTable: "property",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_id = table.Column<int>(type: "integer", nullable: false),
                    guest_id = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("booking_pkey", x => x.id);
                    table.ForeignKey(
                        name: "booking_guest_id_fkey",
                        column: x => x.guest_id,
                        principalTable: "guest",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "booking_property_id_fkey",
                        column: x => x.property_id,
                        principalTable: "property",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "check_in",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_id = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("check_in_pkey", x => x.id);
                    table.ForeignKey(
                        name: "check_in_property_id_fkey",
                        column: x => x.property_id,
                        principalTable: "property",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "property_price_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_id = table.Column<int>(type: "integer", nullable: false),
                    change_date = table.Column<DateOnly>(type: "date", nullable: false),
                    new_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("property_price_history_pkey", x => x.id);
                    table.ForeignKey(
                        name: "property_price_history_property_id_fkey",
                        column: x => x.property_id,
                        principalTable: "property",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "client_review",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    rate = table.Column<short>(type: "smallint", nullable: false),
                    comment = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("client_review_pkey", x => x.id);
                    table.ForeignKey(
                        name: "client_review_booking_id_fkey",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "property_comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('proprety_comment_id_seq'::regclass)"),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("proprety_comment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "proprety_comment_booking_id_fkey",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "property_review",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('proprety_review_id_seq'::regclass)"),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    review_attribute_id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("proprety_review_pkey", x => x.id);
                    table.ForeignKey(
                        name: "proprety_review_booking_id_fkey",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "proprety_review_review_attribute_id_fkey",
                        column: x => x.review_attribute_id,
                        principalTable: "review_attribute",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_additional_terms_attribute_id",
                table: "additional_terms",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_additional_terms_property_id",
                table: "additional_terms",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_guest_id",
                table: "booking",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_property_id",
                table: "booking",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "chat_time_key",
                table: "chat",
                column: "time",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_guest_id",
                table: "chat",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_property_owner_id",
                table: "chat",
                column: "property_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_check_in_property_id",
                table: "check_in",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_client_review_booking_id",
                table: "client_review",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_time",
                table: "message",
                column: "time");

            migrationBuilder.CreateIndex(
                name: "IX_property_property_owner_id",
                table: "property",
                column: "property_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_property_comment_booking_id",
                table: "property_comment",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_property_price_history_property_id",
                table: "property_price_history",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_property_review_booking_id",
                table: "property_review",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_property_review_review_attribute_id",
                table: "property_review",
                column: "review_attribute_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "additional_terms");

            migrationBuilder.DropTable(
                name: "bookings_count");

            migrationBuilder.DropTable(
                name: "check_in");

            migrationBuilder.DropTable(
                name: "client_review");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "property_comment");

            migrationBuilder.DropTable(
                name: "property_price_history");

            migrationBuilder.DropTable(
                name: "property_review");

            migrationBuilder.DropTable(
                name: "terms_attribute");

            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "review_attribute");

            migrationBuilder.DropTable(
                name: "guest");

            migrationBuilder.DropTable(
                name: "property");

            migrationBuilder.DropTable(
                name: "property_owner");
        }
    }
}
