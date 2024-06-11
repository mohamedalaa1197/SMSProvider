using Microsoft.EntityFrameworkCore;
using SMSProvider.Domain;

namespace SMSProvider.Repository.Context;

public class SmsProviderDBContext : DbContext
{
    public SmsProviderDBContext(DbContextOptions<SmsProviderDBContext> options)
        : base(options)
    {
    }

    public DbSet<SmsLog> SmsLogs { get; set; }
    public DbSet<SmsProvider> SmsProviders { get; set; }
}