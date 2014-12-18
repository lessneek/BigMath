param(
    [string]$version = $null
)

$here = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)";
& $here\Build.ps1 -config 'Debug' -version $version;
