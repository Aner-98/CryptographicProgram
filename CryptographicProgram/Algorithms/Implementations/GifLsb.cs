using System;
using System.Drawing;
using CryptographicProgram.Algorithms.Abstractions;

namespace CryptographicProgram.Algorithms.Implementations
{
	public class GifLsb : ISteganographyAlgorithm
	{
		public Bitmap Encode(Bitmap bitmap, string text)
		{
			throw new NotImplementedException();
		}

		public string Decode(Bitmap bitmap)
		{
			throw new NotImplementedException();
		}
	}
}
