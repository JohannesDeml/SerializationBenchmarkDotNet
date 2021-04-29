// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidSerializationException.cs">
//   Copyright (c) 2021 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SerializationBenchmark
{
	/// <summary>
	/// Thrown when the serialization result does not match the original serialization target
	/// </summary>
	[Serializable]
	public class InvalidSerializationException : Exception
	{
		public InvalidSerializationException() : base() { }
		public InvalidSerializationException(string message) : base(message) { }
		public InvalidSerializationException(string message, Exception inner) : base(message, inner) { }
		
		protected InvalidSerializationException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}