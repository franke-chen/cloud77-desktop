param (
  [Parameter(Mandatory = $true)][string]$version,
  [Parameter(Mandatory = $true)][string]$package
)

if (!$version)
{
    echo "no version given"
    return
}

if (!$package)
{
    echo "no package given"
    return
}

echo 'start to pack App'


Start-Process -FilePath ".\squirrel\Squirrel.exe" -ArgumentList "
    --releasify=.\nupkg\$package.$version.nupkg 
    --releaseDir=.\releases --framework-version=net472 
    --icon=.\cloud77.ico
    --setupIcon=.\cloud77.ico
    --loadingGif=.\loading.gif" -Wait

Compress-Archive -LiteralPath .\releases -DestinationPath .\releases_new.zip -Force

echo 'App pack is done'