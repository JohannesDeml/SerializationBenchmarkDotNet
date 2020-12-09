﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProtobufNetTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;

namespace SerializationBenchmark
{
	internal class ProtobufNet : ASerializer<MemoryStream>
	{
		#region GenericSerialization
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
		#endregion
		
		#region Non-GenericSerialization
		protected override MemoryStream Serialize(Type type, object original, out long messageSize)
		{
			var stream = new MemoryStream();
			ProtoBuf.Serializer.Serialize(stream, original);
			messageSize = stream.Position;
			return stream;
		}

		protected override object Deserialize(Type type, MemoryStream stream)
		{
			object copy;
			stream.Position = 0;
			copy = ProtoBuf.Serializer.Deserialize(type, stream);
			return copy;
		}
		#endregion

		public override string ToString()
		{
			return "ProtobufNet";
		}
	}
}