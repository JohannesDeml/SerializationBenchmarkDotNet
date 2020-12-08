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
using BenchmarkDotNet.Attributes;

namespace SerializationBenchmark
{
	public class SerializationTargets<T>
	{
		public readonly Dictionary<Type, T> Targets;

		public SerializationTargets()
		{
			Targets = new Dictionary<Type, T>();
		}

		public SerializationTargets<T> Add<U>(U instance) where U : T
		{
			Targets[typeof(U)] = instance;
			return this;
		}
	}
	
	[Config(typeof(BenchmarkConfig))]
	public class Benchmark
	{
		[ParamsSource(nameof(Serializers))]
		public ISerializer Serializer;

		[ParamsSource(nameof(Targets))]
		public ISerializationTarget Target;
		
		public IEnumerable<ISerializer> Serializers => new ISerializer[]
		{
			new MessagePackCSharp(),
			new Lz4MessagePackCSharp(),
			new NetSerializer(),
			new MsgPack(),
			new JsonSerializer(),
			new ProtobufNet(),
		};

		public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
		{
			new Person {Age = 28, FirstName = "FirstName", LastName = "LastName", Sex = Sex.Female},
			new Vector3(12.345f, 987.654f, 1.3f)
		};

		[GlobalSetup]
		public void PrepareBenchmark()
		{
		}
		
		[GlobalSetup(Target = nameof(Deserialize))]
		public void PrepareDeserializeBenchmark()
		{
			PrepareBenchmark();
			// Serialize once, so we can deserialize the result
			Serialize();
		}

		[Benchmark]
		public long Serialize()
		{
			var size = 0L;
			for (int i = 0; i < 100; i++)
			{
				size += Serializer.BenchmarkSerialize(Target.GetType(), Target);
			}

			return size;
		}
		
		[Benchmark]
		public long Deserialize()
		{
			var size = 0L;

			for (int i = 0; i < 100; i++)
			{
				size += Serializer.BenchmarkDeserialize(Target.GetType(), Target);
			}

			return size;
		}

		[IterationCleanup(Target = nameof(Deserialize))]
		public void ValidateDeserialization()
		{
			Validate(Target.GetType(), Target);
		}

		[GlobalCleanup]
		public void GlobalCleanup()
		{
			Serializer.Cleanup();
		}
		
		private bool Validate(Type type, object original)
		{
			bool valid = Serializer.Validate(type, original);
			if (!valid)
			{
				Console.WriteLine($"Validation error for {original.GetType()} with serializer {Serializer.GetType()}");
			}
			return valid;
		}
	}
}