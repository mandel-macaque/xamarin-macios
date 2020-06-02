<#
    .SYNOPSIS
        Set the mlaunch verbosity to the given value.
    .DESCRIPTION
        Set the mlaunch verbosity to the given value. This
        function overwrittes any already present mlaunch 
        configuration files.
#>
function Set-MLaunchVerbosity {
    param
    (
        [Parameter(Mandatory)]
        [String]
        $Verbosity
    )

    $mlaunchConfigPath = "~/.mlaunch-verbosity"
    if (Test-Path $mlaunchConfigPath -PathType Leaf) {
        Write-Debug "$mlaunchConfigPath found. Content will be overwritten."
    }

    # do not confuse Set-Content with Add-Content, set will override the entire file
    Set-Content -Path $mlaunchConfigPath -Value $Verbosity
}

<#
    .SYNOPSIS
        Ensures that device will be correctly found.
    .DESCRIPTION
        This function re-starts the dameon that will be used
        to find devices. Re-starting it will make sure that new
        devices are correctly found.
#>
function Optimize-DeviceDiscovery {
    Start-Process "launchctl" -ArgumentList "stop com.apple.usbmuxd" -NoNewWindow -PassThru -Wait
}

# module exports
Export-ModuleMember -Function Set-MLaunchVerbosity
Export-ModuleMember -Function Optimize-DeviceDiscovery 