// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Protobuf.cs">
//   Copyright (c) 2021 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Google.Protobuf;

namespace SerializationBenchmark
{
	internal class Protobuf : ASerializer<byte[], IMessage>
	{
		#region GenericSerialization

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var message = original.GetProtobufMessage();
			messageSize = message.CalculateSize();
			var bytes = new byte[messageSize];
			message.WriteTo(bytes);
			return bytes;
		}

		protected override IMessage Deserialize<T>(byte[] bytes)
		{
			return Deserialize(typeof(T), bytes);
		}

		#endregion

		#region Non-GenericSerialization

		protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			var message = original.GetProtobufMessage();
			messageSize = message.CalculateSize();
			var bytes = new byte[messageSize];
			message.WriteTo(bytes);
			return bytes;
		}

		protected override IMessage Deserialize(Type type, byte[] bytes)
		{
			if (type == typeof(Person))
			{
				var person = ProtobufObjects.Person.Parser.ParseFrom(bytes);
				return person;
			}
			
			if (type == typeof(Vector3))
			{
				var vector3 = ProtobufObjects.Vector3.Parser.ParseFrom(bytes);
				return vector3;
			}
			
			throw new NotImplementedException($"Deserialization for type {type} not implemented!");
		}

		#endregion

		protected override bool GetResult(Type type, out ISerializationTarget result)
		{
			var intermediateResult = deserializationResults[type];

			if (type == typeof(Vector3))
			{
				var vector3 = (ProtobufObjects.Vector3) intermediateResult;
				result = new Vector3(vector3.X, vector3.Y, vector3.Z);
				return true;
			}

			if (type == typeof(Person))
			{
				var person = (ProtobufObjects.Person) intermediateResult;
				result = new Person()
				{
					Age = person.Age,
					FirstName = person.FirstName,
					LastName = person.LastName,
					Sex = (Sex) person.Sex
				};
				return true;
			}

			throw new NotImplementedException($"Conversion for type {type} not implemented!");
		}
		
		public override string ToString()
		{
			return "Protobuf";
		}
	}
}