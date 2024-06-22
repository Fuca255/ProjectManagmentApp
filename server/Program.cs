global using server.DTOs.Email;
global using server.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using server.Authorization;
using server.Data;
using server.DTOs.Notification;
using server.Repositories;
using server.Services;
using server.SignalR;
using Swashbuckle.AspNetCore.Filters;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IEmailService, EmailService > ();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description="Standart Authorization using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });


    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration.GetSection("Jwt:Token").Value)),
                ValidateIssuer=false,
                ValidateAudience=false,
            };

        });
builder.Services.AddSignalR();
builder.Services.AddSingleton<IDictionary<string, UserRoomConnection>>(opt=>new Dictionary<string, UserRoomConnection>());



builder.Services.AddScoped<IProjectRepository,ProjectRepository>();
builder.Services.AddScoped<ITeamRepository,TeamRepository>();
builder.Services.AddScoped<IPriorityRepository,PriorityRepository>();
builder.Services.AddScoped<IAssignmentRepository,AssignmentRepository>();
builder.Services.AddScoped<IRoleRepository,RoleRepository>();
builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
builder.Services.AddScoped<ITaskGroupRepository, TaskGroupRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepositroy>();
builder.Services.AddScoped<IStateRepository,StateRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IUserProjectRolesRepository, UserProjectRolesRepository>();
builder.Services.AddScoped<IUserProjectPermissionRepository, UserProjectPermissionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

builder.Services.AddDbContext<ProjectManagmentDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
 );


var app = builder.Build();

if(args.Length == 1 && args[0].ToLower()=="seed")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using(var scope =  scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        service.Seed();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(policy => policy.WithOrigins("http://softeng.pmf.kg.ac.rs:10190","http://localhost:4200")
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();
app.MapHub<NotificationHub>("/api/notificationHub");

app.UseStaticFiles();

app.MapControllers();

app.Run();
