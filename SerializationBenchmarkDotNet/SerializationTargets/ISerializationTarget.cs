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
		/// <summary>
		/// Get the type of the serialization target
		/// </summary>
		/// <returns>Class type</returns>
		Type GetType();
		
		/// <summary>
		/// Trigger generic serialization of this instance through a defined serializer
		/// </summary>
		/// <param name="serializer">Serializer that will serialize this instance</param>
		/// <returns>Serialization size in bytes</returns>
		long Serialize(ISerializer serializer);
		/// <summary>
		/// Trigger generic deserialization of this instance through a defined serializer
		/// </summary>
		/// <param name="serializer">Serializer that will deserialize this instance</param>
		/// <returns>Serialization size in bytes</returns>
		long Deserialize(ISerializer serializer);
		
		/// <summary>
		/// Trigger custom defined serialization (ManualBitPacking)
		/// </summary>
		/// <param name="target">byte buffer to write to</param>
		/// <returns>Bytes written = Serialization size</returns>
		long Serialize(ref byte[] target);
		/// <summary>
		/// Trigger custom defined deserialization (ManualBitPacking)
		/// </summary>
		/// <param name="target">byte buffer to read from</param>
		/// <returns>Bytes read = Serialization size</returns>
		long Deserialize(ref byte[] target);
		
		/// <summary>
		/// Generate a readable string for debug purposes
		/// </summary>
		/// <returns>A string exposing the values of this instance</returns>
		string ToReadableString();

		/// <summary>
		/// Create a protobuf message, necessary to call this once before calling GetProtobufMessage
		/// </summary>
		void GenerateProtobufMessage();
		/// <summary>
		/// Get the protobuf instance equivalent of this class
		/// </summary>
		/// <returns>A protobuf instance with the same values of this instance</returns>
		IMessage GetProtobufMessage();
	}
}