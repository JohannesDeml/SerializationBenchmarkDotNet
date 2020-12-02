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

namespace DotNetSerializationBenchmark
{
	public class SerializationResult<T>
	{
		public T Result;
		public long ByteSize;

		public SerializationResult(T result, long byteSize)
		{
			Result = result;
			ByteSize = byteSize;
		}
	}
	
	public class DeserializationResult
	{
		public object Result;
		public long ByteSize;

		public DeserializationResult(object result, long byteSize)
		{
			Result = result;
			ByteSize = byteSize;
		}
	}

	public abstract class ASerializerTarget<TSerialization>: ISerializerTarget
	{
		private Dictionary<Type, SerializationResult<TSerialization>> serializationResults;
		private Dictionary<Type, DeserializationResult> deserializationResults;
		protected ASerializerTarget()
		{
			serializationResults = new Dictionary<Type, SerializationResult<TSerialization>>();
			deserializationResults = new Dictionary<Type, DeserializationResult>();
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
			deserializationResults[typeof(T)] = new DeserializationResult(copy, target.ByteSize);

			return target.ByteSize;
		}

		public bool Validate<T>(T original) where T : IEquatable<T>
		{
			if (deserializationResults.TryGetValue(typeof(T), out DeserializationResult result))
			{
				return Validate(original, (T) result.Result);
			}

			return false;
		}
		
		public bool ValidateList<T, U>(T originalList) where T : IList<U>
		{
			if (deserializationResults.TryGetValue(typeof(T), out DeserializationResult result))
			{
				return ValidateList<T, U>(originalList, (T) result.Result);
			}

			return false;
		}

		protected abstract TSerialization Serialize<T>(T original, out long messageSize);
		protected abstract T Deserialize<T>(TSerialization serializedObject);

		protected virtual bool Validate<T>(T original, T copy) where T : IEquatable<T>
		{
			return EqualityComparer<T>.Default.Equals(original, copy);
		}
		
		protected virtual bool ValidateList<T, U>(T originalList, T copyList) where T : IList<U>
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