// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector3.cs">
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
	[Serializable]
	[ProtoContract]
	[DataContract]
	[MessagePackObject]
	public struct Vector3 : IEquatable<Vector3>, ISerializationTarget
	{
		[MessagePackMember(0)]
		[Key(0)]
		[ProtoMember(1)]
		[DataMember]
		public float x;

		[MessagePackMember(1)]
		[Key(1)]
		[ProtoMember(2)]
		[DataMember]
		public float y;

		[Key(2)]
		[MessagePackMember(3)]
		[ProtoMember(3)]
		[DataMember]
		public float z;

		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public bool Equals(Vector3 other)
		{
			return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
		}

		public bool Equals(ISerializationTarget obj)
		{
			return obj is Vector3 other && Equals(other);
		}

		public override string ToString()
		{
			return "Vector3";
		}
	}
}