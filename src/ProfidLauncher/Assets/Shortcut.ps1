[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [String] $target,
    [Parameter(Mandatory=$true)]
    [String] $targetArgs,
    [Parameter(Mandatory=$true)]
    [String] $linkName,
    [Parameter(Mandatory=$true)]
    [String] $icon,
	[Parameter(Mandatory=$true)]
    [String] $workDir
)

$desktopDir = [Environment]::GetFolderPath("Desktop")
$lnk = Join-Path -Path $desktopDir -ChildPath $linkName

Write-Output $target
Write-Output $lnk
Write-Output $icon
Write-Output $workDir

$WScriptShell = New-Object -ComObject WScript.Shell
$shortcut = $WScriptShell.CreateShortcut($lnk)
$shortcut.TargetPath = $target
$shortcut.Arguments = $targetArgs
$shortcut.IconLocation = $icon
$shortcut.WorkingDirectory = $workDir
$shortcut.Save()