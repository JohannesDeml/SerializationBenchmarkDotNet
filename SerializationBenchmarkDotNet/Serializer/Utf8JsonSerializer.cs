// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utf8JsonSerializer.cs">
//   Copyright (c) 2023 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SerializationBenchmark;

internal class Utf8JsonSerializer : ADirectSerializer<byte[]>
{
	#region Serialization

	protected override byte[] Serialize<T>(T original, out long messageSize)
	{
		var bytes = Utf8Json.JsonSerializer.Serialize(original);
		messageSize = bytes.Length;
		return bytes;
	}

	protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
	{
		var bytes = Utf8Json.JsonSerializer.Serialize(original);
		messageSize = bytes.Length;
		return bytes;
	}

	#endregion

	#region Deserialization

	protected override ISerializationTarget Deserialize<T>(byte[] bytes)
	{
		return Utf8Json.JsonSerializer.Deserialize<T>(bytes);
	}

	protected override ISerializationTarget Deserialize(Type type, byte[] bytes)
	{
		if (type == typeof(Vector3))
		{
			return Utf8Json.JsonSerializer.Deserialize<Vector3>(bytes);
		}

		if (type == typeof(Person))
		{
			return Utf8Json.JsonSerializer.Deserialize<Person>(bytes);
		}
		
		throw new NotImplementedException($"Deserialization for type {type} not implemented!");
	}

	#endregion

	public override string ToString()
	{
		return "Utf8Json";
	}
}