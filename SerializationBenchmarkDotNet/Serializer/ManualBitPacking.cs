﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManualBitPacking.cs">
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
	internal class ManualBitPacking : ADirectSerializer<byte[]>
	{
		#region GenericSerialization

		protected override byte[] Serialize<T>(T original, out long messageSize)
		{
			var bytes = new byte[128];
			messageSize = original.Serialize(ref bytes) / 8;
			return bytes;
		}

		protected override ISerializationTarget Deserialize<T>(byte[] bytes)
		{
			var instance = Activator.CreateInstance<T>();
			instance.Deserialize(ref bytes);
			return instance;
		}

		#endregion

		#region Non-GenericSerialization

		protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
		{
			var bytes = new byte[128];
			messageSize = original.Serialize(ref bytes) / 8;
			return bytes;
		}

		protected override ISerializationTarget Deserialize(Type type, byte[] bytes)
		{
			var instance = (ISerializationTarget) Activator.CreateInstance(type);
			instance.Deserialize(ref bytes);
			return instance;
		}

		#endregion

		public override string ToString()
		{
			return "ManualBitPacking";
		}
	}
}