echo 'start downloading zip'

$url = 'https://www.cloud77.top/statics/squirrel.zip'
Invoke-RestMethod -Uri $url -OutFile ./squirrel.zip

$url = 'https://www.cloud77.top/statics/cloud77-wpf/releases.zip'
Invoke-RestMethod -Uri $url -OutFile ./releases.zip

mkdir squirrel
Expand-Archive -LiteralPath .\squirrel.zip -DestinationPath .\squirrel

mkdir releases
Expand-Archive -LiteralPath .\releases.zip -DestinationPath .\releases

echo 'done...'

Remove-Item ./releases.zip