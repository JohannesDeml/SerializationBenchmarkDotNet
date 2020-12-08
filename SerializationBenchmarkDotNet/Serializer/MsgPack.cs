// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MsgPackTarget.cs">
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
	internal class MsgPack : ASerializer<byte[]>
	{
		protected override byte[] Serialize(Type type, object original, out long messageSize)
		{
			var bytes = global::MsgPack.Serialization.MessagePackSerializer.Get(type).PackSingleObject(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override object Deserialize(Type type, byte[] bytes)
		{
			return global::MsgPack.Serialization.MessagePackSerializer.Get(type).UnpackSingleObject(bytes);
		}

		public override string ToString()
		{
			return "MsgPack";
		}
	}
}