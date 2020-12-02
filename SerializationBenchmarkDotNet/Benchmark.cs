// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmark.cs">
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
using BenchmarkDotNet.Attributes;

namespace DotNetSerializationBenchmark
{
	[Config(typeof(BenchmarkConfig))]
	public class Benchmark
	{
		[ParamsSource(nameof(Serializers))]
		public ISerializerTarget Serializer;

		public IEnumerable<ISerializerTarget> Serializers => new ISerializerTarget[]
		{
			new MessagePackCSharpTarget(),
			new Lz4MessagePackCSharpTarget(),
			new NetSerializerTarget(),
			new MsgPackTarget(),
			new JsonSerializerTarget(),
			new ProtobufNetTarget(),
		};

		Person[] personArray;
		Vector3[] vector3Array;

		[GlobalSetup]
		public void PrepareBenchmark()
		{
			personArray = Enumerable.Range(1, 1000).Select(x => new Person {Age = x % 128, FirstName = "FirstName", LastName = "LastName", Sex = Sex.Female})
				.ToArray();
			vector3Array = Enumerable.Range(1, 1000).Select(value => new Vector3 {x = 12345.12345f + value, y = 3994.35226f - value, z = 325125.52426f})
				.ToArray();
		}
		
		[GlobalSetup(Target = nameof(Deserialize))]
		public void PrepareDeserializeBenchmark()
		{
			PrepareBenchmark();
			// Call serialize once for the current Serializer, so there is something to deserialize
			Serialize();
		}

		[Benchmark]
		public long Serialize()
		{
			var size = 0L;
			size += Serializer.BenchmarkSerialize(personArray);
			size += Serializer.BenchmarkSerialize(vector3Array);

			return size;
		}
		
		[Benchmark]
		public long Deserialize()
		{
			var size = 0L;
			size += Serializer.BenchmarkDeserialize(personArray);
			size += Serializer.BenchmarkDeserialize(vector3Array);

			return size;
		}

		[IterationCleanup(Target = nameof(Deserialize))]
		public void ValidateAndCleanupIteration()
		{
			if (!Serializer.ValidateList<Person[], Person>(personArray))
			{
				Console.WriteLine($"Validation error for {nameof(personArray)} for target {Serializer.GetType()}");
			}

			if (!Serializer.ValidateList<Vector3[], Vector3>(vector3Array))
			{
				Console.WriteLine($"Validation error for {nameof(vector3Array)} for target {Serializer.GetType()}");
			}
		}

		[GlobalCleanup]
		public void GlobalCleanup()
		{
			Serializer.Cleanup();
		}
	}
}