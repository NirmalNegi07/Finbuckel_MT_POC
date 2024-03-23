using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;

// public interface ITenantInfo
// {
//     string ConnectionString { get; }
// }

// public class MyTenantInfo : ITenantInfo
// {
//     // Implement the ITenantInfo interface to provide the connection string for the current tenant.
//     public string ConnectionString { get; } = "DefaultConnection";
// }

public class AppDbContext : MultiTenantDbContext
{
    private readonly ITenantInfo _tenantInfo;
    public DbSet<Student> GenericEntities { get; set; }

    public AppDbContext(ITenantInfo tenantInfo) : base(tenantInfo)
    {
        _tenantInfo = tenantInfo;
    }

    public AppDbContext(ITenantInfo tenantInfo, DbContextOptions options) : base(tenantInfo, options)
    {
       _tenantInfo = tenantInfo; 
    }
    // public AppDbContext(IMultiTenantContextAccessor<TenantInfo> multiTenantContextAccessor) : base(multiTenantContextAccessor)
    // {
    //     Console.WriteLine("hello");
    //      _tenantInfo = multiTenantContextAccessor.MultiTenantContext?.TenantInfo;
    //      Console.WriteLine("hello 30" + _tenantInfo.ConnectionString);
    //     // DI will pass in the tenant info for the current request.
    //     // ITenantInfo is also injectable.
    //     // _tenantInfo = tenantInfo;
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("hello 30");
        // Use the connection string to connect to the per-tenant database.
        optionsBuilder.UseMySQL(_tenantInfo.ConnectionString);
    }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<GenericEntity>().IsMultiTenant();
    //     base.OnModelCreating(modelBuilder);
    // }
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



