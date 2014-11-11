Properties {
    $solution_name = "BigMath"
    $config = 'Debug'
    $packageVersion = $null
    $build_dir = Split-Path $psake.build_script_file
    $code_dir = "$build_dir\..\src"
    $solution_path = "$code_dir\$solution_name.sln"
    $assembly_info_path = "$code_dir\CommonAssemblyInfo.cs"
    $nuspec_path = "$code_dir\$solution_name.nuspec"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -depends RebuildAndPack

Task RebuildAndPack -Depends Rebuild, Pack

Task ValidateConfig {
    Write-Host "Build configuration: $config."
    Assert ( 'Debug','Release' -contains $config) -failureMessage "Invalid config: $config; Valid values are 'Debug' and 'Release'."
}

Task Build -depends ValidateConfig -description "Builds outdated artifacts." {
    Write-Host "Building soulution..." -ForegroundColor Green
    Exec { msbuild "$solution_path" /t:Build /p:Configuration=$config /v:quiet } 
}

Task Clean -depends ValidateConfig -description "Deletes all build artifacts." {
    Write-Host "Cleaning solution..." -ForegroundColor Green
    Exec { msbuild "$solution_path" /t:Clean /p:Configuration=$config /v:quiet }
}

Task Rebuild -depends Clean,Build -description "Rebuilds all artifacts from source."

Task Pack -depends Build -description "Packs to a NuGet package." {
    Write-Host "Creating NuGet packages" -ForegroundColor Green
    $packages_dir = "$build_dir\output\$config\"
    if (Test-Path $packages_dir)
    {
        rd $packages_dir -rec -force | out-null
    }
    mkdir $packages_dir | out-null

    if (!$packageVersion)
    {
        Write-Host "Package version is not set, hence fetch version from AssemblyVersion attribute from $assembly_info_path."
        $assembly_info_content = Get-Content $assembly_info_path
        $regex = [regex] 'AssemblyVersion\("(?<Version>[0-9]+(?:\.(?:[0-9]+|\*)){1,3})"\)'
        $packageVersion = $regex.Match($assembly_info_content).Groups['Version'].Value
    }

    Write-Host "Package version: $packageVersion."

    Exec { nuget pack $nuspec_path -Properties "config=$config" -Symbols -Version "$packageVersion" -OutputDirectory "$packages_dir" }
}
