using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery.Orders.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippingZipCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZipCodeBegin = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true),
                    ZipCodeEnd = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(8,4)", precision: 8, scale: 4, nullable: false),
                    ExpireDateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireDateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingZipCodes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ShippingZipCodes",
                columns: new[] { "Id", "ZipCodeBegin", "ZipCodeEnd", "Price", "ExpireDateStart", "ExpireDateEnd", "RegisterDate" },
                values: new object[,]
                {
                                { Guid.Parse("8132b9df-d14b-45ae-8b7f-9fe7ecaef8a0"), "20000000", "23799999", 10.00m, DateTime.Now, DateTime.MaxValue, DateTime.Now },
                                { Guid.Parse("b6e9987a-d4c5-48a6-82fd-a470a135dbfa"), "20000000", "28999999", 20.00m, DateTime.Now, DateTime.MaxValue, DateTime.Now },
                                { Guid.Parse("a6f734da-f964-41f7-b2c5-1f71f0baba47"), "10000000", "99999999", 40.00m, DateTime.Now, DateTime.MaxValue, DateTime.Now},
            });


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingZipCodes");
        }
    }
}
