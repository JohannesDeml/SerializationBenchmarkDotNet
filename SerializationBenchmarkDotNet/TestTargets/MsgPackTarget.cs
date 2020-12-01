// --------------------------------------------------------------------------------------------------------------------
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
	internal class MsgPackTarget : ATestTarget
	{
		byte[] bytes;
		protected override long Serialize<T>(T original)
		{
			bytes = MsgPack.Serialization.MessagePackSerializer.Get<T>().PackSingleObject(original);
			return bytes.Length;
		}

		protected override T Deserialize<T>()
		{
			return MsgPack.Serialization.MessagePackSerializer.Get<T>().UnpackSingleObject(bytes);
		}
	}
}