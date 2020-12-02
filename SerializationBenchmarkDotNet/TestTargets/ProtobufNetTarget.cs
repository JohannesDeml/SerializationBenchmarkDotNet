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
	internal class ProtobufNetTarget : ASerializerTarget<MemoryStream>
	{
		protected override MemoryStream Serialize<T>(T original, out long messageSize)
		{
			var stream = new MemoryStream();
			ProtoBuf.Serializer.Serialize<T>(stream, original);
			messageSize = stream.Position;
			return stream;
		}

		protected override T Deserialize<T>(MemoryStream stream)
		{
			T copy = default(T);
			stream.Position = 0;
			copy = ProtoBuf.Serializer.Deserialize<T>(stream);
			return copy;
		}
	}
}