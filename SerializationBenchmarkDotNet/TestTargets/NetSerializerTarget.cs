// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetSerializerTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;

namespace DotNetSerializationBenchmark
{
	internal class NetSerializerTarget : ASerializerTarget
	{
		MemoryStream stream = null;
		private NetSerializer.Serializer netSerializer;

		public NetSerializerTarget(): base()
		{
			// This needs to be extended, if more types are added for testing
			var rootTypes = new[] {typeof(Person[]), typeof(Vector3[])};
			netSerializer = new NetSerializer.Serializer(rootTypes);
		}

		public override void Cleanup()
		{
			stream = null;
		}

		protected override long Serialize<T>(T original)
		{
			netSerializer.SerializeDirect<T>(stream = new MemoryStream(), original);
			return stream.Position;
		}

		protected override T Deserialize<T>()
		{
			T copy = default(T);
			stream.Position = 0;
			netSerializer.DeserializeDirect<T>(stream, out copy);
			return copy;
		}
	}
}