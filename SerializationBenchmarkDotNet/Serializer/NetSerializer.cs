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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SerializationBenchmark
{
	internal class NetSerializer : ASerializer<MemoryStream>
	{
		private global::NetSerializer.Serializer netSerializer;

		public NetSerializer() : base()
		{
			var rootTypes = GetSubclasses(typeof(ISerializationTarget));
			netSerializer = new global::NetSerializer.Serializer(rootTypes);
		}
		
		IEnumerable<Type> GetSubclasses(Type type)
		{
			return type.Assembly.GetTypes().Where(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
		}

		protected override MemoryStream Serialize(Type type, object original, out long messageSize)
		{
			var stream = new MemoryStream();
			netSerializer.SerializeDirect(stream, original);
			messageSize = stream.Position;
			return stream;
		}

		protected override object Deserialize(Type type, MemoryStream stream)
		{
			object copy;
			stream.Position = 0;
			netSerializer.DeserializeDirect(stream, out copy);
			return copy;
		}

		public override string ToString()
		{
			return "NetSerializer";
		}
	}
}