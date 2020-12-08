// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MsgPackTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace SerializationBenchmark
{
	internal class MsgPack : ASerializer<byte[]>
	{
		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = global::MsgPack.Serialization.MessagePackSerializer.Get<T>().PackSingleObject(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override T Deserialize<T>(byte[] bytes)
		{
			return global::MsgPack.Serialization.MessagePackSerializer.Get<T>().UnpackSingleObject(bytes);
		}

		public override string ToString()
		{
			return "MsgPack";
		}
	}
}