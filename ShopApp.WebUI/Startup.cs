using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Data Source=shopDb"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;

                //lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                //users
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accesdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name=".ShopApp.Security.Cookie",
                    SameSite=SameSiteMode.Strict
                };
            });

            services.AddScoped<IProductRepository,EfCoreProductRepository>();
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ICartRepository,EfCoreCartRepository>();


            services.AddScoped<IProductService,ProductManager>();         
            services.AddScoped<ICategoryService,CategoryManager>();
            services.AddScoped<ICartService,CartManager>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
            new SmtpEmailSender(
                _configuration["EmailSender:Host"],
                _configuration.GetValue<int>("EmailSender:Port"),
                _configuration.GetValue<bool>("EmailSender:EnableSSl"),
                 _configuration["EmailSender:UserName"],
                 _configuration["EmailSender:Password"])
            );

            services.AddControllersWithViews();
            //services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Error");
        //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //        app.UseHsts();
        //    }

        //    app.UseHttpsRedirection();
        //    app.UseStaticFiles();

        //    app.UseRouting();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapRazorPages();
        //    });
        //}

        public void Configure(IApplicationBuilder app,IWebHostEnvironment environment,IConfiguration configuration,UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/modules"
            });

            //if (environment.IsDevelopment())
            //{
            //    SeedDatabase.Seed();
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                     name: "cart",
                     pattern: "cart",
                     defaults: new { controller = "Cart", action = "Index" }
                 );

                endpoints.MapControllerRoute(
              name: "adminusers",
              pattern: "admin/user/list",
              defaults: new { controller = "Admin", action = "UserList" }
              );

                endpoints.MapControllerRoute(
                   name: "adminuseredit",
                   pattern: "admin/user/{id?}",
                   defaults: new { controller = "Admin", action = "UserEdit" }
               );

                endpoints.MapControllerRoute(
                name: "adminroles",
                pattern: "admin/role/list",
                defaults: new { controller = "Admin", action = "RoleList" }
                );

                endpoints.MapControllerRoute(
               name: "adminrolecreate",
               pattern: "admin/role/create",
               defaults: new { controller = "Admin", action = "RoleCreate" }
               );

                endpoints.MapControllerRoute(
               name: "adminroleedit",
               pattern: "admin/role/{id?}",
               defaults: new { controller = "Admin", action = "RoleEdit" }
               );

                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" }
                    );

               endpoints.MapControllerRoute(
                name: "adminproductcreate",
                pattern: "admin/products/create",
                defaults: new { controller = "Admin", action = "ProductCreate" }
                );

               endpoints.MapControllerRoute(
                name: "adminproductedit",
                pattern: "admin/products/{id?}",
                defaults: new { controller = "Admin", action = "ProductEdit" }
                );

               endpoints.MapControllerRoute(
                 name: "admincategories",
                 pattern: "admin/categories",
                 defaults: new { controller = "Admin", action = "CategoryList" }
                 );

               endpoints.MapControllerRoute(
                name: "admincategorycreate",
                pattern: "admin/categories/create",
                defaults: new { controller = "Admin", action = "CategoryCreate" }
                );

                endpoints.MapControllerRoute(
                name: "admincategoryedit",
                pattern: "admin/categories/{id?}",
                defaults: new { controller = "Admin", action = "CategoryEdit" }
               );


                endpoints.MapControllerRoute(
                  name: "search",
                  pattern: "search",
                  defaults: new { controller = "Shop", action = "search" }
                  );

                endpoints.MapControllerRoute(
                   name: "productdetails",
                   pattern: "{url}",
                   defaults: new { controller = "Shop", action = "details" }
                   );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new { controller="Shop",action="list" }
                    );

                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                    );
            });


            SeedIdentity.Seed(userManager,roleManager,configuration).Wait();
        }
    }
}
