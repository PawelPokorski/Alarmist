using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alarmist.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedVerificationCodeExpirationAndTimer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCodeExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerificationCodeResendCooldown",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VerificationCodeExpiry",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VerificationCodeResendCooldown",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
