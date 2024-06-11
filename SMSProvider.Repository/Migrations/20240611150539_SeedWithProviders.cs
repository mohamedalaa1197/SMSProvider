using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSProvider.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedWithProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    INSERT INTO SmsProviders (ProviderName, CostPerSms, Status, LoadBalanceCounter, CreatedOn, IsDeleted)
                    VALUES ('Twilio', 0.2777, 0, 0, GETDATE(), 0),
                           ('Vonage', 0.0062, 0, 0, GETDATE(), 0);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
