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
	public class Person : IEquatable<Person>
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

		public bool Equals(Person other)
		{
			return other != null && Age == other.Age && FirstName == other.FirstName && LastName == other.LastName && Sex == other.Sex;
		}
	}
}