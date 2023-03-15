using Donations_App.Data;
using Donations_App.Repositories.CategoryServices;
using Donations_App.Models;
using Donations_App.Services;
using Donations_App.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Donations_App.Repositories.PatientCaseServices;
using Donations_App.Repositories.CartItemServices;

using Donations_App.Repositories.FileUploadedServices;
using Donations_App.Repositories.RequestServices;
using Donations_App.Repositories.OrderServices;
using Donations_App.Repositories.OrderItemsServices;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMailingService, MailingService>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IPatientCaseRepository, PatientCaseRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IFileUploadedService, FileUploadedService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();




//-----------------------------------------------------------------------------------------------------------------------
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection("PaymentSettings"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
}); 


builder.Services.AddAuthentication(f =>
{
    f.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    f.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(k =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    k.SaveToken = true;
    k.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key),
        ClockSkew = TimeSpan.Zero
    };

});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
   app.UseSwagger();
   app.UseSwaggerUI();
//}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseAuthorization();


app.MapControllers();

app.Run();
