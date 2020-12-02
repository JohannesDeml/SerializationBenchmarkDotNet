// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using BenchmarkDotNet.Running;

namespace SerializationBenchmark
{
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