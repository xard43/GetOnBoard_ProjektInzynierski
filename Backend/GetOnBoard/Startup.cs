using GetOnBoard.DAL.Configuration;
using GetOnBoard.DAL;
using GetOnBoard.DAL.Repositories;
using GetOnBoard.Logging;
using GetOnBoard.Models;
using GetOnBoard.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Text;
using GetOnBoard.Chat.Hubs;
using System.Threading.Tasks;

namespace GetOnBoard
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<GetOnBoardDbContext>(
				option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),b => b.MigrationsAssembly("GetOnBoard")));

			// Azure configuration
			services.AddOptions();
			services.Configure<AzureStorageConfig>(Configuration.GetSection("AzureStorageConfig"));

			// Initialize sigleton
			services.AddSingleton<IBlobStorageRepository>(serviceProvider =>
			{
				var blobStorage = new BlobStorageRepository(serviceProvider.GetService<IOptions<AzureStorageConfig>>());
				blobStorage.Initialize().GetAwaiter().GetResult();
				return blobStorage;
			});

			//AddTransient? 
			//services.AddScoped<IGameSessionsRepository, GameSessionsRepository>();
			services.AddTransient<IGameSessionsRepository, GameSessionsRepository>();
			services.AddTransient<IProfileRepository, ProfileRepository>();
			services.AddTransient<IChatRepository, ChatRepository>();
			//services.AddSingleton<IGameSessionsRepository>(serviceProvider =>
			//{
			//	return new GameSessionsRepository(GetOnBoardDbContext dbContext);
			//});


			//TODO: CORS POLICY poprawić
			services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
			{
				builder.WithOrigins(new[] { "http://localhost:81", "http://localhost:80", "http://localhost", "http://getonboard.pl/", "https://getonboard.pl/" })
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
			}
			));
			//TODO: zmienić wymaganie na takie co są w backendzie // Marek - chyba frontendzie ? i moim zdanie to należy wyłączyć i na frroncie będzie to sprawdzane
			services.AddIdentity<ApplicationUser, IdentityRole>(
					option =>
					{
						option.Password.RequireDigit = false;
						option.Password.RequiredLength = 6;
						option.Password.RequireNonAlphanumeric = false;
						option.Password.RequireUppercase = false;
						option.Password.RequireLowercase = false;
					}
				).AddEntityFrameworkStores<GetOnBoardDbContext>()
				.AddDefaultTokenProviders();

			services.AddAuthentication(option =>
					{
						option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
						option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
						option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					})
					.AddJwtBearer(options =>
					{
						options.SaveToken = true;
						options.RequireHttpsMetadata = true;
						options.TokenValidationParameters = new TokenValidationParameters()
						{
							ValidateIssuer = true,
							ValidateAudience = true,
							ValidAudience = Configuration["Jwt:Audience"],
							ValidIssuer = Configuration["Jwt:Issuer"],
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
						};
						options.Events = new JwtBearerEvents
						{
							OnMessageReceived = context =>
							{
								var accessToken = context.Request.Query["access_token"];

								// If the request is for our hub...
								var path = context.HttpContext.Request.Path;
								if (!string.IsNullOrEmpty(accessToken) &&
									(path.StartsWithSegments("/chat")))
								{
									// Read the token out of the query string
									context.Token = accessToken;
								}
								return Task.CompletedTask;
							},
							//add token expired header
							OnAuthenticationFailed = context =>
							{
								if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
								{
									context.Response.Headers.Add("Token-Expired", "true");
								}
								return Task.CompletedTask;
							}
						};
					});
			services.AddMvc()
					.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
					.AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("GetOnBoardAPI", new Info { Title = "GetOnBoardAPI", Version = "0.0.1" });
				c.AddSecurityDefinition("Bearer", new ApiKeyScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = "header",
					Type = "apiKey"
				});
				c.OperationFilter<SecurityRequirementsOperationFilter>();

				var filePath = Path.Combine(System.AppContext.BaseDirectory, "GetOnBoard.xml");
				c.IncludeXmlComments(filePath);
			});
			services.AddSignalR();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}


			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/GetOnBoardAPI/swagger.json", "GetOnBoardAPI");
				c.RoutePrefix = string.Empty;
			});

			app.UseCors("CorsPolicy");

			//app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseSignalR(routes =>
			{
				routes.MapHub<ChatHub>("/chat");
			});
			app.UseMvc();
		}
	}
}
