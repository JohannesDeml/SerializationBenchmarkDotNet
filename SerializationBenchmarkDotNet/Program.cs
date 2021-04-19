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
	public static class Program
	{
		public static void Main(string[] args)
		{
			#if DEBUG
			Console.WriteLine("Debug Serialization Benchmark - Use Release setup for actual benchmarking!");
			BenchmarkRunner.Run<Benchmark>(new DebugBenchmarkConfig());
			#else
			Console.WriteLine("Run Serialization Benchmark");
			BenchmarkRunner.Run<Benchmark>(new BenchmarkConfig());
			#endif
		}
	}
}