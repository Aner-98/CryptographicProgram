using System.IO;

namespace CryptographicProgram.Algorithms.Abstractions
{
	public interface ISteganographyAlgorithm
	{
		byte[] Encode(FileInfo fileInfo, string text);
		string Decode(FileInfo fileInfo);
	}
}
