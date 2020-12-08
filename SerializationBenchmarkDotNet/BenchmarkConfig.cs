// Fork from https://github.com/neuecc/MessagePack-CSharp/blob/master/benchmark/SerializerBenchmark/BenchmarkConfig.cs
// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
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
                .RunOncePerIteration()
                .WithGcServer(true)
                .WithGcForce(false);

            // AddJob(baseConfig
            //     .WithRuntime(CoreRuntime.Core31)
            //     .WithPlatform(Platform.X64));
            
            AddJob(baseConfig
                .WithRuntime(CoreRuntime.Core50)
                .WithPlatform(Platform.X64));

            AddColumn(new DataSizeColumn());
            AddExporter(MarkdownExporter.GitHub);
            var processableStyle = new SummaryStyle(CultureInfo.InvariantCulture, false, SizeUnit.KB, TimeUnit.Microsecond, 
                false, true, 100);
            AddExporter(new CsvExporter(CsvSeparator.Comma, processableStyle));
            AddDiagnoser(MemoryDiagnoser.Default);
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
                mi.DeclaringType.GetField(nameof(Benchmark.Target)).SetValue(instance, benchmarkCase.Parameters[1].Value);
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
