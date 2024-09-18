using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class announcementGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // İlk olarak mevcut primary key'i kaldırın
            migrationBuilder.DropPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements");

            // Eski Id sütununu silin
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Announcements");

            // Yeni Id sütununu Guid tipinde ekleyin
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Announcements",
                nullable: false,
                defaultValueSql: "NEWID()");  // Her satır için yeni bir GUID üretilecek

            // Yeni primary key'i oluşturun
            migrationBuilder.AddPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements",
                column: "Id");
        }


    }
}
