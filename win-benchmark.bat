:: build and run benchmark for windows
:: Options: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build
:: Build targets: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

dotnet build SerializationBenchmarkDotNet\SerializationBenchmarkDotNet.csproj --configuration Release --framework net6.0 --output .\bin\SerializationBenchmarkDotNet-Windows\
.\bin\SerializationBenchmarkDotNet-Windows\SerializationBenchmarkDotNet

echo off
Echo --- Benchmarks finished ---
Echo Save current process list
:: Folder should exist, just to be sure create it if it does not
if not exist "BenchmarkDotNet.Artifacts" mkdir BenchmarkDotNet.Artifacts

echo on
:: Store currently running processes
tasklist /V /FO CSV > "BenchmarkDotNet.Artifacts\running-processes.csv"
tasklist /V > "BenchmarkDotNet.Artifacts\running-processes.txt"

PAUSE