using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "AccountProfile",
                newName: "user_id");

            migrationBuilder.AddColumn<string>(
                name: "create_by",
                table: "AccountProfile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "create_date",
                table: "AccountProfile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "update_by",
                table: "AccountProfile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "update_date",
                table: "AccountProfile",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "create_by",
                table: "AccountProfile");

            migrationBuilder.DropColumn(
                name: "create_date",
                table: "AccountProfile");

            migrationBuilder.DropColumn(
                name: "update_by",
                table: "AccountProfile");

            migrationBuilder.DropColumn(
                name: "update_date",
                table: "AccountProfile");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AccountProfile",
                newName: "account_id");
        }
    }
}
