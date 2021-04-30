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
		/// <summary>
		/// Write a string to a byte buffer, allows for string lengths up to 255 characters
		/// </summary>
		/// <param name="target">Buffer to write to</param>
		/// <param name="value">String that will be ASCII converted and written</param>
		/// <param name="maxLength">Maximum length to write</param>
		/// <param name="pos">position in the byte buffer to write to, updates to the position right after the last bit written to the buffer</param>
		public static void WriteString(byte[] target, string value, int maxLength, ref int pos)
		{
			var length = Math.Clamp(value.Length, 0, maxLength);
			target.Write((ByteConverter) length, ref pos, 8);
			
			var bytePos = pos / 8;
			var stringBytes = System.Text.Encoding.ASCII.GetBytes(value, 0,  length);
			stringBytes.CopyTo(target, bytePos);
			pos += length * 8;
		}

		/// <summary>
		/// Reads a string from the byte buffer
		/// </summary>
		/// <param name="target">Buffer to read from</param>
		/// <param name="maxLength">Maximum expected length</param>
		/// <param name="pos">Position in the buffer to read from, moves to the position behind the string</param>
		/// <returns></returns>
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