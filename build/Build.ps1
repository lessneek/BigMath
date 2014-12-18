param(
    [string]$config = $null,
    [string]$version = $null
)

if (!$config)
{
    $config = if (Test-Path Env:Configuration) { Get-Content Env:Configuration } else { 'Debug' }
}

if (!$version -and (Test-Path Env:Version)) { $version = Get-Content env:Version };

$here = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)";
psake "$here/Default.ps1" -properties "@{config='$config'; version='$version'}";
