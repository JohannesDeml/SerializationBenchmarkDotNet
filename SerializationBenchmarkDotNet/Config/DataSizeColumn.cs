// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSizeColumn.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Parameters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SerializationBenchmark
{
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
			instance.Serializer = SetInstanceValueSave(instance.Serializer, paramInstances, nameof(instance.Serializer));
			instance.Target = SetInstanceValueSave(instance.Target, paramInstances, nameof(instance.Target));
			instance.Generic = SetInstanceValueSave(instance.Generic, paramInstances, nameof(instance.Generic));

			instance.PrepareBenchmark();
			var byteSize = instance.Serialize();

			var cultureInfo = summary.GetCultureInfo();
			if (style.PrintUnitsInContent)
			{
				return SizeValue.FromBytes(byteSize).ToString(style.SizeUnit, cultureInfo);
			}

			return byteSize.ToString("0.##", cultureInfo);
		}

		private T SetInstanceValueSave<T>(T current, ParameterInstances instances, string name)
		{
			var instance = instances.Items.FirstOrDefault(item => item.Name == name);
			if (instance == null)
			{
				Console.WriteLine($"{nameof(DataSizeColumn)}: Could not find parameter {name} - skipping instance value change");
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