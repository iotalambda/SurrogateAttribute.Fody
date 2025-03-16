$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $true

$readmePath = (Join-Path $PSScriptRoot "./README.md")
$readme = Get-Content $readmePath

function Compare-Versions {
    [xml]$commonProps = Get-Content (Join-Path $PSScriptRoot "./Common.props")
    $ownVersion = $commonProps.Project.PropertyGroup.Version
    [xml]$csproj = Get-Content (Join-Path $PSScriptRoot "./SurrogateAttribute.Fody/SurrogateAttribute.Fody.csproj")
    $fodyVersion = ($csproj.Project.ItemGroup.PackageReference | Where-Object { $_.Include -eq "FodyHelpers" }).Version
    $expected = @"
<PackageReference Include="Fody" Version="$($fodyVersion)" PrivateAssets="All" />
<PackageReference Include="SurrogateAttribute.Fody" Version="$($ownVersion)" PrivateAssets="All" />
"@ -replace "`r`n", "`n"

    $marker = ($readme | Select-String "<!--PACKAGEREFERENCES-->").LineNumber
    $start = ($readme | Select-String '```xml' | Where-Object { $_.LineNumber -gt $marker })[0].LineNumber
    $end = ($readme | Select-String '```' | Where-Object { $_.LineNumber -gt $start })[0].LineNumber
    $actual = ($readme | Select-Object -Skip $start -First $($end - $start - 1)) -join "`n"
    
    return @{ Actual = $actual; Expected = $expected }
}

function Compare-Examples {
    $exampleCsFile = Get-Content (Join-Path $PSScriptRoot "./Tests/TestAssembly.Readme/Example1.cs")
    $csStart = ($exampleCsFile | Select-String "// SURROGATEATTRIBUTEEXAMPLE_START").LineNumber
    $csEnd = ($exampleCsFile | Select-String "// SURROGATEATTRIBUTEEXAMPLE_END").LineNumber
    $expected = ($exampleCsFile | Select-Object -Skip $csStart -First $($csEnd - $csStart - 1) | ForEach-Object { $_ -replace "^ {4}", "" }) -join "`n"

    $marker = ($readme | Select-String "<!--SURROGATEATTRIBUTEEXAMPLE-->").LineNumber
    $start = ($readme | Select-String '```c#' | Where-Object { $_.LineNumber -gt $marker })[0].LineNumber
    $end = ($readme | Select-String '```' | Where-Object { $_.LineNumber -gt $start })[0].LineNumber
    $actual = ($readme | Select-Object -Skip $start -First $($end - $start - 1)) -join "`n"

    return @{ Actual = $actual; Expected = $expected }
}


$versions = Compare-Versions
$examples = Compare-Examples

if ($versions.Expected -ne $versions.Actual -or $examples.Expected -ne $examples.Actual) {

    $readme = ($readme -join "`n")
    $readme = $readme.Replace($versions.Actual, $versions.Expected)
    $readme = $readme.Replace($examples.Actual, $examples.Expected)
    $readme | Out-File $readmePath
}