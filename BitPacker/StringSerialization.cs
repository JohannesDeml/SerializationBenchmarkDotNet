// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerialization.cs">
//   Copyright (c) 2021 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace emotitron.Compression
{
	public static class StringSerialization
	{
		public static void WriteString(byte[] target, string value, int maxLength, ref int pos)
		{
			var length = Math.Min(value.Length, maxLength);
			target.Write((ByteConverter) length, ref pos, 8);
			
			var bytePos = pos / 8;
			var stringBytes = System.Text.Encoding.ASCII.GetBytes(value, 0,  length);
			stringBytes.CopyTo(target, bytePos);
			pos += length * 8;
		}

		public static string ReadString(byte[] target, int maxLength, ref int pos)
		{
			var length = Math.Min((int) target.Read(ref pos, 8), maxLength);
			
			var bytePos = pos / 8;
			var stringValue = System.Text.Encoding.ASCII.GetString(target, bytePos, length);
			pos += length * 8;
			return stringValue;
		}
	}
}