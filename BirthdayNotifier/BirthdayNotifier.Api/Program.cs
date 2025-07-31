using BirthdayNotifier.Api.Hangfire;
using BirthdayNotifier.Core.Interfaces;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Interfaces.Services;
using BirthdayNotifier.Infrastructure.Data;
using BirthdayNotifier.Infrastructure.Hangfire;
using BirthdayNotifier.Infrastructure.Options;
using BirthdayNotifier.Infrastructure.Repositories;
using BirthdayNotifier.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using IGroupRepository = BirthdayNotifier.Core.Interfaces.Repositories.IGroupRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IBirthdayEntryRepository, BirthdayEntryRepository>();
builder.Services.AddScoped<IBirthdayService, BirthdayService>();

builder.Services.AddHttpClient<INotificationService, NtfyNotificationService>();

builder.Services.Configure<NtfyOptions>(
    builder.Configuration.GetSection("Ntfy"));


var hangfireOptions = new PostgreSqlStorageOptions
{
    SchemaName = "hangfire_birthdays"
};


builder.Services.AddHangfireServer();
builder.Services.AddHangfire(configuration =>
    configuration.UsePostgreSqlStorage(c =>
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")), hangfireOptions));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = new[] { new HangFireAuthorizationFilter() }
});

JobsResolver.AddJobs();

app.UseHttpsRedirection();

app.Run();