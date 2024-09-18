using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class allGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Task");
            migrationBuilder.DropTable(name: "Users");
            migrationBuilder.DropTable(name: "Announcements");
            migrationBuilder.DropTable(name: "Departments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eğer rollback (geri alma) işlemi yapmak isterseniz, tabloları tekrar oluşturabilirsiniz
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            // Diğer tabloları da benzer şekilde ekleyebilirsiniz
        }

    }
}
