using System;
using System.Drawing;
using CryptographicProgram.Algorithms.Abstractions;

namespace CryptographicProgram.Algorithms.Implementations
{
	public class SimpleLsb : ISteganographyAlgorithm
	{
		public Bitmap Encode(Bitmap bitmap, string text)
		{
			for (var i = 0; i < bitmap.Width; i++)
			{
				for (var j = 0; j < bitmap.Height; j++)
				{
					var pixel = bitmap.GetPixel(i, j);

					if (i < 1 && j < text.Length)
					{
						var letter = Convert.ToChar(text.Substring(j, 1));
						var value = Convert.ToInt32(letter);

						bitmap.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, value));
					}

					if (i == bitmap.Width - 1 && j == bitmap.Height - 1)
					{
						bitmap.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, text.Length));
					}
				}
			}

			bitmap.Save(Guid.NewGuid().ToString() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

			return bitmap;
		}

		public string Decode(Bitmap bitmap)
		{
			var message = "";

			var lastPixel = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1);
			int msgLength = lastPixel.B;

			for (var i = 0; i < bitmap.Width; i++)
			{
				for (var j = 0; j < bitmap.Height; j++)
				{
					var pixel = bitmap.GetPixel(i, j);

					if (i >= 1 || j >= msgLength)
					{
						continue;
					}

					int value = pixel.B;
					var c = Convert.ToChar(value);
					var letter = char.ToString(c);
					message = message + letter;
				}
			}

			return message;
		}

	}
}
