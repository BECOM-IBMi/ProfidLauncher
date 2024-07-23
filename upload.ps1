$ver = dotnet-gitversion /output json /showvariable SemVer

#Version anlegen
$json = '{"softwareId":"5807e42b-1c7b-47fe-b0a0-fc5bceed4004","version":"' + $ver + '"}'

$resp = curl --header "Content-Type: application/json" --request POST --data $json http://srepo.becom.at/api/software/version
echo $resp
$json = ConvertFrom-Json $resp

if ($LASTEXITCODE -ne 0)
{
	exit $LASTEXITCODE
}

$url = 'http://srepo.becom.at/api/software/version/upload/' + $json.id

$path = (Get-ChildItem .\package\*.zip).FullName
$curlFile = 'file=@' +  $path

curl -X PATCH -F "$curlFile" $url