using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alarmist.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecoveryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecoveryId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecoveryId",
                table: "Users");
        }
    }
}
