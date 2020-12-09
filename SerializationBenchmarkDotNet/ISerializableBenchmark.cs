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
	public interface ISerializableBenchmark
	{
		ISerializer Serializer { get; set; }
		ISerializationTarget Target { get; set; }
		bool Generic { get; set; }

		void PrepareBenchmark();
		long Serialize();
	}
}