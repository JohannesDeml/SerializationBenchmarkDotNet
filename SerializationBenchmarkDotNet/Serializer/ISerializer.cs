// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializerTarget.cs">
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
		long BenchmarkSerialize<T>(T original) where T: ISerializationTarget;
		long BenchmarkSerialize(Type type, ISerializationTarget original);
		long BenchmarkDeserialize<T>(T original) where T: ISerializationTarget;
		long BenchmarkDeserialize(Type type, ISerializationTarget original);
		bool Validate<T>(T original) where T : ISerializationTarget;
		bool Validate(Type type, ISerializationTarget original);
		void Cleanup();
	}
}