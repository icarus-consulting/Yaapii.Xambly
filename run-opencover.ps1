param(
    [Parameter(Mandatory = $true)]
    [string]$OpenCoverExe,
    [Parameter(Mandatory = $true)]
    [string]$DotNetExe,
    [Parameter(Mandatory = $true)]
    [string]$TestProject,
    [Parameter(Mandatory = $true)]
    [string]$Configuration,
    [Parameter(Mandatory = $true)]
    [string]$OutputFile,
    [Parameter(Mandatory = $true)]
    [string]$WorkingDirectory
)

$psi = New-Object System.Diagnostics.ProcessStartInfo
$psi.FileName = $OpenCoverExe
$psi.Arguments = ('-target:"{0}" -targetargs:"test ""{1}"" --configuration {2} --no-restore --no-build" -output:"{3}" -oldStyle -filter:+[Yaapii.Xambly]*' -f $DotNetExe, $TestProject, $Configuration, $OutputFile)
$psi.WorkingDirectory = $WorkingDirectory
$psi.UseShellExecute = $false
$psi.RedirectStandardOutput = $true
$psi.RedirectStandardError = $true
$psi.EnvironmentVariables.Clear()

$seen = @{}
[System.Environment]::GetEnvironmentVariables().GetEnumerator() | ForEach-Object {
    $name = [string]$_.Key
    $lower = $name.ToLowerInvariant()
    if (-not $seen.ContainsKey($lower)) {
        $seen[$lower] = $true
        [void]$psi.EnvironmentVariables.Add($name, [string]$_.Value)
    }
}

$process = New-Object System.Diagnostics.Process
$process.StartInfo = $psi
[void]$process.Start()
$stdout = $process.StandardOutput.ReadToEnd()
$stderr = $process.StandardError.ReadToEnd()
$process.WaitForExit()

if ($stdout) {
    Write-Output $stdout
}

if ($stderr) {
    Write-Output $stderr
}

exit $process.ExitCode