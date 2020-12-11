// --------------------------------------------------------------------------------------------------------------------
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
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using Perfolizer.Horology;

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
				.WithGcForce(false)
				.WithRuntime(CoreRuntime.Core50)
				.WithPlatform(Platform.X64));

			AddColumn(new DataSizeColumn());
			AddColumn(FixedColumn.VersionColumn);
			AddColumn(FixedColumn.OperatingSystemColumn);
			
			AddExporter(MarkdownExporter.GitHub);
			AddExporter(new CsvExporter(CsvSeparator.Comma, ConfigConstants.CsvStyle));
		}
	}
	
	public static class ConfigConstants
	{
		/// <summary>
		/// A summary style that makes processing of data more accessible.
		/// * Long column width to avoid names being truncated
		/// * units stay the same
		/// * No units in cell data (Always numbers)
		/// </summary>
		public static readonly SummaryStyle CsvStyle = new SummaryStyle(CultureInfo.InvariantCulture, false, SizeUnit.B, TimeUnit.Nanosecond,
			false, true, 100);
	}
}