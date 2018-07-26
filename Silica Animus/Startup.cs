using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Abominable_Intelligence.Model;
using Abominable_Intelligence.Prediction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Silica_Animus.Builders;
using Silica_Animus.Contexts;
using Silica_Animus.Helpers;
using Silica_Animus.Middleware;
using Silica_Animus.Model;
using Silica_Animus.Repository;

namespace Silica_Animus
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        private ILogger<Startup> Logger { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            // Read connection string from secure place
            // Here simple example, VerySafePlace should be in fact safe!
            var conf = FileReader.ReadAsJSONFromFile<IdentityConf>(Configuration["ConfigurationPath"]);
            services.AddDbContext<SilicaIdentityContext>(options =>
            {
                if (conf == null)
                {
                    conf = FileReader.ReadAsJSONFromFile<IdentityConf>(Configuration["ConfigurationPath"]);
                }
                options.UseSqlServer(conf.DBConnString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<SilicaIdentityContext>()
                    .AddDefaultTokenProviders();

            services.Configure<PasswordHasherOptions>(options =>
            {
                options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = true;
                config.SaveToken = true;
                config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = conf.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = conf.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf.JwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "SilicaAnimus", Version = "v1" });
            });

            services.AddMvc();
            // For proxy purpose
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            services.AddSingleton<IPEngine<SentimentData, SentimentPrediction>, PEngine<SentimentData, SentimentPrediction>>();
            services.AddTransient<IPredictionResultBuilder, PredictionResultBuilder>();
            services.AddTransient<ITokenBuilder, TokenBuilder>();
            services.AddTransient<IBaseResultBuilder, BaseResultBuilder>();
            services.AddTransient<IAuthRepository,AuthRepository>();

            conf = null;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SilicaIdentityContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            // Accept just https
            app.Use((req, midd) =>
            {
                req.Request.Scheme = "https";
                return midd();
            });

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "SilicaAnimus");
            });

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "api/{controller}/{action}/{id?}",
                        defaults: new { controller = "Doc", action = "Index" }
                    );
            });
            context.Database.EnsureCreated();
        }
    }
}
