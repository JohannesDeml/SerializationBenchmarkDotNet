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
			var result = Serialize(original, out long messageSize);
			serializationResults[typeof(T)] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		public long BenchmarkDeserialize<T>(T original)
		{
			var target = serializationResults[typeof(T)];
			var copy = Deserialize<T>(target.Result);
			deserializationResults[typeof(T)] = copy;

			return target.ByteSize;
		}

		public bool Validate<T>(T original) where T : IEquatable<T>
		{
			if (deserializationResults.TryGetValue(typeof(T), out object result))
			{
				return Validate(original, (T) result);
			}

			Console.WriteLine($"Serialized result with type {typeof(T)} not found!");
			return false;
		}
		
		public bool ValidateArray<T>(T[] array)
		{
			if (deserializationResults.TryGetValue(typeof(T[]), out var result))
			{
				return ValidateList(array, (IEnumerable<T>) result);
			}
			
			Console.WriteLine($"Serialized result with type  {typeof(T[])} not found!");
			return false;
		}

		protected abstract TSerialization Serialize<T>(T original, out long messageSize);
		protected abstract T Deserialize<T>(TSerialization serializedObject);

		protected virtual bool Validate<T>(T original, T copy) where T : IEquatable<T>
		{
			return EqualityComparer<T>.Default.Equals(original, copy);
		}
		
		protected virtual bool ValidateList<T>(IEnumerable<T> originalList, IEnumerable<T> copyList)
		{
			return originalList.SequenceEqual(copyList);
		}

		public virtual void Cleanup()
		{
			serializationResults.Clear();
			deserializationResults.Clear();
		}
	}
}