// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using emotitron.Compression;
using Google.Protobuf;
using MessagePack;
using MsgPack.Serialization;
using ProtoBuf;

namespace SerializationBenchmark
{
	public enum Sex : sbyte
	{
		Unknown,
		Male,
		Female,
	}

	[Serializable]
	[ProtoContract]
	[DataContract]
	[MessagePackObject]
	public sealed class Person : IEquatable<Person>, ISerializationTarget
	{
		[MessagePackMember(0)]
		[Key(0)]
		[ProtoMember(1)]
		[DataMember]
		public int Age { get; set; }

		[MessagePackMember(1)]
		[Key(1)]
		[ProtoMember(2)]
		[DataMember]
		public string FirstName { get; set; }

		[MessagePackMember(2)]
		[Key(2)]
		[ProtoMember(3)]
		[DataMember]
		public string LastName { get; set; }

		[MessagePackMember(3)]
		[Key(3)]
		[ProtoMember(4)]
		[DataMember]
		public Sex Sex { get; set; }

		[NonSerialized]
		private IMessage<ProtobufObjects.Person> protobufObject;

		/// <inheritdoc />
		public void GenerateProtobufMessage()
		{
			protobufObject = new ProtobufObjects.Person()
			{
				Age = Age,
				FirstName = FirstName,
				LastName = LastName,
				Sex = (ProtobufObjects.Person.Types.Sex) Sex
			};
		}

		/// <inheritdoc />
		public IMessage GetProtobufMessage()
		{
			return protobufObject;
		}

		/// <inheritdoc />
		public long Serialize(ISerializer serializer)
		{
			return serializer.BenchmarkSerialize(this);
		}

		/// <inheritdoc />
		public long Serialize(ref byte[] target)
		{
			var pos = 0;
			target.Write((ByteConverter) Age, ref pos, 8);
			StringSerialization.WriteString(target, FirstName, 20, ref pos);
			StringSerialization.WriteString(target, LastName, 20, ref pos);
			target.Write((ByteConverter) (sbyte) Sex, ref pos, 8);
			return pos;
		}

		/// <inheritdoc />
		public long Deserialize(ISerializer serializer)
		{
			return serializer.BenchmarkDeserialize(this);
		}

		/// <inheritdoc />
		public long Deserialize(ref byte[] target)
		{
			var pos = 0;
			Age = (int) target.Read(ref pos, 8);
			FirstName = StringSerialization.ReadString(target, 20, ref pos);
			LastName = StringSerialization.ReadString(target, 20, ref pos);
			Sex = (Sex) target.Read(ref pos, 8);
			return pos;
		}

		/// <inheritdoc />
		public string ToReadableString()
		{
			return $"Person Age: {Age}, FirstName: {FirstName}, LastName: {LastName}, Sex: {Sex}";
		}
		
		public override string ToString()
		{
			return "Person";
		}

		public bool Equals(Person other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Age == other.Age && FirstName == other.FirstName && LastName == other.LastName && Sex == other.Sex;
		}

		public bool Equals(ISerializationTarget other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			if (other.GetType() != this.GetType()) return false;
			return Equals((Person) other);
		}
	}
}