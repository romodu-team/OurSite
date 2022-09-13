using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OurSite.Core.DTOs;
using OurSite.Core.DTOs.MailDtos;
using OurSite.Core.Security;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Services.Interfaces.Mail;
using OurSite.Core.Services.Repositories;
using OurSite.Core.Services.Repositories.Mail;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Contexts;
using OurSite.DataLayer.Interfaces;
using OurSite.DataLayer.Repositories;
using OurSite.WebApi.Controllers;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using OurSite.Core.Services.Repositories.Forms;
using OurSite.Core.Services.Interfaces.Projecta;
using OurSite.OurSite.Core.Services.Repositories;
using OurSite.Core.Services.Interfaces.TicketInterfaces;
using OurSite.Core.Services.Repositories.TicketServices;
using Microsoft.AspNetCore.HttpOverrides;
using OurSite.Core.Utilities.Extentions;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

#region addservices
//test database
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection")));
//real database
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.Configure<MailSettingsDto>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IConsultationRequestService, ConsultationRequestService>();
builder.Services.AddScoped<IContactWithUsService, ContactWithUsService>();
builder.Services.AddScoped<IProject, ProjectService>();
builder.Services.AddScoped<IPayment, PaymentService>();
builder.Services.AddScoped<ICheckBoxService, CheckBoxService>();

#endregion
#region Autentication
var TokenValidationParameters= new TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidIssuer = PathTools.Domain,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Wx7Xl6rPABrWvLbLaXoBcaLQ8nQJg7L1Dce3zfE0"))
};
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = TokenValidationParameters;
}
);
#endregion
builder.Services.AddScoped<IContactWithUsService, ContactWithUsService>();
builder.Services.AddScoped<IConsultationRequestService, ConsultationRequestService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IimageGalleryService, ImageGalleryService>();
builder.Services.AddScoped<IWorkSampleService, WorkSampleService>();
builder.Services.AddScoped<IWorkSampleCategoryService, WorkSampleCategoryService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketCategoryService, TicketCategoryService>();
builder.Services.AddScoped<ITicketStatusService, TicketStatusService>();
builder.Services.AddScoped<ITicketPriorityService, TicketPriorityService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddSingleton(TokenValidationParameters);
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in StaticPermissions.GetPermissions())
    {
        options.AddPolicy(permission, policy => policy.RequireClaim(permission));
    }


});
builder.Services.AddDetection();
#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCors", mybuilder =>
    {
        mybuilder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    }

    );
});
#endregion
var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{


}
app.UseSwagger(options =>
options.SerializeAsV2 = true);
app.UseSwaggerUI(options =>
options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));

app.UseHttpsRedirection();
app.UseCors("EnableCors");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseDetection();

if (app.Environment.IsDevelopment())
    app.MapControllers().AllowAnonymous();
else
    app.MapControllers();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "upload")),
    RequestPath = "/upload"
});

app.Run();