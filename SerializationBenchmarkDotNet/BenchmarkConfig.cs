// Frok from https://github.com/neuecc/MessagePack-CSharp/blob/master/benchmark/SerializerBenchmark/BenchmarkConfig.cs
// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace DotNetSerializationBenchmark
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            // run quickly:)
            Job baseConfig = Job.ShortRun.WithIterationCount(1).WithWarmupCount(1);

            // Mimic server setup
            baseConfig.WithGcServer(true).WithGcForce(false);
            
            this.Add(baseConfig.With(CoreRuntime.Core31).With(Jit.RyuJit).With(Platform.X64));

            this.Add(MarkdownExporter.GitHub);
            this.Add(CsvExporter.Default);
            this.Add(MemoryDiagnoser.Default);

            this.Add(new DataSizeColumn());
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
                System.Reflection.MethodInfo mi = benchmarkCase.Descriptor.WorkloadMethod;
                if (!mi.Name.Contains("Serialize"))
                {
                    return "-";
                }

                var instance = Activator.CreateInstance(mi.DeclaringType);
                mi.DeclaringType.GetField(nameof(Benchmark.Serializer)).SetValue(instance, benchmarkCase.Parameters[0].Value);
                mi.DeclaringType.GetMethod(nameof(Benchmark.PrepareBenchmark)).Invoke(instance, null);

                var byteSize = (long) mi.Invoke(instance, null);
                
                var cultureInfo = summary.GetCultureInfo();
                if (style.PrintUnitsInContent)
                    return SizeValue.FromBytes(byteSize).ToString(style.SizeUnit, cultureInfo);

                return byteSize.ToString("0.##", cultureInfo);
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
}
