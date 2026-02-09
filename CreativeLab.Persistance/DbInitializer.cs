using Microsoft.EntityFrameworkCore;

namespace CreativeLab.Persistence;

public class DbInitializer
{
    public static void Initialize(CreativeLabDbContext context)
    {
        context.Database.Migrate();
    }
}