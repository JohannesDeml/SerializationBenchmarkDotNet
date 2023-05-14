// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonSerializationBenchmark.cs">
//   Copyright (c) 2023 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace SerializationBenchmark;

public class PersonSerializationBenchmark : ASerializationBenchmark<Person>
{
	public override IEnumerable<ISerializer> Serializers => new ISerializer[]
	{
		new Overhead(),
		new ManualSerialization(),
		new FlatBuffers(),
		new Lz4MessagePackCSharp(),
		new MessagePackCSharp(),
		new MsgPack(),
		//new NetSerializer(),
		new NewtonsoftJson(),
		new ProtobufNet(),
		new Protobuf(),
		new Utf8JsonSerializer(),
	};
	
	public override IEnumerable<Person> Targets => new Person[]
	{
		new Person()
		{
			Age = 42,
			FirstName = "AAAAAAA",
			LastName = "BBBBBBB",
			Sex = Sex.Undefined
		}
	};
}