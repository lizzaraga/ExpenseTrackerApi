using System.Text;
using ExpenseTracker.Api.Features.Authentication.Configuration;
using ExpenseTracker.Api.Features.Authentication.Interfaces;
using ExpenseTracker.Api.Features.Authentication.Services;
using ExpenseTracker.Api.Configurations;
using ExpenseTracker.Api.Features.Pockets.Interfaces;
using ExpenseTracker.Api.Features.Pockets.Services;
using ExpenseTracker.Api.Features.Purses.Interfaces;
using ExpenseTracker.Api.Features.Purses.Services;
using ExpenseTracker.Api.Features.Transfers.Interfaces;
using ExpenseTracker.Api.Features.Transfers.Services;
using ExpenseTracker.Database;
using ExpenseTracker.Database.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Expense Tracker",
        Contact = new OpenApiContact(){ Email = "fomekongchristmael@gmail.com", Name = "Mael Fomekong"}
    });

    var securityScheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        },
        Type = SecuritySchemeType.Http,
        Name = "Authorization",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Description = "Just enter the Bearer Token",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    };
    o.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    o.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { securityScheme, Array.Empty<string>() }
    });
});

#region Configure Auto Mapper

builder.Services.AddAutoMapper(o => o.AddProfile<AutoMapperProfile>(),
    AppDomain.CurrentDomain.GetAssemblies());

#endregion

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

#region Configure Database

builder.Services.AddDbContext<ExpenseTrackerDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("DbConString"), optionsBuilder =>
    {
        optionsBuilder.MigrationsAssembly(typeof(ExpenseTrackerDbContext).Assembly.FullName);
    });
});

#endregion

#region Configure Identity

builder.Services.AddIdentity<UserAccount, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ExpenseTrackerDbContext>();

#endregion

#region Configure JWT

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
        ValidAudience = builder.Configuration["Jwt:Audience"]!,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});
builder.Services.AddAuthorization();

#endregion

#region Configure Services

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPurseIncomeService, PurseIncomeHistoryService>();
builder.Services.AddScoped<IPurseService, PurseService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<IPocketService, PocketService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

