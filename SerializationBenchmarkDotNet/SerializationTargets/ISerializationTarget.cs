// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializationTarget.cs">
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
	public interface ISerializationTarget
	{
		Type GetType();
	}
}