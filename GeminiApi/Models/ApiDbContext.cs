using Microsoft.EntityFrameworkCore;

namespace GeminiApi.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<FileRecord> FileRecords { get; set; }
    }
}