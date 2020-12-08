// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessagePackCSharpTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace SerializationBenchmark
{
	internal class MessagePackCSharp : ASerializer<byte[]>
	{
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

		public override string ToString()
		{
			return "MessagePackCSharp";
		}
	}
}