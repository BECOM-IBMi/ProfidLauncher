.\build.ps1 --configuration "Release"

if ($LASTEXITCODE -ne 0)
{
	exit $LASTEXITCODE
}

.\upload.ps1