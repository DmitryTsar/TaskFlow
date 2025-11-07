using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TaskFlow.API;
using TaskFlow.Infrastructure.Persistence;
// Создаем логгер
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
try
{    
    Log.Information("Starting TaskFlow API...");    
    var builder = WebApplication.CreateBuilder(args);    
    // Настраиваем Serilog
    builder.Host.UseSerilog();    
    // Регистрация всех сервисов через наш класс
    builder.Services.AddTaskFlowServices(builder.Configuration);
    //JWT Authentication
    builder.Services.AddAuthentication(options =>    
    {        
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    
    })    
        .AddJwtBearer(options =>    
        {        
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);        
            options.TokenValidationParameters = new TokenValidationParameters        
            {            
                ValidateIssuer = true,            
                ValidateAudience = true,            
                ValidateLifetime = true,            
                ValidateIssuerSigningKey = true,            
                ValidIssuer = builder.Configuration["Jwt:Issuer"],            
                ValidAudience = builder.Configuration["Jwt:Audience"],            
                IssuerSigningKey = new SymmetricSecurityKey(key)        
            };    
        });    
    var app = builder.Build();    
    // Автоматическое применение миграций при старте контейнера
    using (var scope = app.Services.CreateScope())    
    {        
        var db = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();        
        db.Database.Migrate(); 
        // создаёт базу и применяет миграции
    }    
    // Middleware
    if (app.Environment.IsDevelopment())    
    {        
        app.UseSwagger();        
        app.UseSwaggerUI();    
    }    
    app.UseSerilogRequestLogging();    
    app.UseHttpsRedirection();    
    app.UseCors("AllowAll");    
    app.UseAuthentication();    
    app.UseAuthorization();    
    app.MapControllers();    
    app.Run();
}
catch (Exception ex)
{   
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{    
    Log.CloseAndFlush();
}
