﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManualSerialization.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SerializationBenchmark
{
	internal class ManualSerialization : ADirectSerializer<byte[]>
	{
		#region Serialization

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = new byte[128];
			var bitSize = ((ISerializationTarget) original).Serialize(ref bytes);
			messageSize = bitSize / 8;
			return bytes;
		}

		protected override byte[] Serialize(Type type, object original, out long messageSize)
		{
			var bytes = new byte[128];
			var bitSize = ((ISerializationTarget) original).Serialize(ref bytes);
			messageSize = bitSize / 8;
			return bytes;
		}

		#endregion

		#region Deserialization

		protected override ISerializationTarget Deserialize<T>(byte[] bytes)
		{
			var instance = Activator.CreateInstance<T>() as ISerializationTarget;
			instance.Deserialize(ref bytes);
			return instance;
		}

		protected override ISerializationTarget Deserialize(Type type, object bytesObject)
		{
			byte[] bytes = (byte[])bytesObject;
			var instance = (ISerializationTarget) Activator.CreateInstance(type);
			instance.Deserialize(ref bytes);
			return instance;
		}

		#endregion

		public override string ToString()
		{
			return "Manual Serialization";
		}
	}
}