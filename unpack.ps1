$packageid
Get-Content .\Cloud77.WPF.beta.nuspec | ForEach-Object {
    if ($_ -match '<id>.*</id>')
    {
        $packageid = $_ -replace("<id>", "")
        $packageid = $packageid -replace("</id>", "")
        $packageid = $packageid -replace("\s", "")
    }
    $_
}

echo $packageid
echo 'done'

#$nuspec = ".\Cloud77.WPF.$tag.nuspec"
#.\NuGet.exe pack $nuspec -Version $version -OutputDirectory .\nupkg -Properties Configuration=Release

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

$path = '.\WPF\Cloud77.WPF\Properties\AssemblyInfo.cs'
$pattern = '^\[assembly: AssemblyVersion\(".*"\)\]'