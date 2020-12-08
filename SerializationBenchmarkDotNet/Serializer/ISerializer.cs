﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializerTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SerializationBenchmark
{
	public interface ISerializer
	{
		long BenchmarkSerialize<T>(T original);
		long BenchmarkSerialize(Type type, object original);
		long BenchmarkDeserialize<T>(T original);
		long BenchmarkDeserialize(Type type, object original);
		bool Validate<T>(T original) where T : IEquatable<T>;
		bool Validate(Type type, object original);
		void Cleanup();
	}
}