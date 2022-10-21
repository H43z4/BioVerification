using AuthorizationService.JwtStatelessToken;
using Biometric.Services;
using Database;
using SharedLib.Interfaces;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IDBHelper>(x => new DBHelper(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddStatelessTokenAuthentication();

builder.Services.AddTransient<IBiometricService, BiometricService>();

builder.Services.AddControllers().AddNewtonsoftJson();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        while (ex.InnerException != null)
        {
            ex = ex.InnerException;
        }

        var data = new Models.ViewModels.Security.ExceptionDetails()
        {
            ExceptionMessage = ex.Message,
            StackTrack = ex.StackTrace,
            TraceId = Guid.NewGuid().ToString()
        };

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await context.Response.WriteAsJsonAsync(data);
    }
});

app.Run();