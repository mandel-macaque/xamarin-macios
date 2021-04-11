param (
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $FileName,

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $FilePath,

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $Description
)

Import-Module ./GitHub.psm1

# we are just doing a simple wrapper so that we can call the functions in the module
# from bash
$obj = New-GistObjectDefinition -Name $FileName -Path $FilePath -Type "markdown"
$url = New-GistWithFiles -Description "$Description" -Files @($obj)
Write-Host $url
