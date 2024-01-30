using Data;
using Microsoft.EntityFrameworkCore;

namespace webapi.Extensions;

public static class DbContextExtension
{
  public static void AddDbContext(this IServiceCollection services, string connectionString)
  {
    services.AddDbContext<WapContext>(options => options.UseSqlServer(connectionString, optionsBuilder =>
    {
      optionsBuilder.MigrationsAssembly("Data");
    }));
  }
}
