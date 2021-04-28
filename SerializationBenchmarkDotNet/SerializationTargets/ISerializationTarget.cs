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
using Google.Protobuf;

namespace SerializationBenchmark
{
	public interface ISerializationTarget : IEquatable<ISerializationTarget>
	{
		Type GetType();
		long Serialize(ISerializer serializer);
		long Deserialize(ISerializer serializer);
		long Serialize(ref byte[] target);
		long Deserialize(ref byte[] target);
		string ToReadableString();

		void GenerateProtobufMessage();
		IMessage GetProtobufMessage();
	}
}