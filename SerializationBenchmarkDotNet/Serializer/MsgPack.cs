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
		#region Serialization

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = global::MsgPack.Serialization.MessagePackSerializer.Get<T>().PackSingleObject(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override byte[] Serialize(Type type, object original, out long messageSize)
		{
			var bytes = global::MsgPack.Serialization.MessagePackSerializer.Get(type).PackSingleObject(original);
			messageSize = bytes.Length;
			return bytes;
		}

		#endregion

		#region Deserialization

		protected override object Deserialize<T>(byte[] bytes)
		{
			return global::MsgPack.Serialization.MessagePackSerializer.Get<T>().UnpackSingleObject(bytes);
		}

		protected override object Deserialize(Type type, object bytes)
		{
			return (object) global::MsgPack.Serialization.MessagePackSerializer.Get(type).UnpackSingleObject((byte[])bytes);
		}

		#endregion

		public override string ToString()
		{
			return "MsgPack-CLI";
		}
	}
}