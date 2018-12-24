using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Enumeration;

namespace CryptographicProgram.Controllers
{
	public class HomeController : Controller
	{
		private static IHostingEnvironment _appEnvironment;

		public static string ImagePath { get; set; } = "/images/image2.gif";
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
				var fileName = uploadedFile.FileName;

				if (string.IsNullOrWhiteSpace(fileName))
				{
					fileName = fileName?.Replace(' ', '_');
				}

				ImagePath = "/images/" + fileName;				
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + ImagePath, FileMode.Create))
				{
					await uploadedFile.CopyToAsync(fileStream);
				}
			}
			return RedirectToAction("Index");
		}
	}
}
