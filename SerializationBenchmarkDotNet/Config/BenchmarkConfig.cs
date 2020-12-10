﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkConfig.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains;
using Perfolizer.Horology;

namespace SerializationBenchmark
{
	public class BenchmarkConfig : ManualConfig
	{
		public BenchmarkConfig()
		{
			Job baseConfig = Job.Default
				.WithUnrollFactor(8)
				.WithGcServer(true)
				.WithGcForce(false);

			// AddJob(baseConfig
			// 	.WithRuntime(CoreRuntime.Core31)
			// 	.WithPlatform(Platform.X64));

			AddJob(baseConfig
				.WithRuntime(CoreRuntime.Core50)
				.WithPlatform(Platform.X64));

			AddColumn(new DataSizeColumn());
			AddExporter(MarkdownExporter.GitHub);
			var processableStyle = new SummaryStyle(CultureInfo.InvariantCulture, false, SizeUnit.KB, TimeUnit.Nanosecond,
				false, true, 100);
			AddExporter(new CsvExporter(CsvSeparator.Comma, processableStyle));
			AddDiagnoser(new EventPipeProfiler(EventPipeProfile.GcVerbose));
		}
	}
}