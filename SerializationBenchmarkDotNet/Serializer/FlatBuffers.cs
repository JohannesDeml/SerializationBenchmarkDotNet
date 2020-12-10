// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlatBuffer.cs">
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
	internal class FlatBuffers : ASerializer<byte[]>
	{
		private FlatBufferBuilder builder;

		public FlatBuffers()
		{
			builder = new FlatBufferBuilder(1);
			
		}

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			return Serialize(typeof(T), original, out messageSize);
		}

		protected override T Deserialize<T>(byte[] serializedObject)
		{
			return (T) Deserialize(typeof(T), serializedObject);
		}

		protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			if (type == typeof(Vector3))
			{
				var vector3 = (Vector3) original;
				FlatbufferObjects.Vector3.CreateVector3(builder, vector3.x, vector3.y, vector3.z);
				
				var buffer = builder.DataBuffer;
				messageSize = buffer.Length - buffer.Position;
				
				var byteArray = builder.SizedByteArray();
				builder.Clear();
				return byteArray;
			}

			messageSize = -1;
			return null;
		}

		protected override ISerializationTarget Deserialize(Type type, byte[] serializedObject)
		{
			var buf = new ByteBuffer(serializedObject);
			
			if (type == typeof(Vector3))
			{
				var vector3 = new FlatbufferObjects.Vector3().__assign(4, buf);
				
				return new Vector3(vector3.X, vector3.Y, vector3.Z);
			}

			return null;
		}
		
		public override string ToString()
		{
			return "FlatBuffers";
		}
	}
}