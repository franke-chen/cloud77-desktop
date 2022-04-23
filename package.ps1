param($version)

if (!$version)
{
    echo "no version given"
    return
}

echo 'start to pack App'

#.\NuGet.exe pack .\Cloud77.WPF.nuspec -Version $version -OutputDirectory .\nupkg -Properties Configuration=Release

Start-Process -FilePath ".\squirrel\Squirrel.exe" -ArgumentList "
    --releasify=.\nupkg\Cloud77.WPF.$version.nupkg 
    --releaseDir=.\releases --framework-version=net472 
    --icon=.\cloud77.ico
    --setupIcon=.\cloud77.ico
    --loadingGif=.\loading.gif" -Wait

Compress-Archive -LiteralPath .\releases -DestinationPath .\releases.zip -Force

echo 'App pack is done'