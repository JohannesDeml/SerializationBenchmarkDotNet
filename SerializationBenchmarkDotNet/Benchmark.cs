// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmark.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

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
		Person p;
		IList<Person> l;
		int integer;
		Vector3 v3;
		IList<Vector3> v3List;
		string largeString;
		
		[GlobalSetup]
		public void PrepareBenchmark()
		{
			p = new Person
			{
				Age = 99999,
				FirstName = "Windows",
				LastName = "Server",
				Sex = Sex.Male,
			};
			l = Enumerable.Range(1000, 1000).Select(x => new Person { Age = x, FirstName = "Windows", LastName = "Server", Sex = Sex.Female }).ToArray();

			integer = 1;
			v3 = new Vector3 { x = 12345.12345f, y = 3994.35226f, z = 325125.52426f };
			v3List = Enumerable.Range(1, 100).Select(_ => new Vector3 { x = 12345.12345f, y = 3994.35226f, z = 325125.52426f }).ToArray();
			largeString = File.ReadAllText("CSharpHtml.txt");
		}

		[IterationCleanup]
		public async void CleanupIteration()
		{
		}

		[Benchmark]
		public int GenerateCommandLists()
		{
			return 1;
		}
	}
}