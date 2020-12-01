// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProtobufNetTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;

namespace DotNetSerializationBenchmark
{
	internal class ProtobufNetTarget : ASerializerTarget
	{
		MemoryStream stream = null;

		public override void Cleanup()
		{
			stream = null;
		}

		protected override long Serialize<T>(T original)
		{
			ProtoBuf.Serializer.Serialize<T>(stream = new MemoryStream(), original);
			return stream.Position;
		}

		protected override T Deserialize<T>()
		{
			T copy = default(T);
			stream.Position = 0;
			copy = ProtoBuf.Serializer.Deserialize<T>(stream);
			return copy;
		}
	}
}