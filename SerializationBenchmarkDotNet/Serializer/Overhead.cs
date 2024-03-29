﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Overhead.cs">
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
	/// Shows the overhead of the benchmark base class, mainly dictionary lookups
	/// Every serializer deriving from <see cref="ASerializer{TSerialization,TDeserialization}"/> has this overhead
	/// </summary>
	internal class Overhead : ADirectSerializer<ISerializationTarget>
	{
		#region Serialization

		protected override ISerializationTarget Serialize<T>(T original, out long messageSize)
		{
			messageSize = 0;
			return original;
		}

		protected override ISerializationTarget Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			messageSize = 0;
			return original;
		}

		#endregion

		#region Deserialization

		protected override ISerializationTarget Deserialize<T>(ISerializationTarget original)
		{
			return original;
		}

		protected override ISerializationTarget Deserialize(Type type, ISerializationTarget original)
		{
			return original;
		}

		#endregion

		public override string ToString()
		{
			return "Overhead";
		}
	}
}