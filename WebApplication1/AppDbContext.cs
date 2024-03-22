using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore;

public interface ITenantInfo
{
    string ConnectionString { get; }
}

public class MyTenantInfo : ITenantInfo
{
    // Implement the ITenantInfo interface to provide the connection string for the current tenant.
    public string ConnectionString { get; } = "DefaultConnection";
}

public class AppDbContext : DbContext
{
    private readonly ITenantInfo _tenantInfo;

    public AppDbContext(ITenantInfo tenantInfo)
    {
        Console.WriteLine("hello");
        // DI will pass in the tenant info for the current request.
        // ITenantInfo is also injectable.
        _tenantInfo = tenantInfo;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("hello 30");
        // Use the connection string to connect to the per-tenant database.
        optionsBuilder.UseSqlServer(_tenantInfo.ConnectionString);
    }
    public DbSet<GenericEntity> GenericEntities { get; set; }
}


// public interface ITenantInfo
// {
//     string ConnectionString { get; }
// }

// public class MyTenantInfo : ITenantInfo
// {
//     // Implement the ITenantInfo interface to provide the connection string for the current tenant.
//     public string ConnectionString { get; } = "DefaultConnection";

//     public static implicit operator TenantInfo(MyTenantInfo v)
//     {
//         throw new NotImplementedException();
//     }
// }

// public class AppDbContext : DbContext
// {
//    private TenantInfo TenantInfo { get; set; }

//    public AppDbContext(MyTenantInfo tenantInfo)
//    {
//        TenantInfo = tenantInfo;
//    } 

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        // Use the connection string to connect to the per-tenant database.
//        optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
//    }
//     public DbSet<GenericEntity> GenericEntities { get; set; }
   
// }



