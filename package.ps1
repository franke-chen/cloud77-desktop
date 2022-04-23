param (
  [Parameter(Mandatory = $true)][string]$tag,
  [Parameter(Mandatory = $true)][string]$version
)

if (!$tag)
{
    echo "no tag given"
    return
}

if (!$version)
{
    echo "no version given"
    return
}

echo 'start to pack App'

$path = '.\WPF\Cloud77.WPF\Properties\AssemblyInfo.cs'

$pattern = '^\[assembly: AssemblyVersion\(".*"\)\]'

$nuspec = ".\Cloud77.WPF.$tag.nuspec"

.\NuGet.exe pack $nuspec -Version $version -OutputDirectory .\nupkg -Properties Configuration=Release

$packageid
Get-Content $nuspec | ForEach-Object {
    if ($_ -match '<id>.*</id>')
    {
        $packageid = $_ -replace("<id>", "")
        $packageid = $packageid -replace("</id>", "")
        $packageid = $packageid -replace("\s", "")
    }
    $_
}

echo $packageid


Start-Process -FilePath ".\squirrel\Squirrel.exe" -ArgumentList "
    --releasify=.\nupkg\$packageid.$version.nupkg 
    --releaseDir=.\releases --framework-version=net472 
    --icon=.\cloud77.ico
    --setupIcon=.\cloud77.ico
    --loadingGif=.\loading.gif" -Wait

Compress-Archive -LiteralPath .\releases -DestinationPath .\releases_new.zip -Force

echo 'App pack is done'