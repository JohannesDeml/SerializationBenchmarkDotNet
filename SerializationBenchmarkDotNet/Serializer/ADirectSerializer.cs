// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ADirectSerializer.cs">
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
	/// <summary>
	/// Inherit from this class if the serializer deserializes to the original <see cref="ISerializationTarget"/> classes
	/// </summary>
	/// <typeparam name="TSerialization"></typeparam>
	public abstract class ADirectSerializer<TSerialization> : ASerializer<TSerialization, object>
	{
		protected override bool GetResult(Type type, out object result)
		{
			return DeserializationResults.TryGetValue(type, out result);
		}
	}
}