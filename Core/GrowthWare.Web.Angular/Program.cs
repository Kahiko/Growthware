using GrowthWare.Framework;
using GrowthWare.WebSupport.Services;
using GrowthWare.WebSupport.Jwt;
using GrowthWare.WebSupport.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:5001",
            ValidAudience = "https://localhost:5001",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigSettings.Secret))
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder => 
    { 
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod(); 
    });
});
// Add services to the container.
builder.Services.AddHttpContextAccessor();          // Added to accommodate Services that need HttpContext

// builder.Services.AddControllers();               // Commented out
builder.Services.AddControllersWithViews();         // Added

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Growthware API", Description = "Growthware is an idea dedicated to producing reusable and extendable core technologies", Version = "v1" });
});
// configure DI for application services
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<IClientChoicesService, ClientChoicesService>();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Growthware.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
IHttpContextAccessor? httpContextAccessor = app.Services.GetService<IHttpContextAccessor>();
if(httpContextAccessor != null)
{
    FunctionUtility.SetHttpContextAccessor(httpContextAccessor);
    SecurityEntityUtility.SetHttpContextAccessor(httpContextAccessor);
} 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Growthware API V1");
    });
}
if (!app.Environment.IsDevelopment())               // Added
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();                               // Added
app.UseRouting();                                   // Added

app.UseSession();

app.UseCors("EnableCORS");
app.UseAuthentication();

app.UseAuthorization();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

// app.MapControllers();                            // Commented out
app.MapControllerRoute(                             // Added
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);

app.MapFallbackToFile("index.html");                // Added

app.Run();
