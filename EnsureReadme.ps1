$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $true

[xml]$commonProps = Get-Content (Join-Path $PSScriptRoot "./Common.props")
$ownVersion = $commonProps.Project.PropertyGroup.Version
[xml]$csproj = Get-Content (Join-Path $PSScriptRoot "./SurrogateAttribute.Fody/SurrogateAttribute.Fody.csproj")
$fodyVersion = ($csproj.Project.ItemGroup.PackageReference | Where-Object { $_.Include -eq "FodyHelpers" }).Version
$expected = @"
<PackageReference Include="Fody" Version="$($fodyVersion)" PrivateAssets="All" />
<PackageReference Include="SurrogateAttribute.Fody" Version="$($ownVersion)" PrivateAssets="All" />
"@ -replace "`r`n", "`n"

$readmePath = (Join-Path $PSScriptRoot "./README.md")
$readme = Get-Content $readmePath
$marker = ($readme | Select-String "<!--PACKAGEREFERENCES-->").LineNumber
$start = ($readme | Select-String '```xml' | Where-Object { $_.LineNumber -gt $marker })[0].LineNumber
$end = ($readme | Select-String '```' | Where-Object { $_.LineNumber -gt $start })[0].LineNumber
$actual = ($readme | Select-Object -Skip $start -First $($end - $start - 1)) -join "`n"

if ($expected -ne $actual) {
    $readme = ($readme -join "`n").Replace($actual, $expected)
    $readme | Out-File $readmePath
}