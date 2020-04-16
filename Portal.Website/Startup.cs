using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portal.Database;
using Portal.Database.Repositories;
using Portal.Services;
using Portal.Services.Blob;
using Portal.Services.GraphServices;

namespace Portal.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options =>
                {
                    Configuration.Bind("AzureAd", options);
                });
            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options => {
                options.Authority = options.Authority + "/v2.0/";
                options.TokenValidationParameters.ValidateIssuer = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<PortalContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PortalDatabase")));
            DocumentService.DocumentBlogConnectionString = Configuration.GetConnectionString("BlobStorage");
            ProfilePhotoService.PhotoBlobConnectionString = Configuration.GetConnectionString("BlobStorage");

            string clientSecret = Configuration["ClientSecret"];
            string clientId = Configuration["ClientId"];
            string tenantId = Configuration["TenantId"];
            var useSharedCalendar = bool.Parse(Configuration["UseSharedCalendar"]);

            services.AddTransient<AzureAuthenticationProvider>(s => new AzureAuthenticationProvider(clientId, clientSecret, tenantId));
            services.AddTransient<NewsArticleManager>();
            services.AddTransient<NewsArticleRepository>();
            services.AddTransient<DocumentService>();
            services.AddTransient<ProfilePhotoService>();
            services.AddTransient<PeopleService>();
            services.AddTransient<CalendarService>(s => new CalendarService(Configuration["GroupCalendarId"], Configuration["SharedCalendar"], useSharedCalendar, clientId, clientSecret, tenantId));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
