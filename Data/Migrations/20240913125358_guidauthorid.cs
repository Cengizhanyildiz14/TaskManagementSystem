using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class guidauthorid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Primary Key'i kaldırın
            migrationBuilder.DropPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements");

            // Eski Id ve AuthorId sütunlarını kaldırın
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Announcements");

            // Yeni Id sütununu Guid olarak ekleyin
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Announcements",
                nullable: false,
                defaultValueSql: "NEWID()"); // Her satır için yeni bir GUID üretilir

            // Yeni AuthorId sütununu Guid olarak ekleyin
            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Announcements",
                nullable: false,
                defaultValueSql: "NEWID()");

            // Yeni Primary Key'i ayarlayın
            migrationBuilder.AddPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements",
                column: "Id");
        }

    }
}
