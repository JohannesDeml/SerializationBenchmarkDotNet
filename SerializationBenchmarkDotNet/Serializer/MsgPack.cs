// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MsgPack.cs">
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
	internal class MsgPack : ADirectSerializer<byte[]>
	{
		#region GenericSerialization

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = global::MsgPack.Serialization.MessagePackSerializer.Get<T>().PackSingleObject(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override ISerializationTarget Deserialize<T>(byte[] bytes)
		{
			return global::MsgPack.Serialization.MessagePackSerializer.Get<T>().UnpackSingleObject(bytes);
		}

		#endregion

		#region Non-GenericSerialization

		protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			var bytes = global::MsgPack.Serialization.MessagePackSerializer.Get(type).PackSingleObject(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override ISerializationTarget Deserialize(Type type, byte[] bytes)
		{
			return (ISerializationTarget) global::MsgPack.Serialization.MessagePackSerializer.Get(type).UnpackSingleObject(bytes);
		}

		#endregion

		public override string ToString()
		{
			return "MsgPack";
		}
	}
}