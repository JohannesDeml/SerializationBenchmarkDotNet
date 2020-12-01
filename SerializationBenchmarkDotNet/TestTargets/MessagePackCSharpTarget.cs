// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessagePackCSharpTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetSerializationBenchmark
{
	internal class MessagePackCSharpTarget : ATestTarget
	{
		byte[] bytes;
		protected override long Serialize<T>(T original)
		{
			bytes = MessagePack.MessagePackSerializer.Serialize(original);
			return bytes.Length;
		}

		protected override T Deserialize<T>()
		{
			return MessagePack.MessagePackSerializer.Deserialize<T>(bytes);
		}
	}
}