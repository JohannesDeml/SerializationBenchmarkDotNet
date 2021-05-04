# Serialization Benchmark .NET

*Benchmark for different serialization libraries for .NET Core /.NET 5*

![Screenshot](./Docs/screenshot.png)

[![Releases](https://img.shields.io/github/release/JohannesDeml/SerializationBenchmarkDotNet/all.svg)](../../releases) [![.NET 5.0](https://img.shields.io/badge/.NET-5.0-blueviolet.svg)](https://dotnet.microsoft.com/download/dotnet/5.0)

## Description

This benchmark compares serialization times, deserialization times and serialization size of different libraries. The benchmark aims to be easily extensible for other serialization targets, while painting an objective picture of the most common serializers for .NET. Additionally, it also compares serializers against manual serialization.

## Libraries

* Manual Serialization with [BitPackingTools](https://github.com/emotitron/BitpackingTools)
* [Protobuf](https://github.com/protocolbuffers/protobuf) (v3.5.18)
* [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) (v2.2.85)
* [NetSerializer](https://github.com/tomba/netserializer) (v4.1.1)
* [FlatBuffers](https://github.com/google/flatbuffers) (v1.12.0 - master [14725d6](https://github.com/google/flatbuffers/commit/14725d6c3b901f210ee08715fb5c359369bcf3ef))
* [Protobuf-Net](https://github.com/protobuf-net/protobuf-net) (v3.0.101)
* [MsgPack-CLI](https://github.com/msgpack/msgpack-cli) (v1.0.1)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) (v13.0.1)

## Benchmarks

![Overview](./Docs/sbn-overview-1.0.0-2-rows.png)
The benchmarks were run with three different setups. The results are shown in the charts above. Newtonsoft.Json was removed from this results, since it takes significantly more time for serialization and deserialization and would have made the chart hard to read.

### Used Hardware

* Ubuntu VPS
  * Virtual private server with dedicated CPU's running - [Hardware](https://www.netcup.eu/bestellen/produkt.php?produkt=2624)
  * Ubuntu 20.04.2 LTS x86-64 Kernel 5.4.0-72-generic

* Ubuntu Desktop / Windows Desktop
  * Desktop PC from 2020 - [Hardware](https://pcpartpicker.com/user/JohannesDeml/saved/zz7yK8)
  * Windows 10 Pro 20H2 x86-64 Build 19042.844
  * Ubuntu 20.04.2 LTS x86-64 Kernel 5.8.0-50-generic

### Targets
* Person: Class with 2 variable length strings, one byte for age and enum Sex with three values
* Vector 3: Struct with 3 floats

### Results
Here are the results from the 5 most promising libraries. All other results can be seen in the overview chart above or can be analyzed in the raw results from the release section. The results presented here are from the Ubuntu VPS setup. The results are cleaned by substracting the retrieved benchmark overhead.  

You can find the raw data of the results in the [release section](../../releases).

The benchmarks are generated with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The results are generated from version 1.0.0

``` ini
BenchmarkDotNet=v0.12.1, OS=ubuntu 20.04
AMD EPYC 7702P, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  Job-TIPVXW : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT

Platform=X64  Runtime=.NET Core 5.0  Concurrent=True  
Force=False  Server=True  IterationTime=250.0000 ms  
MaxIterationCount=20  MinIterationCount=15  UnrollFactor=8  
WarmupCount=3  Version=1.0.0  OS=Linux 5.4.0-72-generic #80-Ubuntu SMP Mon Apr 12 17:35:00 UTC 2021  
DateTime=04/30/2021 11:07:23  SystemTag=Ubuntu VPS  
```

#### Serialization
![Serialization duration](./Docs/sbn-serialize-1.0.0.png)
#### Deserialization
![Overview](./Docs/sbn-deserialize-1.0.0.png)
#### Serialization Size
![Overview](./Docs/sbn-datasize-1.0.0.png)

### Reproduce

Make sure you have [.Net 5 SDK](https://dotnet.microsoft.com/download) installed.  

You can reproduce the results by running `linux-benchmark.sh` on Linux or `win-benchmark.bat` on Windows. The benchmarks will take about 5 minutes to complete.

## Contribute

Do you think a Serializer is missing or want to add a different serialization target? Let's evolve this benchmark together! Either hit me up via [E-mail](mailto:public@deml.io) to discuss your idea, or [open an issue](../../issues), or make a pull request directly. There are a few rules in order to not make the benchmark too cluttered.

### Adding a Library

Make sure your library is either popular or can compete with the serialization seed of the selected libraries. The library also needs to be .NET5 compatible and should be interesting for others.

If your library is available through nuget, add it to the main project through nuget. Otherwise, add a new project and either include a https submodule, or a copy of the repo with a note, which commit was used.

Implement the Serialize logic in the class `./SerializationBenchmarkDotNet/Serializer/YourSerializationLibraryName.cs`. This class should either inherit from `ADirectSerializer` if it uses the `Person.cs` and `Vector3.cs` directly, or `ASerializer` if it creates another format it deserializes to. Take a look at the other serializers to get an idea on how to implement your library.

To test your library, just add it to the list of Serializers in `SerializationBenchmark.cs`. You can debug your serializer by switching to the debug setting. This way, the benchmark will be run in the same process and only once per combination. Make sure your serializer manages to serialize and deserialize all targets and returns the correct serialization sizes.

### Adding a Target

The new target should create interesting results. Either, it shows that a library is better at something that another thing, or it covers a space which is currently not covered by the existing targets.

Implement the target class in `./SerializationBenchmarkDotNet/SerializationTargets/YourTarget.cs`. The class has to implement `IEquatable<Person>, ISerializationTarget`.  To test your library, just add it to the list of Targets in `SerializationBenchmark.cs`. Make sure, all serializers can handle your new target. You will need to add custom code apart from attributes for some libraries (Manual serialization, FlatBuffers, Protobuf).

You can debug your serializer by switching to the debug setting. This way, the benchmark will be run in the same process and only once per combination. Make sure, each library generates serialization and deserialization times. If a library creates wrong results, it will throw an error and therefore no time will be generated.

## License

[MIT](./LICENSE)