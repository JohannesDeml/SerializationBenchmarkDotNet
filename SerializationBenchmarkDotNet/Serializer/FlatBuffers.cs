// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlatBuffers.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using FlatBuffers;

namespace SerializationBenchmark
{
	internal class FlatBuffers : ASerializer<byte[], IFlatbufferObject>
	{
		private readonly FlatBufferBuilder builder;

		public FlatBuffers()
		{
			builder = new FlatBufferBuilder(1);
		}

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			return Serialize(typeof(T), original, out messageSize);
		}

		protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			if (type == typeof(Vector3))
			{
				var vector3 = (Vector3) original;
				var flatBufferVector3 = FlatbufferObjects.Vector3.CreateVector3(builder, vector3.X, vector3.Y, vector3.Z);
				builder.Finish(flatBufferVector3.Value);

				messageSize = GetSize();

				var byteArray = builder.SizedByteArray();
				builder.Clear();
				return byteArray;
			}

			if (type == typeof(Person))
			{
				var person = (Person) original;
				var firstNameOffset = builder.CreateString(person.FirstName);
				var lastNameOffset = builder.CreateString(person.LastName);

				FlatbufferObjects.Person.StartPerson(builder);
				FlatbufferObjects.Person.AddAge(builder, person.Age);
				FlatbufferObjects.Person.AddFirstName(builder, firstNameOffset);
				FlatbufferObjects.Person.AddLastName(builder, lastNameOffset);
				FlatbufferObjects.Person.AddSex(builder, (FlatbufferObjects.Sex) person.Sex);
				var flatBufferPerson = FlatbufferObjects.Person.EndPerson(builder);
				builder.Finish(flatBufferPerson.Value);

				messageSize = GetSize();

				var byteArray = builder.SizedByteArray();
				builder.Clear();
				return byteArray;
			}

			throw new NotImplementedException($"Serialization for type {type} not implemented!");
		}

		private int GetSize()
		{
			// Suggested calculation: buf.Length - buf.Position results in the buffer array size, not the actual size
			// I think offset can be used to get the correct value, since we're clearing the builder every time
			return builder.Offset;
		}
		
		protected override IFlatbufferObject Deserialize<T>(byte[] serializedObject)
		{
			return Deserialize(typeof(T), serializedObject);
		}

		protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
		{
			var buf = new ByteBuffer(serializedObject);

			if (type == typeof(Vector3))
			{
				var offset = buf.GetInt(buf.Position) + buf.Position;
				var vector3 = new FlatbufferObjects.Vector3().__assign(offset, buf);
				return vector3;
			}

			if (type == typeof(Person))
			{
				var person = FlatbufferObjects.Person.GetRootAsPerson(buf);
				return person;
			}

			throw new NotImplementedException($"Deserialization for type {type} not implemented!");
		}

		protected override bool GetResult(Type type, out ISerializationTarget result)
		{
			var intermediateResult = DeserializationResults[type];

			if (type == typeof(Vector3))
			{
				var vector3 = (FlatbufferObjects.Vector3) intermediateResult;
				result = new Vector3(vector3.X, vector3.Y, vector3.Z);
				return true;
			}

			if (type == typeof(Person))
			{
				var person = (FlatbufferObjects.Person) intermediateResult;
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
			return "FlatBuffers";
		}
	}
}