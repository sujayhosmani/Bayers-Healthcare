using BayersHealthcare.Application;
using BayersHealthcare.Common.ResponseInterceptor;
using BayersHealthcare.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });
builder.Services.AddHealthChecks().AddMongoDb(configuration.GetConnectionString("DefaultConnection"), "MongoDb Health", HealthStatus.Degraded);
var origins = configuration.GetValue<string>("AllowedOrigins").Split(";");


builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowedOriginCORSPolicy", policy => policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});
builder.Services.AddApplicationLayer(configuration);
builder.Services.AddInfrastructureLayer(configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowedOriginCORSPolicy");
app.UseHttpsRedirection();
app.GlobalExceptionConfig();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        var response = new ErrorResponseBody("Token is not available or unauthorize", StatusCodes.Status401Unauthorized);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        var response = new ErrorResponseBody("You do not have permission, Unauthorized access", StatusCodes.Status403Forbidden);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
