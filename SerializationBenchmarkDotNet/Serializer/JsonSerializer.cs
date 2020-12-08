// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializerTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SerializationBenchmark
{
	internal class JsonSerializer : ASerializer<MemoryStream>
	{
		private Newtonsoft.Json.JsonSerializer jsonSerializer;

		public JsonSerializer() : base()
		{
			jsonSerializer = new Newtonsoft.Json.JsonSerializer();
		}

		protected override MemoryStream Serialize(Type type, object original, out long messageSize)
		{
			var stream = new MemoryStream();
			using (var tw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
			using (var jw = new JsonTextWriter(tw))
			{
				jsonSerializer.Serialize(jw, original, type);
			}

			messageSize = stream.Position;
			return stream;
		}

		protected override object Deserialize(Type type, MemoryStream stream)
		{
			object copy;
			stream.Position = 0;
			using (var tr = new StreamReader(stream, Encoding.UTF8, false, 1024, true))
			using (var jr = new JsonTextReader(tr))
			{
				copy = jsonSerializer.Deserialize(jr, type);
			}

			return copy;
		}

		public override string ToString()
		{
			return "JsonSerializer";
		}
	}
}