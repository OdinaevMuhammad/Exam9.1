using System.Security.Principal;
using Infrastructure.Context;
using Infrastructure.Services;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(op => op.UseNpgsql(connection));

builder.Services.AddAutoMapper(typeof(ServiceProfile));
builder.Services.AddControllers();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<JobHistoryService>();
builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<JobTimeHistoryService>();
// builder.Services.AddScoped<UserService>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
 {
 config.Password.RequiredLength = 4;
 config.Password.RequireDigit = false; // must have at least one digit
 config.Password.RequireNonAlphanumeric = false; // must have at least one non-alphanumeric character
 config.Password.RequireUppercase = false; // must have at least one uppercase character
 config.Password.RequireLowercase = false; // must have at least one lowercase character
 })
 .AddEntityFrameworkStores<DataContext>()
 .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
 {
 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
 }).AddJwtBearer(o =>
 {
 var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
 o.TokenValidationParameters = new TokenValidationParameters
 {
 ValidateIssuer = false,
 ValidateAudience = false,
 ValidateLifetime = true,
 ValidateIssuerSigningKey = true,
 ValidIssuer = builder.Configuration["JWT:Issuer"],
 ValidAudience = builder.Configuration["JWT:Audience"],
 IssuerSigningKey = new SymmetricSecurityKey(key)
 };
 });
builder.Services.AddSwaggerGen(c =>
 {
 c.SwaggerDoc("v1", new OpenApiInfo
 {
 Title = "Sample web API",
 Version = "v1",
 Description = "Sample API Services.",
 Contact = new OpenApiContact
 {
 Name = "John Doe"
 },
 });
 c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
 c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
 {
 Name = "Authorization",
 Type = SecuritySchemeType.ApiKey,
 Scheme = "Bearer",
 BearerFormat = "JWT",
 In = ParameterLocation.Header,
 Description = "JWT Authorization header using the Bearer scheme."
 });
 
 c.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
 {
 new OpenApiSecurityScheme
 {
 Reference = new OpenApiReference
 {
 Type = ReferenceType.SecurityScheme,
 Id = "Bearer"
 }
 },
 new string[] {}
 }
 });
 });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();

app.Run();
