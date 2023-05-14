// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializer.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SerializationBenchmark
{
	public interface ISerializer
	{
		/// <summary>
		/// Generic serialization for a serialization target
		/// </summary>
		/// <param name="original">the instance to serialize</param>
		/// <typeparam name="T">Class type of original</typeparam>
		/// <returns>Serialization size in bytes</returns>
		long BenchmarkSerialize<T>(T original);
		/// <summary>
		/// Non-generic serialization for a serialization target
		/// </summary>
		/// <param name="type">Class type of original</param>
		/// <param name="original">the instance to serialize</param>
		/// <returns>Serialization size in bytes</returns>
		long BenchmarkSerialize(Type type, object original);
		
		/// <summary>
		/// Generic deserialization for a serialization target
		/// </summary>
		/// <param name="original">the instance to deserialize</param>
		/// <typeparam name="T">Class type of original</typeparam>
		/// <returns>Serialization size in bytes</returns>
		long BenchmarkDeserialize<T>(T original);
		/// <summary>
		/// Non-generic deserialization for a serialization target
		/// </summary>
		/// <param name="type">Class type of original</param>
		/// <param name="original">the instance to deserialize</param>
		/// <returns>Serialization size in bytes</returns>
		long BenchmarkDeserialize(Type type, object original);
		
		/// <summary>
		/// Validate if original has the same values as the stored result from deserialization
		/// </summary>
		/// <param name="type">Class type of original</param>
		/// <param name="original">Original instance to compare against</param>
		/// <returns>True if original matches with the stored deserialization result</returns>
		bool Validate(Type type, object original);
		
		/// <summary>
		/// Clear all stored serialization/deserialization results
		/// </summary>
		void Cleanup();
	}
}