// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializerTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DotNetSerializationBenchmark
{
	internal class JsonSerializerTarget : ASerializerTarget<MemoryStream>
	{
		private JsonSerializer jsonSerializer;

		public JsonSerializerTarget(): base()
		{
			jsonSerializer = new JsonSerializer();
		}

		public override void Cleanup()
		{
		}

		protected override MemoryStream Serialize<T>(T original, out long messageSize)
		{
			var stream = new MemoryStream();
			using (var tw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
			using (var jw = new JsonTextWriter(tw))
			{
				jsonSerializer.Serialize(jw, original);
			}

			messageSize = stream.Position;
			return stream;
		}

		protected override T Deserialize<T>(MemoryStream stream)
		{
			T copy = default(T);
			stream.Position = 0;
			using (var tr = new StreamReader(stream, Encoding.UTF8, false, 1024, true))
			using (var jr = new JsonTextReader(tr))
			{
				copy = jsonSerializer.Deserialize<T>(jr);
			}
			return copy;
		}
	}
}