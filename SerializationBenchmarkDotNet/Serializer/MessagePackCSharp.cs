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

		public override string ToString()
		{
			return "MessagePackCSharp";
		}
	}
}