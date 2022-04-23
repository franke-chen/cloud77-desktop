param(
  [Parameter(Mandatory = $true)][string]$tag
)

$content='Cloud77 WPF'
if ($tag = 'alpha')
{
    $content='Cloud77 WPF Alpha'
}
if ($tag = 'beta')
{
    $content='Cloud77 WPF Beta'
}

$path = '.\WPF\Cloud77.WPF\Properties\AssemblyInfo.cs'
$path2 = '.\WPF\Cloud77.WPF\Properties\AssemblyInfoNew.cs'

$pattern1 = '^\[assembly: AssemblyTitle\(".*"\)\]'
$pattern2 = '^\[assembly: AssemblyDescription\(".*"\)\]'
$pattern3 = '^\[assembly: AssemblyProduct\(".*"\)\]'

Get-Content $path | ForEach-Object {
    if ($_ -match $pattern1)
    {
        "[assembly: AssemblyTitle(""$content"")]"
    }
    elseif ($_ -match $pattern2)
    {
        "[assembly: AssemblyDescription(""$content"")]"
    }
    elseif ($_ -match $pattern3)
    {
        "[assembly: AssemblyProduct(""$content"")]"
    }
    else
    {
        $_
    }
} | Set-Content $path2

Move-Item $path2 $path -Force

echo 'done'
