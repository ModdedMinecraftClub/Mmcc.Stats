$stopWatch = New-Object -TypeName System.Diagnostics.Stopwatch
$stopWatch.Start()
$starter_location = Get-Location

Remove-Item ./out -Recurse -ErrorAction Ignore

Write-Host "`nBuilding the ASP.NET Core back-end...`n"

dotnet publish ./src/Mmcc.Stats -c Release --output ./out

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nError while building the ASP.NET Core back-end. Exiting...`n"
    exit
}

Write-Host "`Built the ASP.NET Core back-end.`n"

Set-Location ./src/Mmcc.Stats.Frontend

Write-Host "`nRestoring npm packages...`n"

npm install

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nError while restoring npm packages. Exiting...`n"
    exit
}

Write-Host "`nRestored npm packages.`n"
Write-Host "`nApplying security fixes...`n"

npm audit fix

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nError while applying fixes. Exiting...`n"
    exit
}

Write-Host "`nApplied security fixes.`n"
Write-Host "`nBuilding the Svelte app...`n"

npm run build

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nError while building the Svelte app. Exiting...`n"
    exit
}

Write-Host "`nBuilt the Svelte app.`n"
Write-Host "`nCopying the Svelte bundle..."

Set-Location $starter_location
New-Item -Path ./out -Name "wwwroot" -ItemType "directory"
Copy-Item "./src/Mmcc.Stats.Frontend/public/*" -Destination "./out/wwwroot" -Recurse

Write-Host "`nCopied the Svelte bundle.`n"

$stopWatch.Stop()

Write-Host "`nBuild succeeded in $($stopWatch.Elapsed)`n"
