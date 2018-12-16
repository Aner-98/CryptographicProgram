using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CryptographicProgram.Controllers
{
	public class HomeController : Controller
	{
		IHostingEnvironment _appEnvironment;

		public static string ImagePath { get; set; } = "/images/image.jpg";
		public static string DecryptImageText { get; set; }
		public static string EncryptImageText { get; set; }

		public HomeController(IHostingEnvironment appEnvironment)
		{
			_appEnvironment = appEnvironment;
		}

		public ViewResult Index()
		{
			return View("CryptographicPage");
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
				ImagePath = "/images/" + uploadedFile.FileName;
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + ImagePath, FileMode.Create))
				{
					await uploadedFile.CopyToAsync(fileStream);
				}
			}

			return RedirectToAction("CryptographicPage");
		}

		[HttpPost]
		public IActionResult EncryptText(string encryptText)
		{
			EncryptImageText = encryptText;
			return RedirectToAction("CryptographicPage");
		}

		[HttpGet]
		public IActionResult DecryptText()
		{
			DecryptImageText = "Helow";
			return RedirectToAction("CryptographicPage");
		}
	}
}
