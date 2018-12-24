using System.Drawing;
using System.IO;
using CryptographicProgram.Algorithms.Abstractions;
using CryptographicProgram.Enums;

namespace CryptographicProgram.Algorithms.Implementations
{
	public class Lsb : ISteganographyAlgorithm
	{
		public byte[] Encode(FileInfo fileInfo, string text)
		{
			var state = BitmapState.Hiding;
			var charIndex = 0;
			var charValue = 0;
			long pixelElementIndex = 0;
			var zeros = 0;
			var bitmap = new Bitmap(fileInfo.FullName);

			for (var i = 0; i < bitmap.Height; i++)
			{
				for (var j = 0; j < bitmap.Width; j++)
				{
					var pixel = bitmap.GetPixel(j, i);

					var red = pixel.R - pixel.R % 2;
					var green = pixel.G - pixel.G % 2;
					var blue = pixel.B - pixel.B % 2;

					for (var n = 0; n < 3; n++)
					{
						if (pixelElementIndex % 8 == 0)
						{
							if (state == BitmapState.FillingWithZeros && zeros == 8)
							{
								if ((pixelElementIndex - 1) % 3 < 2)
								{
									bitmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
								};
							}

							if (charIndex >= text.Length)
							{
								state = BitmapState.FillingWithZeros;
							}
							else
							{
								charValue = text[charIndex++];
							}
						}

						switch (pixelElementIndex % 3)
						{
							case 0:
								{
									if (state == BitmapState.Hiding)
									{
										red += charValue % 2;
										charValue /= 2;
									}
								}
								break;
							case 1:
								{
									if (state == BitmapState.Hiding)
									{
										green += charValue % 2;

										charValue /= 2;
									}
								}
								break;
							case 2:
								{
									if (state == BitmapState.Hiding)
									{
										blue += charValue % 2;

										charValue /= 2;
									}

									bitmap.SetPixel(j, i, Color.FromArgb(red, green, blue));
								}
								break;
						}

						pixelElementIndex++;

						if (state == BitmapState.FillingWithZeros)
						{
							zeros++;
						}
					}
				}
			}

			using (var stream = new MemoryStream())
			{
				bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
				return stream.ToArray();
			}
		}

		public string Decode(FileInfo fileInfo)
		{
			var colorUnitIndex = 0;
			var charValue = 0;
			var extractedText = string.Empty;
			var bitmap = new Bitmap(fileInfo.FullName);

			for (var i = 0; i < bitmap.Height; i++)
			{
				for (var j = 0; j < bitmap.Width; j++)
				{
					var pixel = bitmap.GetPixel(j, i);

					for (var n = 0; n < 3; n++)
					{
						switch (colorUnitIndex % 3)
						{
							case 0:
								{
									charValue = charValue * 2 + pixel.R % 2;
								}
								break;
							case 1:
								{
									charValue = charValue * 2 + pixel.G % 2;
								}
								break;
							case 2:
								{
									charValue = charValue * 2 + pixel.B % 2;
								}
								break;
						}

						colorUnitIndex++;

						if (colorUnitIndex % 8 == 0)
						{
							charValue = ReverseBits(charValue);

							if (charValue == 0)
							{
								return extractedText;
							}

							var c = (char)charValue;

							extractedText += c.ToString();
						}
					}
				}
			}
			return extractedText;
		}

		private static int ReverseBits(int n)
		{
			var result = 0;

			for (var i = 0; i < 8; i++)
			{
				result = result * 2 + n % 2;

				n /= 2;
			}

			return result;
		}
	}
}
