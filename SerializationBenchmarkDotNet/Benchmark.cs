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
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace SerializationBenchmark
{
	[Config(typeof(BenchmarkConfig))]
	public class Benchmark
	{
		[ParamsSource(nameof(Serializers))]
		public ISerializer Serializer;

		public IEnumerable<ISerializer> Serializers => new ISerializer[]
		{
			new MessagePackCSharp(),
			new Lz4MessagePackCSharp(),
			new NetSerializer(),
			new MsgPack(),
			new JsonSerializer(),
			new ProtobufNet(),
		};

		private Person person;
		private Vector3 vector3;
		private Person[] personArray;
		private Vector3[] vector3Array;

		[GlobalSetup]
		public void PrepareBenchmark()
		{
			person = new Person {Age = 28, FirstName = "FirstName", LastName = "LastName", Sex = Sex.Female};
			vector3 = new Vector3(12.345f, 987.654f, 1.3f);
			personArray = Enumerable.Range(1, 1000).Select(x => new Person {Age = x % 128, FirstName = "FirstName", LastName = "LastName", Sex = Sex.Female})
				.ToArray();
			vector3Array = Enumerable.Range(1, 1000).Select(value => new Vector3 {x = 12345.12345f + value * 0.573f, y = 3994.35226f - value * 0.249f, z = 325125.52426f / (value * 2.571f)})
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
			size += Serializer.BenchmarkSerialize(person);
			size += Serializer.BenchmarkSerialize(vector3);

			return size;
		}
		
		[Benchmark]
		public long Deserialize()
		{
			var size = 0L;
			size += Serializer.BenchmarkDeserialize(person);
			size += Serializer.BenchmarkDeserialize(vector3);

			return size;
		}

		[IterationCleanup(Target = nameof(Deserialize))]
		public void ValidateDeserialization()
		{
			Validate(person);
			Validate(vector3);
		}

		[GlobalCleanup]
		public void GlobalCleanup()
		{
			Serializer.Cleanup();
		}
		
		private bool Validate<T>(T original) where T : IEquatable<T>
		{
			bool valid = Serializer.Validate(original);
			if (!valid)
			{
				Console.WriteLine($"Validation error for {original.GetType()} with serializer {Serializer.GetType()}");
			}
			return valid;
		}

		private bool ValidateArray<T>(T[] originalArray)
		{
			bool valid = Serializer.ValidateArray(originalArray);
			if (!valid)
			{
				Console.WriteLine($"Validation error for {originalArray.GetType()} with serializer {Serializer.GetType()}");
			}
			return valid;
		}
	}
}