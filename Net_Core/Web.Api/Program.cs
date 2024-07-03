using GrowthWare.Framework;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Web.Support.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddAuthentication(opt => {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer [jwt]'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    var mOpenApiSecurityScheme = new OpenApiSecurityScheme {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { mOpenApiSecurityScheme, Array.Empty<string>() } });

    options.SwaggerDoc("v1", new OpenApiInfo {
            Title = "Growthware API",
            Description = "Growthware is an idea dedicated to producing reusable and extendable core technologies",
            Version = "v1"
        }
    );
});
// Add JWT Authentication
builder.Services.AddScoped<IJwtUtility, JwtUtility>();
// Configure Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.Cookie.Name = ".Growthware.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Configure CORS
builder.Services.AddCors(options => {
    options.AddPolicy("EnableCORS", builder => {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// configure DI for application services
builder.Services.AddScoped<IJwtUtility, JwtUtility>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
// Configure the HTTP request pipeline.
// Enable CORS
app.UseCors("EnableCORS");
// Use Session
app.UseSession();
// Use custom jwt middleware
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

// Configure IHttpContextAccessor 
IHttpContextAccessor? httpContextAccessor = app.Services.GetService<IHttpContextAccessor>();
if (httpContextAccessor != null)
{
    SecurityEntityUtility.SetHttpContextAccessor(httpContextAccessor);
    SessionHelper.SetHttpContextAccessor(httpContextAccessor);
}

if (app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();                       // moved out of if (app.Environment.IsDevelopment())
app.UseSwaggerUI(mSwaggerUIOptions => { // moved out of if (app.Environment.IsDevelopment())
    mSwaggerUIOptions.DocumentTitle = "Growthware API";
    mSwaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Growthware API V1");
    mSwaggerUIOptions.InjectJavascript("/assets/swagger-ui/custom-script.js");
    mSwaggerUIOptions.InjectStylesheet("/assets/swagger-ui/custom-style.css");
});
app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);

app.Run();
