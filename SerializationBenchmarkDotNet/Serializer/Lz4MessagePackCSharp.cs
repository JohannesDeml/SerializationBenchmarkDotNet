// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Lz4MessagePackCSharp.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using MessagePack;

namespace SerializationBenchmark
{
	internal class Lz4MessagePackCSharp : ADirectSerializer<byte[]>
	{
		readonly MessagePackSerializerOptions lz4Options;

		public Lz4MessagePackCSharp() : base()
		{
			lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
		}

		#region Serialization

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(original, lz4Options);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(type, original, lz4Options);
			messageSize = bytes.Length;
			return bytes;
		}

		#endregion

		#region Deserialization

		protected override ISerializationTarget Deserialize<T>(byte[] bytes)
		{
			return MessagePack.MessagePackSerializer.Deserialize<T>(bytes, lz4Options);
		}

		protected override ISerializationTarget Deserialize(Type type, byte[] bytes)
		{
			return (ISerializationTarget) MessagePack.MessagePackSerializer.Deserialize(type, bytes, lz4Options);
		}

		#endregion

		public override string ToString()
		{
			return "Lz4MessagePack-CSharp";
		}
	}
}