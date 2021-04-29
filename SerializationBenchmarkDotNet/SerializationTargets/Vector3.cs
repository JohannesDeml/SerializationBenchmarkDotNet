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
using emotitron.Compression;
using Google.Protobuf;
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
		public float X;

		[MessagePackMember(1)]
		[Key(1)]
		[ProtoMember(2)]
		[DataMember]
		public float Y;

		[Key(2)]
		[MessagePackMember(3)]
		[ProtoMember(3)]
		[DataMember]
		public float Z;

		[NonSerialized]
		private IMessage<ProtobufObjects.Vector3> protobufObject;
		
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.protobufObject = null;
		}

		/// <inheritdoc />
		public void GenerateProtobufMessage()
		{
			protobufObject = new ProtobufObjects.Vector3()
			{
				X = X,
				Y = Y,
				Z = Z
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
			target.WriteFloat(X, ref pos);
			target.WriteFloat(Y, ref pos);
			target.WriteFloat(Z, ref pos);
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
			X = target.ReadFloat(ref pos);
			Y = target.ReadFloat(ref pos);
			Z = target.ReadFloat(ref pos);
			return pos;
		}

		/// <inheritdoc />
		public string ToReadableString()
		{
			return $"Vector3: x:{X}, y: {Y}, z:{Z}";
		}

		public override string ToString()
		{
			return "Vector3";
		}
		
		public bool Equals(Vector3 other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
		}

		public bool Equals(ISerializationTarget other)
		{
			return other is Vector3 otherVector3 && Equals(otherVector3);
		}
	}
}