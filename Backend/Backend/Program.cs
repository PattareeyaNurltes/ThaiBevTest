using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(); 
builder.Services.AddOpenApi();

//Add Swagger gen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero // ปิดการยืดเวลา 5 นาทีของระบบ
        };
    });

// Register DbContext
builder.Services.AddDbContext<PattareeyaDbContext>
       (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Seervices
builder.Services.AddScoped<BaseHadler>();
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddScoped<Backend.Services.TokenHandler>();
builder.Services.AddScoped<UsersHadler>();



var app = builder.Build();

//Add Swagger gen
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
