$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$mappedDrive = "W:"
$httpPort = 5000
$httpsPort = 5001

function Stop-ProcessOnPort {
    param(
        [Parameter(Mandatory = $true)]
        [int]$Port
    )

    $connections = Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue
    foreach ($connection in $connections) {
        $processId = $connection.OwningProcess
        if ($processId -and $processId -ne $PID) {
            Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
        }
    }
}

try {
    $existing = (subst) | Where-Object { $_ -like "$mappedDrive*" }
    if (-not $existing) {
        subst $mappedDrive $projectRoot | Out-Null
    }

    Stop-ProcessOnPort -Port $httpPort
    Stop-ProcessOnPort -Port $httpsPort

    $mappedApiPath = Join-Path $mappedDrive "Training_and_Workout_App.API"
    Push-Location $mappedApiPath

    dotnet build ".\Training_and_Workout_App.API.csproj" | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed."
    }

    $env:ASPNETCORE_ENVIRONMENT = "Development"
    $env:ASPNETCORE_URLS = "http://localhost:$httpPort;https://localhost:$httpsPort"

    Write-Host ""
    Write-Host "API starting..."
    Write-Host "Swagger: http://localhost:$httpPort/swagger"
    Write-Host "Swagger HTTPS: https://localhost:$httpsPort/swagger"
    Write-Host ""

    dotnet ".\bin\Debug\net8.0\Training_and_Workout_App.API.dll"
    exit $LASTEXITCODE
}
finally {
    Pop-Location -ErrorAction SilentlyContinue
}
