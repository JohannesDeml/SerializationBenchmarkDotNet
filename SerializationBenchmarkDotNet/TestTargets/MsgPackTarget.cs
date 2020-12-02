﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MsgPackTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace DotNetSerializationBenchmark
{
	internal class MsgPackTarget : ASerializerTarget<byte[]>
	{
		public override void Cleanup()
		{
		}
		
		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = MsgPack.Serialization.MessagePackSerializer.Get<T>().PackSingleObject(original);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override T Deserialize<T>(byte[] bytes)
		{
			return MsgPack.Serialization.MessagePackSerializer.Get<T>().UnpackSingleObject(bytes);
		}
	}
}