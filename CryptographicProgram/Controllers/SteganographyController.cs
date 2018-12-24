using System;
using System.IO;
using CryptographicProgram.Algorithms.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;


namespace CryptographicProgram.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SteganographyController : ControllerBase
	{
		private ISteganographyAlgorithm _steganographyAlgorithm;
		private readonly IHostingEnvironment _appEnvironment;
		private readonly IAlgorithmFactory _algorithmFactory;


		public SteganographyController(IAlgorithmFactory algorithmFactory, IHostingEnvironment appEnvironment)
		{
			_appEnvironment = appEnvironment;
			_algorithmFactory = algorithmFactory;
		}

		[HttpGet]
		[Route("GetEncode")]
		public IActionResult EncodeText([FromQuery]string text)
		{
			var file = new FileInfo(_appEnvironment.WebRootPath + HomeController.ImagePath);
			var extension = file.Extension.ToLower() == ".gif" ? ".gif" : ".bmp";
			_steganographyAlgorithm = _algorithmFactory.GetAlgorithm(extension);

			var fileBytes = _steganographyAlgorithm.Encode(file, text);


			var newFileName = Guid.NewGuid().ToString() + extension;
			var newFilePath = _appEnvironment.WebRootPath + @"\images\" + newFileName;

			System.IO.File.WriteAllBytes(newFilePath, fileBytes);

			HomeController.ImagePath = "/images/" + newFileName;
			return Ok();
		}

		[HttpGet]
		[Route("SecretText")]
		public IActionResult GetSecretText()
		{
			var file = new FileInfo(_appEnvironment.WebRootPath + HomeController.ImagePath);
			var extension = file.Extension.ToLower() == ".gif" ? ".gif" : ".bmp";
			_steganographyAlgorithm = _algorithmFactory.GetAlgorithm(extension);

			return Ok(new { text = _steganographyAlgorithm.Decode(file) });
		}
	}
}