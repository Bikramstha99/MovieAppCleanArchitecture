using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewTwoFactorOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TwoFactorOTP",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "ExpirationDateTime",
                table: "TwoFactorOTP",
                newName: "GeneratedDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDateTime",
                table: "TwoFactorOTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredDateTime",
                table: "TwoFactorOTP");

            migrationBuilder.RenameColumn(
                name: "GeneratedDateTime",
                table: "TwoFactorOTP",
                newName: "ExpirationDateTime");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "TwoFactorOTP",
                newName: "UserId");
        }
    }
}
