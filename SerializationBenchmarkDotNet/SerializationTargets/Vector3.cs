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

namespace DotNetSerializationBenchmark
{
	[Serializable]
	[ProtoContract]
	[DataContract]
	[MessagePackObject]
	public struct Vector3
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
	}
}