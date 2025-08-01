using System.Text;
using BirthdayNotifier.Api.Hangfire;
using BirthdayNotifier.Core.Interfaces.Repositories;
using BirthdayNotifier.Core.Interfaces.Services;
using BirthdayNotifier.Domain.Identity;
using BirthdayNotifier.Infrastructure.Data;
using BirthdayNotifier.Infrastructure.Hangfire;
using BirthdayNotifier.Infrastructure.Options;
using BirthdayNotifier.Infrastructure.Repositories;
using BirthdayNotifier.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IBirthdayEntryRepository, BirthdayEntryRepository>();
builder.Services.AddScoped<IBirthdayService, BirthdayService>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddHttpClient<INotificationService, NtfyNotificationService>();

builder.Services.Configure<NtfyOptions>(
    builder.Configuration.GetSection("Ntfy"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
        };
    });

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


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
app.UseAuthentication();
app.UseAuthorization();

app.Run();