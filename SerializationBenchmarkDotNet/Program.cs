using System;
using BenchmarkDotNet.Running;

namespace DotNetSerializationBenchmark
{
	using MessagePack;
	using MsgPack.Serialization;
	using Newtonsoft.Json;
	using ProtoBuf;
	using System;
	using System.Diagnostics;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	
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
			return Age == other.Age && FirstName == other.FirstName && LastName == other.LastName && Sex == other.Sex;
		}
	}

	public enum Sex : sbyte
	{
		Unknown, Male, Female,
	}
	
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
	
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start running Serialization Benchmark");
			BenchmarkRunner.Run<Benchmark>();
			Console.WriteLine("Finished running Serialization Benchmark");
		}
	}
}