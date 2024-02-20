using GrowthWare.Framework;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = ConfigSettings.Issuer,
            ValidAudience = ConfigSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigSettings.Secret)),
        };
    }
);

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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer [jwt]'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    var mOpenApiSecurityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { mOpenApiSecurityScheme, Array.Empty<string>() } });

    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Growthware API",
            Description = "Growthware is an idea dedicated to producing reusable and extendable core technologies",
            Version = "v1"
        }
    );
});
// configure DI for application services
builder.Services.AddScoped<IJwtUtility, JwtUtility>();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Growthware.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
IHttpContextAccessor? httpContextAccessor = app.Services.GetService<IHttpContextAccessor>();
if (httpContextAccessor != null)
{
    SecurityEntityUtility.SetHttpContextAccessor(httpContextAccessor);
    SessionController.SetHttpContextAccessor(httpContextAccessor);
}
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(mSwaggerUIOptions =>
{
    mSwaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Growthware API V1");
    mSwaggerUIOptions.InjectJavascript("/assets/swagger-ui/custom-script.js");
    mSwaggerUIOptions.InjectStylesheet("/assets/swagger-ui/custom-style.css");
});

if (!app.Environment.IsDevelopment())               // Added
{
    app.UseDefaultFiles();                          // Added
    app.UseStaticFiles();                           // Added
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
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

app.Run();
