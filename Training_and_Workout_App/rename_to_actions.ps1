$baseDir = 'c:\Local Disk D\Lectii\Anul 2\semestrul 2\project11\back\Training_and_Workout_App'

# Pasul 1: Redenumeste fisierele din Interfaces si Structure
$folders = @(
    "$baseDir\Training_and_Workout_App.BusinessLayer\Interfaces",
    "$baseDir\Training_and_Workout_App.BusinessLayer\Structure"
)

foreach ($folder in $folders) {
    Get-ChildItem -Path $folder -Filter '*Service.cs' | ForEach-Object {
        $newName = $_.Name -replace 'Service\.cs$', 'Actions.cs'
        Rename-Item -Path $_.FullName -NewName $newName -Force
        Write-Host "Renamed: $($_.Name) -> $newName"
    }
}

# Pasul 2: Inlocuieste 'Service' cu 'Actions' in toate fisierele .cs (case-sensitive)
$count = 0
Get-ChildItem -Path $baseDir -Recurse -Filter '*.cs' | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw -Encoding UTF8
    if ($content -cmatch 'Service') {
        $newContent = $content -creplace 'Service', 'Actions'
        Set-Content -Path $_.FullName -Value $newContent -Encoding UTF8 -NoNewline
        $count++
        Write-Host "Updated: $($_.Name)"
    }
}

Write-Host ""
Write-Host "Total files updated: $count"
Write-Host "Done!"
