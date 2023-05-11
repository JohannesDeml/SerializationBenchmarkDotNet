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
			if (IsTestRun())
			{
				Console.WriteLine("Debug Serialization Benchmark - Use Release setup for actual benchmarking!");
				BenchmarkRunner.Run<SerializationBenchmark>(new DebugBenchmarkConfig());
			}
			else
			{
				Console.WriteLine("Run Serialization Benchmark");
				BenchmarkRunner.Run<SerializationBenchmark>(new BenchmarkConfig());
			}
		}

		private static bool IsTestRun()
		{
#if DEBUG
			return true;
#else
			return false;
#endif
		}
	}
}