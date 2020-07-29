dotnet publish ./src -c Release --output ./out
Set-Location ./out
try {
    dotnet Mmcc.Stats.dll
} finally {
    Set-Location ..
}