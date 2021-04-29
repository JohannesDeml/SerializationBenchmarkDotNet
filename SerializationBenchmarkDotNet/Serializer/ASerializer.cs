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
	/// <summary>
	/// Stores a single serialization result (e.g. a byte array or memory stream) along with its size
	/// </summary>
	/// <typeparam name="TSerialization"></typeparam>
	public class SerializationResult<TSerialization>
	{
		public readonly TSerialization Result;
		public readonly long ByteSize;

		public SerializationResult(TSerialization result, long byteSize)
		{
			Result = result;
			ByteSize = byteSize;
		}
	}

	/// <summary>
	/// Base class for a serializer. Inherit from this class if the target serializer does not use the same class for deserialization
	/// Otherwise inherit from <see cref="ADirectSerializer{TSerialization}"/>
	/// </summary>
	/// <typeparam name="TSerialization">Type the data is serialized to</typeparam>
	/// <typeparam name="TDeserialization">Type of the deserialization result</typeparam>
	public abstract class ASerializer<TSerialization, TDeserialization> : ISerializer
	{
		private readonly Dictionary<Type, SerializationResult<TSerialization>> serializationResults;
		protected readonly Dictionary<Type, TDeserialization> DeserializationResults;

		protected ASerializer()
		{
			serializationResults = new Dictionary<Type, SerializationResult<TSerialization>>();
			DeserializationResults = new Dictionary<Type, TDeserialization>();
		}

		/// <inheritdoc />
		public long BenchmarkSerialize<T>(T original) where T : ISerializationTarget
		{
			var result = Serialize(original, out long messageSize);
			serializationResults[typeof(T)] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		/// <inheritdoc />
		public long BenchmarkSerialize(Type type, ISerializationTarget original)
		{
			var result = Serialize(type, original, out long messageSize);
			serializationResults[type] = new SerializationResult<TSerialization>(result, messageSize);
			return messageSize;
		}

		/// <inheritdoc />
		public long BenchmarkDeserialize<T>(T original) where T : ISerializationTarget
		{
			var type = typeof(T);
			var target = serializationResults[type];
			var copy = Deserialize<T>(target.Result);
			DeserializationResults[type] = copy;

			return target.ByteSize;
		}

		/// <inheritdoc />
		public long BenchmarkDeserialize(Type type, ISerializationTarget original)
		{
			var target = serializationResults[type];
			var copy = Deserialize(type, target.Result);
			DeserializationResults[type] = copy;

			return target.ByteSize;
		}

		/// <inheritdoc />
		public bool Validate(Type type, ISerializationTarget original)
		{
			if (GetResult(type, out ISerializationTarget result))
			{
				var isValid = EqualityComparer<ISerializationTarget>.Default.Equals(original, result);
				return isValid;
			}

			Console.WriteLine($"Serialized result with type {type} not found!");
			return false;
		}

		protected abstract bool GetResult(Type type, out ISerializationTarget result);

		#region Serialization

		protected abstract TSerialization Serialize<T>(T original, out long messageSize) where T : ISerializationTarget;
		protected abstract TSerialization Serialize(Type type, ISerializationTarget original, out long messageSize);
		
		#endregion

		#region Deserialization

		
		protected abstract TDeserialization Deserialize<T>(TSerialization serializedObject) where T : ISerializationTarget;
		protected abstract TDeserialization Deserialize(Type type, TSerialization serializedObject);

		#endregion

		/// <inheritdoc />
		public virtual void Cleanup()
		{
			serializationResults.Clear();
			DeserializationResults.Clear();
		}
	}
}