// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkConfig.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace SerializationBenchmark
{
	public class BenchmarkConfig : ManualConfig
	{
		public BenchmarkConfig()
		{
			Add(DefaultConfig.Instance);

			AddJob(Job.Default
				.WithUnrollFactor(8)
				.WithGcServer(true)
				.WithGcConcurrent(true)
				.WithGcForce(false)
				.WithRuntime(CoreRuntime.Core50)
				.WithPlatform(Platform.X64));

			AddColumn(new DataSizeColumn());
			ConfigHelper.AddDefaultColumns(this);
		}
	}
}