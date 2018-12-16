using System.Drawing;

namespace CryptographicProgram.Algorithms.Abstractions
{
	public interface ISteganographyAlgorithm
	{
		Bitmap Encode(Bitmap bitmap, string text);
		string Decode(Bitmap bitmap);
	}
}
