// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmark.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Text;

namespace DotNetSerializationBenchmark
{
	[GcServer(true)]
	[GcConcurrent(false)]
	public partial class Benchmark
	{
		Person person;
		Person[] personArray;
		int integer;
		Vector3 vector3;
		Vector3[] vector3Array;
		string largeString;

		private Person[] personOutputArray;
		private Vector3[] vector3OutputArray;

		private NetSerializer.Serializer netSerializer;
		private JsonSerializer jsonSerializer = new JsonSerializer();
		
		[GlobalSetup]
		public void PrepareBenchmark()
		{
			person = new Person
			{
				Age = 99999,
				FirstName = "Windows",
				LastName = "Server",
				Sex = Sex.Male,
			};
			personArray = Enumerable.Range(1000, 1000).Select(x => new Person { Age = x, FirstName = "Windows", LastName = "Server", Sex = Sex.Female }).ToArray();

			integer = 1;
			vector3 = new Vector3 { x = 12345.12345f, y = 3994.35226f, z = 325125.52426f };
			vector3Array = Enumerable.Range(1, 100).Select(_ => new Vector3 { x = 12345.12345f, y = 3994.35226f, z = 325125.52426f }).ToArray();
			largeString = File.ReadAllText("CSharpHtml.txt");
			
			netSerializer = new NetSerializer.Serializer(new[] { person.GetType(), typeof(Array), typeof(Person[]), integer.GetType(), vector3.GetType(), typeof(Vector3[]), largeString.GetType() });
		}

		[IterationCleanup]
		public void ValidateAndCleanupIteration()
		{
		}

		[Benchmark]
		public long BenchmarkMessagePackCSharp()
		{
			var size = 0L;
			personOutputArray = SerializeMessagePackCSharp(personArray, ref size);
			return size;
		}
		
		[Benchmark]
		public long BenchmarkLZ4MessagePackCSharp()
		{
			var size = 0L;
			SerializeLZ4MessagePackCSharp(personArray, ref size);
			return size;
		}
		
		[Benchmark]
		public long BenchmarkNetSerializer()
		{
			var size = 0L;
			SerializeNetSerializer(vector3Array, ref size);
			return size;
		}
		
		[Benchmark]
		public long BenchmarkMsgPack()
		{
			var size = 0L;
			SerializeMsgPack(personArray, ref size);
			return size;
		}
		
		[Benchmark]
		public long BenchmarkJsonSerializer()
		{
			var size = 0L;
			SerializeJsonSerializer(personArray, ref size);
			return size;
		}
		
		[Benchmark]
		public long BenchmarkProtobufNet()
		{
			var size = 0L;
			SerializeProtobufNet(personArray, ref size);
			return size;
		}
		
		#region MessagePack

		private T SerializeMessagePackCSharp<T>(T original, ref long messageSize)
		{
			T copy = default(T);
			byte[] bytes = null;

			bytes = MessagePack.MessagePackSerializer.Serialize(original);
			copy = MessagePack.MessagePackSerializer.Deserialize<T>(bytes);
			messageSize = bytes.Length;

			return copy;
		}
		
		private T SerializeLZ4MessagePackCSharp<T>(T original, ref long messageSize)
		{
			var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
			T copy = default(T);
			byte[] bytes = null;

			bytes = MessagePack.MessagePackSerializer.Serialize(original, lz4Options);
			copy = MessagePack.MessagePackSerializer.Deserialize<T>(bytes, lz4Options);
			messageSize = bytes.Length;

			return copy;
		}
		
		#endregion

		#region NetSerializer

		private T SerializeNetSerializer<T>(T original, ref long messageSize)
		{
			T copy = default(T);
			MemoryStream stream = null;

			netSerializer.SerializeDirect<T>(stream = new MemoryStream(), original);
			stream.Position = 0;
			netSerializer.DeserializeDirect<T>(stream, out copy);
			messageSize = stream.Position;

			return copy;
		}

		#endregion

		#region MsgPack

		private T SerializeMsgPack<T>(T original, ref long messageSize)
		{
			T copy = default(T);
			byte[] bytes = null;

			bytes = MsgPack.Serialization.MessagePackSerializer.Get<T>().PackSingleObject(original);
			copy = MsgPack.Serialization.MessagePackSerializer.Get<T>().UnpackSingleObject(bytes);
			messageSize = bytes.Length;

			return copy;
		}

		#endregion

		#region JsonSerializer

		private T SerializeJsonSerializer<T>(T original, ref long messageSize)
		{
			T copy = default(T);
			MemoryStream stream = null;

			stream = new MemoryStream();
			using (var tw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
			using (var jw = new JsonTextWriter(tw))
			{
				jsonSerializer.Serialize(jw, original);
			}
			
			stream.Position = 0;
			using (var tr = new StreamReader(stream, Encoding.UTF8, false, 1024, true))
			using (var jr = new JsonTextReader(tr))
			{
				copy = jsonSerializer.Deserialize<T>(jr);
			}
			messageSize = stream.Position;

			return copy;
		}

		#endregion
		
		#region Protobuf

		private T SerializeProtobufNet<T>(T original, ref long messageSize)
		{
			T copy = default(T);
			MemoryStream stream = null;

			ProtoBuf.Serializer.Serialize<T>(stream = new MemoryStream(), original);
			stream.Position = 0;
			copy = ProtoBuf.Serializer.Deserialize<T>(stream);
			messageSize = stream.Position;

			return copy;
		}

		#endregion
	}
}