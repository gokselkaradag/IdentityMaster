using IdentityApp.Data;
using IdentityApp.Models.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
        new SmtpEmailSender(
                builder.Configuration["EmailSender:Host"],
                builder.Configuration.GetValue<int>("EmailSender:Port"),
                builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                builder.Configuration["EmailSender:Username"],
                builder.Configuration["EmailSender:Password"])
        );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DataContext>(options =>
            {
                var config = builder.Configuration;
                var connectionstring = config.GetConnectionString("database");
                options.UseSqlServer(connectionstring);
            });

            builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.SignIn.RequireConfirmedEmail = true;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login/Index/";
            });

            builder.Services.AddAuthentication()
.AddGoogle(x =>
{
    x.ClientId = "98175004192-vopir39dc32510fdc2ntb09hkai4hdk0.apps.googleusercontent.com";
    x.ClientSecret = "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";

    x.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
        return Task.CompletedTask;
    };

})
.AddFacebook(x =>
{
    x.ClientId = "1504086693556868";
    x.ClientSecret = "fa3e08f66e488f5426fd4af40aac5a7f";
});


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
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
