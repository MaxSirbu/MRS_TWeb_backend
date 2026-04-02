$ErrorActionPreference = "Stop"

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
            Write-Host "Stopped process $processId on port $Port."
        }
    }
}

Stop-ProcessOnPort -Port 5000
Stop-ProcessOnPort -Port 5001
Stop-ProcessOnPort -Port 5227
Stop-ProcessOnPort -Port 7056
