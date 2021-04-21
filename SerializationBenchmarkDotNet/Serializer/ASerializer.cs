// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ASerializer.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

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
	
	public abstract class ASerializer<TSerialization, TDeserialization>: ISerializer
	{
		private Dictionary<Type, SerializationResult<TSerialization>> serializationResults;
		protected Dictionary<Type, TDeserialization> deserializationResults;
		protected ASerializer()
		{
			serializationResults = new Dictionary<Type, SerializationResult<TSerialization>>();
			deserializationResults = new Dictionary<Type, TDeserialization>();
		}

		public long BenchmarkSerialize<T>(T original) where T: ISerializationTarget
		{
			var result = Serialize(original, out long messageSize);
			serializationResults[typeof(T)] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		public long BenchmarkSerialize(Type type, ISerializationTarget original)
		{
			var result = Serialize(type, original, out long messageSize);
			serializationResults[type] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		public long BenchmarkDeserialize<T>(T original) where T: ISerializationTarget
		{
			var type = typeof(T);
			var target = serializationResults[type];
			var copy = Deserialize<T>(target.Result);
			deserializationResults[type] = copy;

			return target.ByteSize;
		}

		public long BenchmarkDeserialize(Type type, ISerializationTarget original)
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
			if (GetResult(type, out ISerializationTarget result))
			{
				return EqualityComparer<ISerializationTarget>.Default.Equals(original, result);
			}

			Console.WriteLine($"Serialized result with type {type} not found!");
			return false;
		}

		protected abstract bool GetResult(Type type, out ISerializationTarget result);

		#region GenericSerialization
		protected abstract TSerialization Serialize<T>(T original, out long messageSize) where T: ISerializationTarget;
		protected abstract TDeserialization Deserialize<T>(TSerialization serializedObject) where T: ISerializationTarget;
		#endregion
		
		#region Non-GenericSerialization
		protected abstract TSerialization Serialize(Type type, ISerializationTarget original, out long messageSize);
		protected abstract TDeserialization Deserialize(Type type, TSerialization serializedObject);
		#endregion

		public virtual void Cleanup()
		{
			serializationResults.Clear();
			deserializationResults.Clear();
		}
	}
}