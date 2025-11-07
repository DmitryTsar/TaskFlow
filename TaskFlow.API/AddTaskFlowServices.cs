using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TaskFlow.Application;
using TaskFlow.Infrastructure;

namespace TaskFlow.API{    
    public static class DependencyInjection{
        public static IServiceCollection AddTaskFlowServices(this IServiceCollection services, IConfiguration configuration){
            // Подключаем контроллеры + Swagger
            services.AddControllers().AddJsonOptions(opts =>{
                // ?? Полная поддержка Юникода (русские, китайские, эмодзи и т.д.)
                opts.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                    // ?? Отключаем автоматическое изменение регистра свойств (PascalCase остаётся)
                opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;                
            });            
            services.AddEndpointsApiExplorer();            
            services.AddSwaggerGen();            
            // Добавляем инфраструктуру (DbContext + репозитории)
            services.AddInfrastructure(configuration);            
            // Регистрируем MediatR
            services.AddMediatR(typeof(MediatrMarker).Assembly);            
            services.AddFluentValidationAutoValidation();            
            services.AddValidatorsFromAssembly(Assembly.Load("TaskFlow.Application"));
            // Настраиваем CORS (разрешим всё на старте)
            services.AddCors(opt =>{                
                opt.AddPolicy("AllowAll", policy =>{                    
                    policy.AllowAnyOrigin()                          
                    .AllowAnyMethod()                          
                    .AllowAnyHeader();                
                });            
            });            
            // Можно добавить другие сервисы здесь
            return services;        
        }    
    }
}
