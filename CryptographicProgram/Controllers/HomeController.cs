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
		private static IHostingEnvironment _appEnvironment;

		public static string ImagePath { get; set; } = "/images/image.jpg";
		public static string DecryptImageText { get; set; }
		public static string EncryptImageText { get; set; }

		public HomeController(IHostingEnvironment appEnvironment)
		{
			_appEnvironment = appEnvironment;
		}

		public ViewResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddFile(IFormFile uploadedFile)
		{
			if (uploadedFile != null)
			{
				ImagePath = _appEnvironment.WebRootPath + "/images/" + uploadedFile.FileName;
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + ImagePath, FileMode.Create))
				{
					await uploadedFile.CopyToAsync(fileStream);
				}
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult EncryptText(string encryptText)
		{
			EncryptImageText = encryptText;
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult DecryptText()
		{
			DecryptImageText = "Hello";
			return RedirectToAction("Index");
		}
	}
}
