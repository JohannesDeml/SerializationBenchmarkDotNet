// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugBenchmarkConfig.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace SerializationBenchmark
{
	public class DebugBenchmarkConfig : DebugConfig
	{
		public override IEnumerable<Job> GetJobs()
			=> new[]
			{
				Job.InProcess
					.RunOncePerIteration()
					.WithGcServer(true)
					.WithGcForce(false)
			};
	}
}