// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugBenchmarkConfig.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

namespace SerializationBenchmark
{
	public class DebugBenchmarkConfig : ManualConfig
	{
		public DebugBenchmarkConfig()
		{
			AddJob(Job.InProcess
				.RunOncePerIteration()
				.WithGcServer(true)
				.WithGcConcurrent(true)
				.WithGcForce(false));

			AddLogger(ConsoleLogger.Default);
			AddColumnProvider(DefaultColumnProviders.Instance);
			AddColumn(new DataSizeColumn());
		}
	}
}