// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationBenchmark.cs">
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
	public abstract class ASerializationBenchmark<T> : ISerializableBenchmark
	{
		[ParamsSource(nameof(Serializers))]
		public ISerializer Serializer { get; set; }

		[ParamsSource(nameof(Targets))]
		public T Target { get; set; }

		public object TargetObject
		{
			get
			{
				return Target;
			}
			set
			{
				Target = (T)value;
			}
		}

		//[ParamsAllValues]
		public bool Generic { get; set; } = true;

		public abstract IEnumerable<ISerializer> Serializers { get; }

		public abstract IEnumerable<T> Targets { get; }

		[GlobalSetup(Target = nameof(Serialize))]
		public void PrepareBenchmark()
		{
			if (Target as ISerializationTarget != null)
			{
				// Needed for benchmarking the Protobuf library
				((ISerializationTarget)Target).GenerateProtobufMessage();
			}
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
			if (Generic)
			{
				return Serializer.BenchmarkSerialize(Target);
			}
			
			return Serializer.BenchmarkSerialize(Target.GetType(), Target);
		}

		public long GetSerializedSize()
		{
			PrepareBenchmark();
			return Serialize();
		}

		[Benchmark]
		public long Deserialize()
		{
			if (Generic)
			{
				return Serializer.BenchmarkDeserialize(Target);
			}
			
			return Serializer.BenchmarkDeserialize(Target.GetType(), Target);
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

		private void Validate(Type type, T original)
		{
			bool valid = Serializer.Validate(type, original);
			if (!valid)
			{
				throw new InvalidSerializationException($"Validation error for {original.GetType()} with serializer {Serializer.GetType()}");
			}
		}
	}
}