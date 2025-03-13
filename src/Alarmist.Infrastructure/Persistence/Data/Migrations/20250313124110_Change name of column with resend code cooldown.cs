using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alarmist.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changenameofcolumnwithresendcodecooldown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerificationCodeResendTimer",
                table: "Users",
                newName: "VerificationCodeResendCooldown");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerificationCodeResendCooldown",
                table: "Users",
                newName: "VerificationCodeResendTimer");
        }
    }
}
