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

namespace DotNetSerializationBenchmark
{
	internal class Lz4MessagePackCSharpTarget : ASerializerTarget
	{
		byte[] bytes;
		MessagePackSerializerOptions lz4Options;

		public Lz4MessagePackCSharpTarget(): base()
		{
			lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
		}

		public override void Cleanup()
		{
			bytes = null;
		}

		protected override long Serialize<T>(T original)
		{
			bytes = MessagePack.MessagePackSerializer.Serialize(original, lz4Options);
			return bytes.Length;
		}

		protected override T Deserialize<T>()
		{
			return MessagePack.MessagePackSerializer.Deserialize<T>(bytes, lz4Options);
		}
	}
}