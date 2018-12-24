using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicProgram.Algorithms.Abstractions;

namespace CryptographicProgram
{
	public interface IAlgorithmFactory
	{
		ISteganographyAlgorithm GetAlgorithm(string extension);
	}
}
