using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PlayApp.Helpers2;
using Synercoding.FormsAuthentication;
using Infrastructure.Context;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Core.Entities;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DbConn>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var section = builder.Configuration.GetSection("FormsAuthentication");

var faOptions = new FormsAuthenticationOptions()
{
    DecryptionKey = section.GetValue<string>("DecryptionKey"),
    ValidationKey = section.GetValue<string>("ValidationKey"),
    EncryptionMethod = section.GetValue<EncryptionMethod>("EncryptionMethod"),
    ValidationMethod = section.GetValue<ValidationMethod>("ValidationMethod"),
};

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
      .AddCookie(options =>
      {
          options.Cookie.Name = section.GetValue<string>("CookieName");
          options.AccessDeniedPath = "/" + section.GetValue<string>("ProjectName") + "/Identity/Account/AccessDenied";
          //options.AccessDeniedPath = "/Error/ErrorPage";
          options.LoginPath = "/Services/Account/Login";
          options.ReturnUrlParameter = "ReturnUrl";
          options.Events = new CookieAuthenticationEvents()
          {
              OnSigningOut = (context) =>
              {
                  context.HttpContext.Response.Redirect(section.GetValue<string>("Logout"));
                  return Task.CompletedTask;
              },
              OnRedirectToLogout = (context) =>
              {
                  context.HttpContext.Response.Redirect(section.GetValue<string>("Logout"));
                  return Task.CompletedTask;
              }
          };
          options.TicketDataFormat = new FormsAuthenticationDataFormat<AuthenticationTicket>(
              faOptions,
              FormsAuthHelper.ConvertCookieToTicket,
              FormsAuthHelper.ConvertTicketToCookie
          );
          options.SlidingExpiration = true;
          options.Events.OnRedirectToAccessDenied = context => FormsAuthHelper.RedirectToAccessDenied(context, section.GetValue<string>("BaseAuthUrl"));
          options.Events.OnRedirectToLogin = context => FormsAuthHelper.RedirectToLogin(context, section.GetValue<string>("BaseAuthUrl"), section.GetValue<string>("ProjectName"));
      });



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUser,UserRepository>();
builder.Services.AddScoped<IServices,ServicesRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var userRepository = scope.ServiceProvider.GetRequiredService<IUser>();


    var user = new User
    { UserName = "mgahmed", Role = (Core.Enums.Roles)1, Create_At = DateTime.Now, Created_by = 1 };

    await userRepository.Create(user);

}


app.Run();
