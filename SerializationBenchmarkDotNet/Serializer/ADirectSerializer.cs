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
	public abstract class ADirectSerializer<TSerialization> : ASerializer<TSerialization, ISerializationTarget>
	{
		protected override bool GetResult(Type type, out ISerializationTarget result)
		{
			return deserializationResults.TryGetValue(type, out result);
		}
	}
}