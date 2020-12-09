// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessagePackCSharpTarget.cs">
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
	internal class MessagePackCSharp : ASerializer<byte[]>
	{
		#region GenericSerialization
		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override T Deserialize<T>(byte[] bytes)
		{
			return MessagePack.MessagePackSerializer.Deserialize<T>(bytes);
		}
		#endregion
		
		#region Non-GenericSerialization
		protected override byte[] Serialize(Type type, object original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(type, original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override object Deserialize(Type type, byte[] bytes)
		{
			return MessagePack.MessagePackSerializer.Deserialize(type, bytes);
		}
		#endregion

		public override string ToString()
		{
			return "MessagePackCSharp";
		}
	}
}