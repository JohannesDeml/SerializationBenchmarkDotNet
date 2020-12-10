﻿// --------------------------------------------------------------------------------------------------------------------
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
	[Config(typeof(BenchmarkConfig))]
	public class Benchmark : ISerializableBenchmark
	{
		[ParamsSource(nameof(Serializers))]
		public ISerializer Serializer { get; set; }

		[ParamsSource(nameof(Targets))]
		public ISerializationTarget Target { get; set; }

		//[ParamsAllValues]
		public bool Generic { get; set; } = true;
		
		public IEnumerable<ISerializer> Serializers => new ISerializer[]
		{
			new FlatBuffers(),
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
			if (Generic)
			{
				size += Target.Serialize(Serializer);
			}
			else
			{
				size += Serializer.BenchmarkSerialize(Target.GetType(), Target);
			}

			return size;
		}
		
		[Benchmark]
		public long Deserialize()
		{
			var size = 0L;
			if (Generic)
			{
				size += Target.Deserialize(Serializer);
			}
			else
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
		
		private void Validate(Type type, ISerializationTarget original)
		{
			bool valid = Serializer.Validate(type, original);
			if (!valid)
			{
				throw new Exception($"Validation error for {original.GetType()} with serializer {Serializer.GetType()}");
			}
		}
	}
}