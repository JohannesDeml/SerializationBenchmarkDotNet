// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ATestTarget.cs">
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

namespace SerializationBenchmark
{
	public class SerializationResult<TSerialization>
	{
		public TSerialization Result;
		public long ByteSize;

		public SerializationResult(TSerialization result, long byteSize)
		{
			Result = result;
			ByteSize = byteSize;
		}
	}

	public abstract class ASerializer<TSerialization>: ISerializer
	{
		private Dictionary<Type, SerializationResult<TSerialization>> serializationResults;
		private Dictionary<Type, object> deserializationResults;
		protected ASerializer()
		{
			serializationResults = new Dictionary<Type, SerializationResult<TSerialization>>();
			deserializationResults = new Dictionary<Type, object>();
		}

		public long BenchmarkSerialize<T>(T original)
		{
			return BenchmarkSerialize(typeof(T), original);
		}

		public long BenchmarkSerialize(Type type, object original)
		{
			var result = Serialize(type, original, out long messageSize);
			serializationResults[type] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		public long BenchmarkDeserialize<T>(T original)
		{
			return BenchmarkDeserialize(typeof(T), original);
		}

		public long BenchmarkDeserialize(Type type, object original)
		{
			var target = serializationResults[type];
			var copy = Deserialize(type, target.Result);
			deserializationResults[type] = copy;

			return target.ByteSize;
		}

		public bool Validate<T>(T original) where T : IEquatable<T>
		{
			return Validate(typeof(T), original);
		}
		
		public bool Validate(Type type, object original)
		{
			if (deserializationResults.TryGetValue(type, out object result))
			{
				return original.Equals(result);
			}

			Console.WriteLine($"Serialized result with type {type} not found!");
			return false;
		}

		protected abstract TSerialization Serialize(Type type, object original, out long messageSize);
		protected abstract object Deserialize(Type type, TSerialization serializedObject);

		public virtual void Cleanup()
		{
			serializationResults.Clear();
			deserializationResults.Clear();
		}
	}
}