using CryptographicProgram.Algorithms.Abstractions;
using CryptographicProgram.Algorithms.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptographicProgram
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.AddScoped<ISteganographyAlgorithm, SimpleLsb>();
			services.AddScoped<IAlgorithmFactory, AlgorithmFactory>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseCors(builder => builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials());

			app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();
		}
	}
}