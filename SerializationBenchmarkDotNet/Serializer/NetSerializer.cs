// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetSerializer.cs">
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
	internal class NetSerializer : ADirectSerializer<MemoryStream>
	{
		private readonly global::NetSerializer.Serializer netSerializer;

		public NetSerializer() : base()
		{
			var rootTypes = GetSubclasses(typeof(ISerializationTarget));
			netSerializer = new global::NetSerializer.Serializer(rootTypes);
		}

		static IEnumerable<Type> GetSubclasses(Type type)
		{
			return type.Assembly.GetTypes().Where(t => type.IsAssignableFrom(t));
		}

		#region Serialization

		protected override MemoryStream Serialize<T>(T original, out long messageSize)
		{
			var stream = new MemoryStream();
			netSerializer.SerializeDirect<T>(stream, original);
			messageSize = stream.Position;
			return stream;
		}

		protected override MemoryStream Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			var stream = new MemoryStream();
			netSerializer.SerializeDirect(stream, (object) original);
			messageSize = stream.Position;
			return stream;
		}

		#endregion

		#region Deserialization

		protected override ISerializationTarget Deserialize<T>(MemoryStream stream)
		{
			stream.Position = 0;
			netSerializer.DeserializeDirect<T>(stream, out var copy);
			return copy;
		}

		protected override ISerializationTarget Deserialize(Type type, MemoryStream stream)
		{
			stream.Position = 0;
			netSerializer.DeserializeDirect(stream, out object copy);
			return (ISerializationTarget) copy;
		}

		#endregion

		public override string ToString()
		{
			return "NetSerializer";
		}
	}
}