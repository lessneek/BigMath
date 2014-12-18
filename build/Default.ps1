Properties {
    $solution_name = "BigMath"
    $config = 'Debug'
    $version = $null
    $build_dir = Split-Path $psake.build_script_file
    $code_dir = "$build_dir\..\src"
    $solution_path = "$code_dir\$solution_name.sln"
    $assembly_info_path = "$code_dir\CommonAssemblyInfo.cs"
    $nuspec_file = "$code_dir\$solution_name.nuspec"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -depends RebuildAndPack

task RebuildAndPack -Depends Rebuild, PackNuGetPackages

Task RestoreNuGetPackages {
    Exec { nuget.exe restore $solution_path }
}

Task ValidateConfig {
    Write-Host "Build configuration: $config."
    Assert ( 'Debug','Release' -contains $config) -failureMessage "Invalid config: $config; Valid values are 'Debug' and 'Release'."
}

Task Build -depends ValidateConfig, RestoreNuGetPackages -description "Builds outdated artifacts." {
    Write-Host "Building soulution..." -ForegroundColor Green
    Exec { msbuild "$solution_path" /t:Build /p:Configuration=$config /v:quiet } 
}

Task Clean -depends ValidateConfig -description "Deletes all build artifacts." {
    Write-Host "Cleaning solution..." -ForegroundColor Green
    Exec { msbuild "$solution_path" /t:Clean /p:Configuration=$config /v:quiet }
}

Task Rebuild -depends Clean,Build -description "Rebuilds all artifacts from source."

Task PackNuGetPackages -depends Rebuild {
    Write-Host "Creating NuGet packages" -ForegroundColor Green
    $packages_dir = "$build_dir\output\$config\"
    if (Test-Path $packages_dir)
    {
        rd $packages_dir -rec -force | out-null
    }
    mkdir $packages_dir | out-null

    if (!$version)
    {
        # assembly version  calculated by GitVersion
        $version = (gitversion | Out-String | ConvertFrom-Json).NuGetVersion
        Write-Host "NuGet Version (calculated by GitVersion): $version"
    }
    else
    {
        Write-Host "NuGet Version: $version"
    }

    Exec { nuget.exe pack $nuspec_file -Version $version -Prop config=$config -Symbols -Verbosity detailed -OutputDirectory "$packages_dir" }
}
