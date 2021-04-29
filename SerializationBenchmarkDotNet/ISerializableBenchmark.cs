// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializableBenchmark.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace SerializationBenchmark
{
	/// <summary>
	/// Benchmark target for BenchmarkDotNet (bdn)
	/// Allows for access to serializer and target selected by bdn to get the serialization size
	/// </summary>
	public interface ISerializableBenchmark
	{
		ISerializer Serializer { get; set; }
		ISerializationTarget Target { get; set; }
		bool Generic { get; set; }

		void PrepareBenchmark();
		long Serialize();
	}
}