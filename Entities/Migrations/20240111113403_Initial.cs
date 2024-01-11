using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PhoneNummber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNummber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contacts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "PhoneNummber", "UserName" },
                values: new object[,]
                {
                    { 1, "kristijan@gmail.com", "+38177733", "KristijanMn" },
                    { 2, "Markom@gmail.com", "+38172637781", "markoM" },
                    { 3, "simasdm@gmail.com", "+38112345", "Simasd" }
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "ContactId", "ContactName", "PhoneNummber", "UserID" },
                values: new object[,]
                {
                    { 1, "Marko Maric", "+38172637781", 1 },
                    { 2, "Pera Peric", "+3899999", 1 },
                    { 3, "Sima Simic", "+38112345", 1 },
                    { 4, "Marko Maric", "+38172637781", 2 },
                    { 5, "Marko Maric", "+38172637781", 3 },
                    { 6, "Pera Peric", "+3899999", 2 },
                    { 7, "Sima Simic", "+38112345", 2 },
                    { 8, "Tosa Tosic", "+9991111", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserID",
                table: "Contacts",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
