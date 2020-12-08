#!/bin/bash

# Options: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build
# Build targets: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

dotnet build SerializationBenchmarkDotNet/SerializationBenchmarkDotNet.csproj --configuration Release --framework net5.0 --output ./bin/SerializationBenchmarkDotNet-Linux/
