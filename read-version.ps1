$pattern = '^\[assembly: AssemblyVersion\(".*"\)\]'
$path = '.\WPF\Cloud77.WPF\Properties\AssemblyInfo.cs'
$ver = 'not found'

Get-Content $path | ForEach-Object { 
    if ($_ -match $pattern)
    {
        $ver = $_ 
        $_.ToUpper()
    }
    else
    {
        $_
    }
}

$ver = $ver -replace ("assembly: AssemblyVersion", "")
$ver = $ver -replace ('[\[|\]|\(|\)]|\"', "")
echo $ver