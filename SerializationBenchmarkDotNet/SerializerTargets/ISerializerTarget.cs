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
	public interface ISerializerTarget
	{
		long BenchmarkSerialize<T>(T original);
		long BenchmarkDeserialize<T>(T original);
		bool Validate<T>(T original) where T : IEquatable<T>;
		bool ValidateArray<T>(T[] array);
		void Cleanup();
	}
}