﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Lz4MessagePackCSharpTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using MessagePack;

namespace SerializationBenchmark
{
	internal class Lz4MessagePackCSharp : ASerializer<byte[]>
	{
		MessagePackSerializerOptions lz4Options;

		public Lz4MessagePackCSharp() : base()
		{
			lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
		}

		protected override byte[] Serialize(Type type, object original, out long messageSize)
		{
			var bytes = MessagePack.MessagePackSerializer.Serialize(type, original, lz4Options);
			messageSize = bytes.Length;
			return bytes;
		}

		protected override object Deserialize(Type type, byte[] bytes)
		{
			return MessagePack.MessagePackSerializer.Deserialize(type, bytes, lz4Options);
		}

		public override string ToString()
		{
			return "Lz4MessagePackCSharp";
		}
	}
}