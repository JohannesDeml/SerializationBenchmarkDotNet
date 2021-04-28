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
	public class Person : IEquatable<Person>, ISerializationTarget
	{
		[MessagePackMember(0)]
		[Key(0)]
		[ProtoMember(1)]
		[DataMember]
		public virtual int Age { get; set; }

		[MessagePackMember(1)]
		[Key(1)]
		[ProtoMember(2)]
		[DataMember]
		public virtual string FirstName { get; set; }

		[MessagePackMember(2)]
		[Key(2)]
		[ProtoMember(3)]
		[DataMember]
		public virtual string LastName { get; set; }

		[MessagePackMember(3)]
		[Key(3)]
		[ProtoMember(4)]
		[DataMember]
		public virtual Sex Sex { get; set; }

		[NonSerialized]
		private IMessage<ProtobufObjects.Person> protobufObject;
		
		public bool Equals(Person other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Age == other.Age && FirstName == other.FirstName && LastName == other.LastName && Sex == other.Sex;
		}

		public bool Equals(ISerializationTarget obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Person) obj);
		}

		public override string ToString()
		{
			return "Person";
		}

		public string ToReadableString()
		{
			return $"Person Age: {Age}, FirstName: {FirstName}, LastName: {LastName}, Sex: {Sex}";
		}

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

		public IMessage GetProtobufMessage()
		{
			return protobufObject;
		}

		public long Serialize(ISerializer serializer)
		{
			return serializer.BenchmarkSerialize(this);
		}

		public long Deserialize(ISerializer serializer)
		{
			return serializer.BenchmarkDeserialize(this);
		}

		public long Serialize(ref byte[] target)
		{
			var pos = 0;
			target.Write((ByteConverter) Age, ref pos, 8);
			WriteString(target, FirstName, 20, ref pos);
			WriteString(target, LastName, 20, ref pos);
			target.Write((ByteConverter) (sbyte) Sex, ref pos, 8);
			return pos;
		}

		private void WriteString(byte[] target, string value, int maxLength, ref int pos)
		{
			var bytePos = pos / 8;
			var stringBytes = System.Text.Encoding.ASCII.GetBytes(value, 0, Math.Min(maxLength, value.Length));
			stringBytes.CopyTo(target, bytePos);
			pos += maxLength * 8;
		}

		public long Deserialize(ref byte[] target)
		{
			var pos = 0;
			Age = (int) target.Read(ref pos, 8);
			FirstName = ReadString(target, 20, ref pos);
			LastName = ReadString(target, 20, ref pos);
			Sex = (Sex) target.Read(ref pos, 8);
			return pos;
		}

		private string ReadString(byte[] target, int maxLength, ref int pos)
		{
			var bytePos = pos / 8;
			var stringValue = System.Text.Encoding.ASCII.GetString(target, bytePos, maxLength);
			pos += maxLength * 8;
			var sanitizedString = stringValue.Trim((char) 0x00);
			return sanitizedString;
		}
	}
}