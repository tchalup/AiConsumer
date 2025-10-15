using Microsoft.EntityFrameworkCore;

namespace AiConsumer.Api.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<FileRecord> FileRecords { get; set; }
    }
}