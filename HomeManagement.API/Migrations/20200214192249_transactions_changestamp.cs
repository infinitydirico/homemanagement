using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeManagement.API.Migrations
{
    public partial class transactions_changestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeStamp",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeStamp",
                table: "Transactions");
        }
    }
}
