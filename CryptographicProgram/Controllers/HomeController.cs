using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CryptographicProgram.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CryptographicProgram.Controllers
{
	public class HomeController : Controller
	{
		IHostingEnvironment _appEnvironment;
		public static string MyPath { get; set; } = "/images/image.jpg";

		public HomeController(IHostingEnvironment appEnvironment)
		{
			_appEnvironment = appEnvironment;
		}

		public IActionResult CryptographicPage()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddFile(IFormFile uploadedFile)
		{
			if (uploadedFile != null)
			{
				MyPath = "/images/" + uploadedFile.FileName;
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + MyPath, FileMode.Create))
				{
					await uploadedFile.CopyToAsync(fileStream);
				}
			}

			return RedirectToAction("CryptographicPage");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
