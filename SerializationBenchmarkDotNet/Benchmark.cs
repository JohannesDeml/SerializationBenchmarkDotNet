// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmark.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace DotNetSerializationBenchmark
{
	[GcServer(true)]
	[GcConcurrent(false)]
	public class Benchmark
	{
		[ParamsSource(nameof(Serializers))]
		public ATestTarget CurrentTarget;

		public IEnumerable<ATestTarget> Serializers => new ATestTarget[]
		{
			new MessagePackCSharpTarget(),
			new Lz4MessagePackCSharpTarget(),
			new NetSerializerTarget(new[] {typeof(Array), typeof(Person[]), typeof(Vector3[]), typeof(string)}),
			new MsgPackTarget(),
			new JsonSerializerTarget(),
			new ProtobufNetTarget(),
		};

		Person[] personArray;
		Vector3[] vector3Array;
		string largeString;

		[GlobalSetup]
		public void PrepareBenchmark()
		{
			personArray = Enumerable.Range(1000, 1000).Select(x => new Person {Age = x % 128, FirstName = "FirstName", LastName = "LastName", Sex = Sex.Female})
				.ToArray();
			vector3Array = Enumerable.Range(1, 100).Select(_ => new Vector3 {x = 12345.12345f, y = 3994.35226f, z = 325125.52426f}).ToArray();
			largeString = File.ReadAllText("CSharpHtml.txt");
		}

		[IterationCleanup]
		public void ValidateAndCleanupIteration()
		{
			if (!CurrentTarget.ValidateList<Person[], Person>(personArray))
			{
				Console.WriteLine($"Validation error for {nameof(personArray)} for target {CurrentTarget.GetType()}");
			}

			if (!CurrentTarget.ValidateList<Vector3[], Vector3>(vector3Array))
			{
				Console.WriteLine($"Validation error for {nameof(vector3Array)} for target {CurrentTarget.GetType()}");
			}
		}

		[Benchmark]
		public long RunBenchmark()
		{
			var size = 0L;
			size += CurrentTarget.Run(personArray);
			size += CurrentTarget.Run(vector3Array);

			return size;
		}
	}
}