using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMultiTenant<TenantInfo>()
    .WithHostStrategy()
    .WithConfigurationStore();

var app = builder.Build();


// Use the middleware to enable multi-tenancy
app.UseMultiTenant();

// Use the routing middleware
app.UseRouting();

// Define your endpoints after configuring multi-tenancy middleware and routing middleware
app.MapGet("/", (IMultiTenantContextAccessor<TenantInfo> multiTenantContextAccessor) =>
{
    var tenantInfo = multiTenantContextAccessor.MultiTenantContext?.TenantInfo;
    if (tenantInfo != null)
    {
        Console.WriteLine( "Hello");
        using var scope = app.Services.CreateScope();
        Console.WriteLine( "Hello2");
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>( );
        Console.WriteLine( "Hello3");
        Console.WriteLine(dbContext + "dbContext");
        Console.WriteLine(tenantInfo.Name + "tenantInfo");
        return $"Hello {tenantInfo.Name}!";
    }
    else
    {
        return "Tenant information not available!";
    }
});

builder.Services.AddScoped<ITenantInfo, MyTenantInfo>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Get the multi-tenant context accessor from the dependency injection container.
    using var scope = app.Services.CreateScope();
    var multiTenantContextAccessor = scope.ServiceProvider.GetRequiredService<IMultiTenantContextAccessor<TenantInfo>>();

    // Resolve the current tenant information. This ensures the correct tenant is used.
    var tenantInfo = multiTenantContextAccessor.MultiTenantContext?.TenantInfo;

    // Use the connection string based on the resolved tenant information.
    options.UseMySQL(tenantInfo?.ConnectionString ?? builder.Configuration.GetConnectionString("DefaultConnection"));
});

app.Run();

// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Finbuckle.MultiTenant;
// using MySql.Data.EntityFrameworkCore.Extensions;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddMultiTenant<TenantInfo>()
//     .WithHostStrategy()
//     .WithConfigurationStore();

// builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
// {
//     Console.WriteLine("oye hello");
//     // Retrieve the multi-tenant context and get the connection string for the current tenant.
//     var multiTenantContext = serviceProvider.GetService<IMultiTenantContextAccessor<TenantInfo>>()?.MultiTenantContext;
//     var connectionString = multiTenantContext?.TenantInfo?.ConnectionString ?? builder.Configuration.GetConnectionString("DefaultConnection");
//     options.UseMySQL(connectionString); // Use the MySQL provider with the connection string.
// });

// builder.Services.AddSingleton<ITenantInfo, MyTenantInfo>();


// var app = builder.Build();

// // Configure the HTTP request pipeline.
// app.UseMultiTenant(); // Use multi-tenancy middleware.

// app.UseRouting();

// app.MapGet("/", async (IMultiTenantContextAccessor<TenantInfo> multiTenantContextAccessor) =>
// {
//     var tenantInfo = multiTenantContextAccessor.MultiTenantContext?.TenantInfo;
//     Console.WriteLine("tenantInfo" + tenantInfo.ConnectionString);
//     if (tenantInfo != null)
//     {
//         using var scope = app.Services.CreateScope();
//         var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//         try
//         {
//             var data = await dbContext.GenericEntities.Where(e => e.TenantId == tenantInfo.Id).ToListAsync();
//             return $"Data for {tenantInfo.Name}: {string.Join(", ", data.Select(d => d.Data))}";
//         }
//         catch (Exception ex)
//         {
//             // Log the exception or display an error message
//             return $"An error occurred: {ex.Message}";
//         }
//     }
//     else
//     {
//         return "Tenant information not available!";
//     }
// });

// app.Run();
