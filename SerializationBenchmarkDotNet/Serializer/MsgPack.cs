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
		#region GenericSerialization
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
		#endregion
		
		#region Non-GenericSerialization
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
		#endregion

		public override string ToString()
		{
			return "MsgPack";
		}
	}
}