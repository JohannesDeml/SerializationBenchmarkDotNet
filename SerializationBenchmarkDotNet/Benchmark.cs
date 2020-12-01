// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmark.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace DotNetSerializationBenchmark
{
	[GcServer(true)]
	[GcConcurrent(false)]
	public class Benchmark
	{
		private Dictionary<Type, ATestTarget> testTargets;
		Person[] personArray;
		Vector3[] vector3Array;
		string largeString;
		
		private ATestTarget currentTarget;
		
		[GlobalSetup]
		public void PrepareBenchmark()
		{
			personArray = Enumerable.Range(1000, 1000).Select(x => new Person { Age = x%128, FirstName = "FirstName", LastName = "LastName", Sex = Sex.Female }).ToArray();
			vector3Array = Enumerable.Range(1, 100).Select(_ => new Vector3 { x = 12345.12345f, y = 3994.35226f, z = 325125.52426f }).ToArray();
			largeString = File.ReadAllText("CSharpHtml.txt");
			
			var rootTypes = new[] { typeof(Array), typeof(Person[]), typeof(Vector3[]), largeString.GetType() };

			testTargets = new Dictionary<Type, ATestTarget>();
			AddTestTarget(new MessagePackCSharpTarget());
			AddTestTarget(new Lz4MessagePackCSharpTarget());
			AddTestTarget(new NetSerializerTarget(rootTypes));
			AddTestTarget(new MsgPackTarget());
			AddTestTarget(new JsonSerializerTarget());
			AddTestTarget(new ProtobufNetTarget());
		}

		private void AddTestTarget<T>(T target) where T:ATestTarget
		{
			testTargets.Add(typeof(T), target);
		}

		[IterationCleanup]
		public void ValidateAndCleanupIteration()
		{
			if (!currentTarget.ValidateList<Person[], Person>(personArray))
			{
				Console.WriteLine($"Validation error for {nameof(personArray)} for target {currentTarget.GetType()}");
			}
			
			if(!currentTarget.ValidateList<Vector3[], Vector3>(vector3Array))
			{
				Console.WriteLine($"Validation error for {nameof(vector3Array)} for target {currentTarget.GetType()}");
			}
		}

		[Benchmark]
		public long BenchmarkMessagePackCSharp()
		{
			currentTarget = testTargets[typeof(MessagePackCSharpTarget)];
			return RunBenchmark();
		}

		[Benchmark]
		public long BenchmarkLz4MessagePackCSharp()
		{
			currentTarget = testTargets[typeof(Lz4MessagePackCSharpTarget)];
			return RunBenchmark();
		}
		
		[Benchmark]
		public long BenchmarkNetSerializer()
		{
			currentTarget = testTargets[typeof(NetSerializerTarget)];
			return RunBenchmark();
		}
		
		[Benchmark]
		public long BenchmarkMsgPack()
		{
			currentTarget = testTargets[typeof(MsgPackTarget)];
			return RunBenchmark();
		}
		
		[Benchmark]
		public long BenchmarkJsonSerializer()
		{
			currentTarget = testTargets[typeof(JsonSerializerTarget)];
			return RunBenchmark();
		}
		
		[Benchmark]
		public long BenchmarkProtobufNet()
		{
			currentTarget = testTargets[typeof(ProtobufNetTarget)];
			return RunBenchmark();
		}
		
		private long RunBenchmark()
		{
			var size = 0L;
			size += currentTarget.Run(personArray);
			size += currentTarget.Run(vector3Array);

			return size;
		}
	}
}