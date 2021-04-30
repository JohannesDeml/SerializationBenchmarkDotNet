// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessagePackCSharp.cs">
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
	internal class MessagePackCSharp : ADirectSerializer<byte[]>
	{
		#region Serialization

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(type, original);
			messageSize = bytes.Length;
			return bytes;
		}

		#endregion

		#region Deserialization

		protected override ISerializationTarget Deserialize<T>(byte[] bytes)
		{
			return MessagePack.MessagePackSerializer.Deserialize<T>(bytes);
		}

		protected override ISerializationTarget Deserialize(Type type, byte[] bytes)
		{
			return (ISerializationTarget) MessagePack.MessagePackSerializer.Deserialize(type, bytes);
		}

		#endregion

		public override string ToString()
		{
			return "MessagePack-CSharp";
		}
	}
}