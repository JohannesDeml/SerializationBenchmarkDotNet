
dotnet C:\sonarqube\scanner\SonarScanner.MSBuild.dll begin /k:"serialization-benchmark-dotnet" /d:sonar.host.url="http://localhost:9000"
dotnet build SerializationBenchmarkDotNet\SerializationBenchmarkDotNet.csproj --configuration Release
dotnet C:\sonarqube\scanner\SonarScanner.MSBuild.dll end
PAUSE