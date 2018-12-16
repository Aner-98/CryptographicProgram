using System.Drawing;
using CryptographicProgram.Algorithms.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CryptographicProgram.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class SteganographyController : ControllerBase
    {
	    private string _imageName => HomeController.ImagePath;
		private readonly ISteganographyAlgorithm _steganographyAlgorithm;

	    public SteganographyController(ISteganographyAlgorithm steganographyAlgorithm)
	    {
		    _steganographyAlgorithm = steganographyAlgorithm;
	    }

	    [HttpGet]
	    [Route("GetEncode")]
	    public IActionResult EncodeText([FromQuery]string text)
	    {
		    _steganographyAlgorithm.Encode(new Bitmap(_imageName), text).Save(_imageName);
			return Ok();
	    }

	    [HttpGet]
	    [Route("SecretText")]
	    public IActionResult GetSecretText()
	    {
		    return Ok(_steganographyAlgorithm.Decode(new Bitmap(_imageName)));
	    }
	}
}