using habytee.Interconnection.Models;
using habytee.Server.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<DataService>();

builder.Services.AddDbContext<ReadDbContext>(options =>
    //options.UseNpgsql($"Host={Environment.GetEnvironmentVariable("POSTGRES_WORKER_HOST")};Port={Environment.GetEnvironmentVariable("POSTGRES_WORKER_PORT")};Username={Environment.GetEnvironmentVariable("POSTGRES_WORKER_USERNAME")};Password={Environment.GetEnvironmentVariable("POSTGRES_WORKER_PASSWORD")};Database={Environment.GetEnvironmentVariable("POSTGRES_WORKER_DATABASE")}")
    //same for read and write right now.
    options.UseNpgsql($"Host={Environment.GetEnvironmentVariable("POSTGRES_MASTER_HOST")};Port={Environment.GetEnvironmentVariable("POSTGRES_MASTER_PORT")};Username={Environment.GetEnvironmentVariable("POSTGRES_MASTER_USERNAME")};Password={Environment.GetEnvironmentVariable("POSTGRES_MASTER_PASSWORD")};Database={Environment.GetEnvironmentVariable("POSTGRES_MASTER_DATABASE")}")
);

builder.Services.AddDbContext<WriteDbContext>(options =>
    options.UseNpgsql($"Host={Environment.GetEnvironmentVariable("POSTGRES_MASTER_HOST")};Port={Environment.GetEnvironmentVariable("POSTGRES_MASTER_PORT")};Username={Environment.GetEnvironmentVariable("POSTGRES_MASTER_USERNAME")};Password={Environment.GetEnvironmentVariable("POSTGRES_MASTER_PASSWORD")};Database={Environment.GetEnvironmentVariable("POSTGRES_MASTER_DATABASE")}")
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var writeDbContext = serviceProvider.GetRequiredService<WriteDbContext>();
    //TODO: https://learn.microsoft.com/de-de/shows/cituscon-an-event-for-postgres-2023/citus-on-kubernetes-citus-con-an-event-for-postgres-2023
    //https://hub.docker.com/r/citusdata/citus/
    /*SELECT citus_set_coordinator_host('citus-master', Environment.GetEnvironmentVariable("POSTGRES_MASTER_PORT"));
     SELECT * from citus_add_node('citus-workers', Environment.GetEnvironmentVariable("POSTGRES_WORKER_PORT"));
    */
    writeDbContext.Database.Migrate();

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
//app.UseHttpsRedirection();

app.Run();
