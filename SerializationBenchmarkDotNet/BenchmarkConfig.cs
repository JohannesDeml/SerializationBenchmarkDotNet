// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkConfig.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Parameters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace SerializationBenchmark
{
	public class BenchmarkConfig : ManualConfig
	{
		public BenchmarkConfig()
		{
			Job baseConfig = Job.Default
				.WithUnrollFactor(8)
				// Quick run through to check everything is working
				//.RunOncePerIteration()
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

	public class DataSizeColumn : IColumn
	{
		public string Id => "DataSize";

		public string ColumnName => "DataSize";

		public bool AlwaysShow => true;

		public ColumnCategory Category => ColumnCategory.Custom;

		public int PriorityInCategory => int.MaxValue;

		public bool IsNumeric => true;

		public UnitType UnitType => UnitType.Size;

		public string Legend => null;

		public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
		{
			return this.GetValue(summary, benchmarkCase, null);
		}

		public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
		{
			var type = benchmarkCase.Descriptor.Type;
			var report = summary[benchmarkCase];
			System.Reflection.MethodInfo mi = benchmarkCase.Descriptor.WorkloadMethod;

			if (!typeof(ISerializableBenchmark).IsAssignableFrom(type))
			{
				return string.Empty;
			}

			if (!mi.Name.Contains("Serialize"))
			{
				return "-";
			}

			if (!report.Success || !report.BuildResult.IsBuildSuccess || report.ExecuteResults.Count == 0)
			{
				return "NA";
			}

			var instance = Activator.CreateInstance(type) as ISerializableBenchmark;
			if (instance == null)
			{
				return "NA";
			}

			var paramInstances = benchmarkCase.Parameters;
			instance.Serializer = SetInstanceSave(instance.Serializer, paramInstances, nameof(instance.Serializer));
			instance.Target = SetInstanceSave(instance.Target, paramInstances, nameof(instance.Target));
			instance.Generic = SetInstanceSave(instance.Generic, paramInstances, nameof(instance.Generic));

			instance.PrepareBenchmark();
			var byteSize = instance.Serialize();

			var cultureInfo = summary.GetCultureInfo();
			if (style.PrintUnitsInContent)
			{
				return SizeValue.FromBytes(byteSize).ToString(style.SizeUnit, cultureInfo);
			}

			return byteSize.ToString("0.##", cultureInfo);
		}

		private T SetInstanceSave<T>(T current, ParameterInstances instances, string name)
		{
			var instance = instances.Items.FirstOrDefault(item => item.Name == name);
			if (instance == null)
			{
				Console.WriteLine($"Could not find parameter {name}");
				return current;
			}
			
			return (T) instance.Value;
		}
		
		public bool IsAvailable(Summary summary)
		{
			return true;
		}

		public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase)
		{
			return false;
		}
	}
}