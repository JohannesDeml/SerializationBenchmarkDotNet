﻿// --------------------------------------------------------------------------------------------------------------------
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
			var result = Serialize(original, out long messageSize);
			serializationResults[typeof(T)] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		public long BenchmarkSerialize(Type type, object original)
		{
			var result = Serialize(type, original, out long messageSize);
			serializationResults[type] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		public long BenchmarkDeserialize<T>(T original)
		{
			var type = typeof(T);
			var target = serializationResults[type];
			var copy = Deserialize<T>(target.Result);
			deserializationResults[type] = copy;

			return target.ByteSize;
		}

		public long BenchmarkDeserialize(Type type, object original)
		{
			var target = serializationResults[type];
			var copy = Deserialize(type, target.Result);
			deserializationResults[type] = copy;

			return target.ByteSize;
		}

		public bool Validate<T>(T original) where T : ISerializationTarget
		{
			return Validate(typeof(T), original);
		}
		
		public bool Validate(Type type, ISerializationTarget original)
		{
			if (deserializationResults.TryGetValue(type, out object result))
			{
				return EqualityComparer<ISerializationTarget>.Default.Equals(original, (ISerializationTarget)result);
			}

			Console.WriteLine($"Serialized result with type {type} not found!");
			return false;
		}

		#region GenericSerialization
		protected abstract TSerialization Serialize<T>(T original, out long messageSize);
		protected abstract T Deserialize<T>(TSerialization serializedObject);
		#endregion
		
		#region Non-GenericSerialization
		protected abstract TSerialization Serialize(Type type, object original, out long messageSize);
		protected abstract object Deserialize(Type type, TSerialization serializedObject);
		#endregion

		public virtual void Cleanup()
		{
			serializationResults.Clear();
			deserializationResults.Clear();
		}
	}
}