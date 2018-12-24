using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using CryptographicProgram.Algorithms.Abstractions;

namespace CryptographicProgram.Algorithms.Implementations
{
	public class GifLsb : ISteganographyAlgorithm
	{
		private readonly byte[] _checkSequence = new byte[] { 0, 1, 0, 1, 0, 1, 0, 1 };
		private readonly int _firstLSBit = 0;
		private readonly int _secondLSBit = 1;


		private byte ConvertToByte2(BitArray bits)
		{
			byte ans = 0;
			byte pow = 0;

			for (int i = bits.Length - 1; i >= 0; i--)
			{
				if (bits[i])
				{
					ans += Convert.ToByte(Math.Pow(2, pow));
				}
				pow++;
			}
			return ans;
		}

		byte ConvertToByte(BitArray bits)
		{
			byte[] bytes = new byte[1];
			bits.CopyTo(bytes, 0);
			return bytes[0];
		}

		private bool GetBit(byte b, int bitNumber)
		{
			return (b & (1 << bitNumber)) != 0;
		}

		private bool[] GetByteAsArrayOfBits(byte b)
		{
			var array = new bool[8];

			for (var i = 0; i < 8; i++)
			{
				array[i] = GetBit(b, i);
			}

			return array;
		}

		public byte[] Encode(FileInfo fileInfo, string text)
		{
			if (fileInfo == null)
			{
				throw new Exception("Input file is null");
			}

			if (text == null)
			{
				throw new Exception("Text is null");
			}

			// read bytes from input file
			byte[] bytes = File.ReadAllBytes(fileInfo.FullName);

			// check format
			if (Encoding.ASCII.GetString(bytes.Take(6).ToArray()) != "GIF89a")
			{
				throw new Exception("Input file has wrong GIF format");
			}

			// read palette size property from first three bits in the 10-th byte from the file
			var b10 = new BitArray(bytes[10]);
			var bsize = ConvertToByte(new BitArray(new bool[] { GetBit(bytes[10], 0), GetBit(bytes[10], 1), GetBit(bytes[10], 2) }));

			// calculate color count and possible message length
			var bOrigColorCount = (int)Math.Pow(2, bsize + 1);
			var possibleMessageLength = bOrigColorCount * 3 / 4;
			var possibleTextLength = possibleMessageLength - 2; // one byte for check and one byte for message length

			if (possibleTextLength < text.Length)
			{
				throw new Exception("Text is too big");
			}

			var n = 13;

			// write check sequence
			for (var i = 0; i < _checkSequence.Length / 2; i++)
			{
				var ba = new BitArray(GetByteAsArrayOfBits(bytes[n]));
				ba[_firstLSBit] = Convert.ToBoolean(_checkSequence[2 * i]);
				ba[_secondLSBit] = Convert.ToBoolean(_checkSequence[2 * i + 1]);
				bytes[n] = ConvertToByte(ba);
				n++;
			}

			// write text length
			var cl = new BitArray(GetByteAsArrayOfBits((byte) text.Length));
			for (int i = 0; i < cl.Length / 2; i++)
			{
				var ba = new BitArray(GetByteAsArrayOfBits(bytes[n]));
				ba[_firstLSBit] = cl[2 * i];
				ba[_secondLSBit] = cl[2 * i + 1];
				bytes[n] = ConvertToByte(ba);
				n++;
			}

			// write message
			byte[] textBytes = Encoding.ASCII.GetBytes(text);

			for (int i = 0; i < textBytes.Length; i++)
			{
				var c = new BitArray(GetByteAsArrayOfBits(textBytes[i]));
				for (int ci = 0; ci < c.Length / 2; ci++)
				{
					var ba = new BitArray(GetByteAsArrayOfBits(bytes[n]));
					ba[_firstLSBit] = c[2 * ci];
					ba[_secondLSBit] = c[2 * ci + 1];
					bytes[n] = ConvertToByte(ba);
					n++;
				}
			}

			return bytes;
		}

		public string Decode(FileInfo fileInfo)
		{
			if (fileInfo == null)
			{
				throw new Exception("Input file is null");
			}

			// read bytes from input file
			byte[] bytes = File.ReadAllBytes(fileInfo.FullName);

			// check format
			// check format
			if (Encoding.ASCII.GetString(bytes.Take(6).ToArray()) != "GIF89a")
			{
				throw new Exception("Input file has wrong GIF format");
			}

			// read palette size property from first three bits in the 10-th byte from the file
			var b10 = new BitArray(bytes[10]);
			var bsize = ConvertToByte(new BitArray(new bool[] { GetBit(bytes[10], 0), GetBit(bytes[10], 1), GetBit(bytes[10], 2) }));

			// calculate color count and possible message length
			int bOrigColorCount = (int)Math.Pow(2, bsize + 1);
			int possibleMessageLength = bOrigColorCount * 3 / 4;
			int possibleTextLength = possibleMessageLength - 2; // one byte for check and one byte for message length

			int n = 13;

			// read check sequence
			BitArray csBits = new BitArray(_checkSequence.Length);
			for (int i = 0; i < 4; i++)
			{
				var ba = new BitArray(GetByteAsArrayOfBits(bytes[n]));
				csBits[2 * i] = ba[_firstLSBit];
				csBits[2 * i + 1] = ba[_secondLSBit];
				n++;
			}

			byte cs = ConvertToByte(csBits);

			if (cs != ConvertToByte(new BitArray(new bool[]
			{
				Convert.ToBoolean(_checkSequence[0]),
				Convert.ToBoolean(_checkSequence[1]),
				Convert.ToBoolean(_checkSequence[2]),
				Convert.ToBoolean(_checkSequence[3]),
				Convert.ToBoolean(_checkSequence[4]),
				Convert.ToBoolean(_checkSequence[5]),
				Convert.ToBoolean(_checkSequence[6]),
				Convert.ToBoolean(_checkSequence[7]),
			})))
			{
				throw new Exception(
					"There is no encrypted message in the image (Check sequence is incorrect)");
			}

			// read text length
			BitArray cl = new BitArray(8);
			for (int i = 0; i < 4; i++)
			{
				var ba = new BitArray(GetByteAsArrayOfBits(bytes[n]));
				cl[2 * i] = ba[_firstLSBit];
				cl[2 * i + 1] = ba[_secondLSBit];
				n++;
			}

			byte textLength = ConvertToByte(cl);

			if (possibleTextLength < textLength)
			{
				throw new Exception("There is no messages (Decoded message length (" + textLength +
									") is less than Possible message length (" + possibleTextLength +
									"))");
			}

			// read text bits and make text bytes
			byte[] bt = new byte[textLength];
			for (int i = 0; i < bt.Length; i++)
			{
				BitArray bc = new BitArray(8);
				for (int bci = 0; bci < bc.Length / 2; bci++)
				{
					var ba = new BitArray(GetByteAsArrayOfBits(bytes[n]));
					bc[2 * bci] = ba[_firstLSBit];
					bc[2 * bci + 1] = ba[_secondLSBit];
					n++;
				}

				bt[i] = ConvertToByte(bc);
			}

			return Encoding.ASCII.GetString(bt);
		}
	}
}