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