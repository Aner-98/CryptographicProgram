using CryptographicProgram.Algorithms.Abstractions;
using CryptographicProgram.Algorithms.Implementations;

namespace CryptographicProgram
{
	public class AlgorithmFactory : IAlgorithmFactory
	{
		public ISteganographyAlgorithm GetAlgorithm(string extension)
		{
			if (extension.ToLower() == ".gif")
			{
				return new GifLsb();
			}

			return new SimpleLsb();
		}
	}
}
