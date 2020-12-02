// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Lz4MessagePackCSharpTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using MessagePack;

namespace SerializationBenchmark
{
	internal class Lz4MessagePackCSharpTarget : ASerializerTarget<byte[]>
	{
		MessagePackSerializerOptions lz4Options;

		public Lz4MessagePackCSharpTarget() : base()
		{
			lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
		}

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(original, lz4Options);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override T Deserialize<T>(byte[] bytes)
		{
			return MessagePack.MessagePackSerializer.Deserialize<T>(bytes, lz4Options);
		}

		public override string ToString()
		{
			return "Lz4MessagePackCSharp";
		}
	}
}