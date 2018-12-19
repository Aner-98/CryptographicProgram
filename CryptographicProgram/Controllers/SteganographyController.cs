using System;
using System.Drawing;
using CryptographicProgram.Algorithms.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;


namespace CryptographicProgram.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SteganographyController : ControllerBase
	{
		private readonly ISteganographyAlgorithm _steganographyAlgorithm;
		private readonly IHostingEnvironment _appEnvironment;


		public SteganographyController(ISteganographyAlgorithm steganographyAlgorithm, IHostingEnvironment appEnvironment)
		{
			_appEnvironment = appEnvironment;
			_steganographyAlgorithm = steganographyAlgorithm;
		}

		[HttpGet]
		[Route("GetEncode")]
		public IActionResult EncodeText([FromQuery]string text)
		{
			var path = _appEnvironment.WebRootPath + HomeController.ImagePath;
			var bitmap = _steganographyAlgorithm.Encode(new Bitmap(path), text);

			var fileName = Guid.NewGuid().ToString() + ".bmp";
			var filePath = _appEnvironment.WebRootPath + @"\images\" + fileName;

			bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
			HomeController.ImagePath = "/images/" + fileName;

			var image = System.IO.File.OpenRead(filePath);
			return File(image, "image/jpeg");
		}

		[HttpGet]
		[Route("SecretText")]
		public IActionResult GetSecretText()
		{
			return Ok(new { text = _steganographyAlgorithm.Decode(new Bitmap(_appEnvironment.WebRootPath + HomeController.ImagePath)) });
		}
	}
}