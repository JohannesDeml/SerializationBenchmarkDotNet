// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetSerializerTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;

namespace SerializationBenchmark
{
	internal class NetSerializer : ASerializer<MemoryStream>
	{
		private global::NetSerializer.Serializer netSerializer;

		public NetSerializer() : base()
		{
			// This needs to be extended, if more types are added for testing
			var rootTypes = new[] {typeof(Person), typeof(Vector3)};
			netSerializer = new global::NetSerializer.Serializer(rootTypes);
		}

		protected override MemoryStream Serialize<T>(T original, out long messageSize)
		{
			var stream = new MemoryStream();
			netSerializer.SerializeDirect<T>(stream, original);
			messageSize = stream.Position;
			return stream;
		}

		protected override T Deserialize<T>(MemoryStream stream)
		{
			T copy = default(T);
			stream.Position = 0;
			netSerializer.DeserializeDirect<T>(stream, out copy);
			return copy;
		}

		public override string ToString()
		{
			return "NetSerializer";
		}
	}
}