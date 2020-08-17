param(
    [Parameter(Mandatory=$true)][string]$dotnetRuntimeIdentifier
)

$starter_location = Get-Location

try {
    Set-Location ./src/Mmcc.Stats.Frontend

    Write-Host "`nRestoring npm packages...`n"

    npm install

    if($LASTEXITCODE -ne 0) {
        Write-Host "`nError while restoring npm packages. Exiting...`n"
        exit
    }

    Write-Host "`nApplying security fixes...`n"

    npm audit fix

    if($LASTEXITCODE -ne 0) {
        Write-Host "`nError while applying fixes. Exiting...`n"
        exit
    }
}
finally {
    Set-Location $starter_location
}

Write-Host "`nRestored npm packages.`n"
Write-Host "Building the app...`n"

dotnet publish ./src/Mmcc.Stats -r $dotnetRuntimeIdentifier -c Release /p:PublishSingleFile=true /p:IncludeNativeLibrariesInSingleFile=true --output ./out