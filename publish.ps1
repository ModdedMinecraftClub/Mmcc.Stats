param(
    [Parameter(Mandatory=$true)][string]$platform
)

dotnet publish ./src/Mmcc.Stats -r $platform -c Release /p:PublishSingleFile=true /p:IncludeNativeLibrariesInSingleFile=true --output ./out